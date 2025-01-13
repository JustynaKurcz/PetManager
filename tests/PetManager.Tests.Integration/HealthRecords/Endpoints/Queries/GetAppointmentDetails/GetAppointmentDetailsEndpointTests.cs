using PetManager.Api.Endpoints.HealthRecords;
using PetManager.Application.HealthRecords.Queries.GetAppointmentDetails.DTO;
using PetManager.Tests.Integration.Configuration;
using PetManager.Tests.Integration.HealthRecords.Factories;
using PetManager.Tests.Integration.Pets.Factories;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.HealthRecords.Endpoints.Queries.GetAppointmentDetails;

public class GetAppointmentDetailsEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();
    private readonly PetTestFactory _petFactory = new();
    private readonly HealthRecordTestFactory _healthRecordFactory = new();
    private readonly AppointmentTestFactory _appointmentFactory = new();

    [Fact]
    public async Task get_appointment_details_without_being_authorized_should_return_401_status_code()
    {
        // Arrange
        var query = _appointmentFactory.GetAppointmentDetailsQuery();

        // Act
        var response = await _client.GetAsync(
            HealthRecordEndpoints.GetAppointmentDetails
                .Replace("{healthRecordId:guid}", query.HealthRecordId.ToString())
                .Replace("{appointmentId:guid}", query.AppointmentId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task get_appointment_details_with_valid_data_should_return_200_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        await Authenticate(user.Id, user.Role.ToString());

        var pet = _petFactory.CreatePet(user.Id);
        await AddAsync(pet);

        var appointment = _appointmentFactory.CreateAppointment(pet.HealthRecordId);
        await AddAsync(appointment);

        // Act
        var response = await _client.GetAsync(
            HealthRecordEndpoints.GetAppointmentDetails
                .Replace("{healthRecordId:guid}", pet.HealthRecordId.ToString())
                .Replace("{appointmentId:guid}", appointment.Id.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        await response.Content.ReadFromJsonAsync<AppointmentDetailsDto>().ShouldNotBeNull();
    }

    [Fact]
    public async Task get_appointment_details_given_non_existing_health_record_should_return_400_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        await Authenticate(user.Id, user.Role.ToString());

        var nonExistingHealthRecordId = Guid.NewGuid();
        var appointmentId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync(
            HealthRecordEndpoints.GetAppointmentDetails
                .Replace("{healthRecordId:guid}", nonExistingHealthRecordId.ToString())
                .Replace("{appointmentId:guid}", appointmentId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task get_appointment_details_given_non_existing_appointment_should_return_400_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        await Authenticate(user.Id, user.Role.ToString());

        var pet = _petFactory.CreatePet(user.Id);
        await AddAsync(pet);

        var nonExistingAppointmentId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync(
            HealthRecordEndpoints.GetAppointmentDetails
                .Replace("{healthRecordId:guid}", pet.HealthRecordId.ToString())
                .Replace("{appointmentId:guid}", nonExistingAppointmentId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
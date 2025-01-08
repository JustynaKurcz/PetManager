using PetManager.Api.Endpoints.HealthRecords;
using PetManager.Application.HealthRecords.Commands.AddAppointmentToHealthRecord;
using PetManager.Tests.Integration.Configuration;
using PetManager.Tests.Integration.HealthRecords.Factories;
using PetManager.Tests.Integration.Pets.Factories;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.HealthRecords.Endpoints.Commands.AddAppointmentToHealthRecord;

public class AddAppointmentToHealthRecordEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();
    private readonly PetTestFactory _petFactory = new();
    private readonly HealthRecordTestFactory _healthRecordFactory = new();

    [Fact]
    public async Task add_appointment_to_health_record_without_being_authorized_should_return_401_status_code()
    {
        // Arrange
        var command = _healthRecordFactory.AddAppointmentToHealthRecordCommand();

        // Act
        var response = await _client.PostAsJsonAsync(
            HealthRecordEndpoints.AddAppointment.Replace("{healthRecordId:guid}", command.HealthRecordId.ToString()),
            command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task add_appointment_to_health_record_with_valid_data_should_return_201_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        Authenticate(user.UserId, user.Role.ToString());

        var pet = _petFactory.CreatePet(user.UserId);
        await AddAsync(pet);

        var command = _healthRecordFactory.AddAppointmentToHealthRecordCommand() with
        {
            HealthRecordId = pet.HealthRecordId
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            HealthRecordEndpoints.AddAppointment.Replace("{healthRecordId:guid}",
                pet.HealthRecordId.ToString()),
            command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        await response.Content.ReadFromJsonAsync<AddAppointmentToHealthRecordResponse>().ShouldNotBeNull();
    }

    [Fact]
    public async Task add_appointment_to_health_record_given_non_existing_health_record_should_return_400_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        Authenticate(user.UserId, user.Role.ToString());

        var command = _healthRecordFactory.AddAppointmentToHealthRecordCommand();

        // Act
        var response = await _client.PostAsJsonAsync(
            HealthRecordEndpoints.AddAppointment.Replace("{healthRecordId:guid}", command.HealthRecordId.ToString()),
            command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
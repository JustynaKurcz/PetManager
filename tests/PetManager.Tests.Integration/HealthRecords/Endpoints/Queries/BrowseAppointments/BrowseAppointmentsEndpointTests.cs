using PetManager.Api.Endpoints.HealthRecords;
using PetManager.Tests.Integration.Configuration;
using PetManager.Tests.Integration.HealthRecords.Factories;
using PetManager.Tests.Integration.Pets.Factories;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.HealthRecords.Endpoints.Queries.BrowseAppointments;

public class BrowseAppointmentsEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();
    private readonly PetTestFactory _petFactory = new();
    private readonly AppointmentTestFactory _appointmentFactory = new();

    private const int AppointmentsToCreate = 10;

    [Theory]
    [InlineData(1, 10, "")]
    [InlineData(2, 5, "check")]
    [InlineData(-3, 5, " ")]
    [InlineData(0, 0, ".")]
    public async Task browse_appointments_with_valid_pagination_should_return_200_status_code(
        int pageNumber, int pageSize, string search)
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        await Authenticate(user.Id, user.Role.ToString());

        var pet = _petFactory.CreatePet(user.Id);
        await AddAsync(pet);

        var appointments = await _appointmentFactory.CreateAppointments(pet.HealthRecordId, AppointmentsToCreate);
        await AddRangeAsync(appointments);

        // Act
        var response = await _client.GetAsync(
            HealthRecordEndpoints.BrowseAppointments
                .Replace("{healthRecordId:guid}", pet.HealthRecordId.ToString()) +
            $"?Search={search}&PageNumber={pageNumber}&PageSize={pageSize}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(2, 5)]
    public async Task browse_appointments_with_empty_database_should_return_empty_list(
        int pageNumber, int pageSize)
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        await Authenticate(user.Id, user.Role.ToString());

        var pet = _petFactory.CreatePet(user.Id);
        await AddAsync(pet);

        // Act
        var response = await _client.GetAsync(
            HealthRecordEndpoints.BrowseAppointments
                .Replace("{healthRecordId:guid}", pet.HealthRecordId.ToString()) +
            $"?PageNumber={pageNumber}&PageSize={pageSize}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task browse_appointments_without_being_authorized_should_return_401_status_code()
    {
        // Arrange
        var healthRecordId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync(
            HealthRecordEndpoints.BrowseAppointments
                .Replace("{healthRecordId:guid}", healthRecordId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task browse_appointments_with_non_existing_health_record_should_return_400_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        await Authenticate(user.Id, user.Role.ToString());

        var nonExistingHealthRecordId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync(
            HealthRecordEndpoints.BrowseAppointments
                .Replace("{healthRecordId:guid}", nonExistingHealthRecordId.ToString()) + 
            "?PageNumber=1&PageSize=5");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
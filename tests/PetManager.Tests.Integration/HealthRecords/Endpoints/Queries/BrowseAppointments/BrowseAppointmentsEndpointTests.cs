using PetManager.Api.Endpoints.HealthRecords;
using PetManager.Tests.Integration.Configuration;
using PetManager.Tests.Integration.HealthRecords.Factories;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.HealthRecords.Endpoints.Queries.BrowseAppointments;

public class BrowseAppointmentsEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();
    private readonly AppointmentTestFactory _appointmentFactory = new();

    [Fact] public async Task browse_appointments_without_being_authorized_should_return_401_status_code()
    {
        // Arrange
        var query = _appointmentFactory.BrowseAppointmentsQuery();

        // Act
        var response = await _client.GetAsync(
            HealthRecordEndpoints.BrowseAppointments
                .Replace("{healthRecordId:guid}", query.HealthRecordId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
}
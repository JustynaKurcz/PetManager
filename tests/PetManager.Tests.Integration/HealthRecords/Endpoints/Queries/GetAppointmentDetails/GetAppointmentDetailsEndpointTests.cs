using PetManager.Api.Endpoints.HealthRecords;
using PetManager.Tests.Integration.Configuration;
using PetManager.Tests.Integration.HealthRecords.Factories;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.HealthRecords.Endpoints.Queries.GetAppointmentDetails;

public class GetAppointmentDetailsEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();
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
}
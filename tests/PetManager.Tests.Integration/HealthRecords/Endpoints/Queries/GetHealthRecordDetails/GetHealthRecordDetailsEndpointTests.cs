using PetManager.Api.Endpoints.HealthRecords;
using PetManager.Tests.Integration.HealthRecords.Factories;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.HealthRecords.Endpoints.Queries.GetHealthRecordDetails;

public class GetHealthRecordDetailsEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();
    private readonly HealthRecordTestFactory _healthRecordFactory = new();

    [Fact]
    public async Task get_health_record_details_without_being_authorized_should_return_401_status_code()
    {
        // Arrange
        var query = _healthRecordFactory.GetHealthRecordDetailsQuery();

        // Act
        var response = await _client.GetAsync(
            HealthRecordEndpoints.GetHealthRecordDetails.Replace("{healthRecordId:guid}",
                query.HealthRecordId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
using PetManager.Api.Endpoints.HealthRecords;
using PetManager.Tests.Integration.Configuration;
using PetManager.Tests.Integration.HealthRecords.Factories;
using PetManager.Tests.Integration.Pets.Factories;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.HealthRecords.Endpoints.Queries.BrowseVaccinations;

public class BrowseVaccinationsEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();
    private readonly PetTestFactory _petFactory = new();
    private readonly VaccinationTestFactory _vaccinationFactory = new();

    [Fact]
    public async Task browse_vaccinations_without_being_authorized_should_return_401_status_code()
    {
        // Arrange
        var healthRecordId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync(
            HealthRecordEndpoints.BrowseVaccinations
                .Replace("{healthRecordId:guid}", healthRecordId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task browse_vaccinations_with_valid_data_should_return_200_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        await Authenticate(user.Id, user.Role.ToString());

        var pet = _petFactory.CreatePet(user.Id);
        await AddAsync(pet);

        var vaccinations = await _vaccinationFactory.CreateVaccinations(pet.HealthRecordId, 3);
        await AddRangeAsync(vaccinations);

        // Act
        var response = await _client.GetAsync(
            HealthRecordEndpoints.BrowseVaccinations
                .Replace("{healthRecordId:guid}", pet.HealthRecordId.ToString()) + "?PageNumber=1&PageSize=5");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task browse_vaccinations_with_non_existing_health_record_should_return_400_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        await Authenticate(user.Id, user.Role.ToString());

        var nonExistingHealthRecordId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync(
            HealthRecordEndpoints.BrowseVaccinations
                .Replace("{healthRecordId:guid}", nonExistingHealthRecordId.ToString()) + "?PageNumber=1&PageSize=5");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task browse_vaccinations_with_empty_list_should_return_200_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        await Authenticate(user.Id, user.Role.ToString());

        var pet = _petFactory.CreatePet(user.Id);
        await AddAsync(pet);

        // Act
        var response = await _client.GetAsync(
            HealthRecordEndpoints.BrowseVaccinations
                .Replace("{healthRecordId:guid}", pet.HealthRecordId.ToString()) + "?PageNumber=1&PageSize=5");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
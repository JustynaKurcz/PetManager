using PetManager.Api.Endpoints.HealthRecords;
using PetManager.Application.HealthRecords.Queries.GetVaccinationDetails.DTO;
using PetManager.Tests.Integration.HealthRecords.Factories;
using PetManager.Tests.Integration.Pets.Factories;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.HealthRecords.Endpoints.Queries.GetVaccinationDetails;

public class GetVaccinationDetailsEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();
    private readonly PetTestFactory _petFactory = new();
    private readonly HealthRecordTestFactory _healthRecordFactory = new();
    private readonly VaccinationTestFactory _vaccinationFactory = new();

    [Fact]
    public async Task get_vaccination_details_without_being_authorized_should_return_401_status_code()
    {
        // Arrange
        var query = _vaccinationFactory.GetVaccinationDetailsQuery();

        // Act
        var response = await _client.GetAsync(
            HealthRecordEndpoints.GetVaccinationDetails
                .Replace("{healthRecordId:guid}", query.HealthRecordId.ToString())
                .Replace("{vaccinationId:guid}", query.VaccinationId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task get_vaccination_details_with_valid_data_should_return_200_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        Authenticate(user.UserId, user.Role.ToString());
        
        var pet = _petFactory.CreatePet(user.UserId);
        await AddAsync(pet);
        
        var healthRecord = _healthRecordFactory.CreateHealthRecord(pet.PetId);
        await AddAsync(healthRecord);

        var vaccination = _vaccinationFactory.CreateVaccination(healthRecord.HealthRecordId);
        await AddAsync(vaccination);

        // Act
        var response = await _client.GetAsync(
            HealthRecordEndpoints.GetVaccinationDetails
                .Replace("{healthRecordId:guid}", healthRecord.HealthRecordId.ToString())
                .Replace("{vaccinationId:guid}", vaccination.VaccinationId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        await response.Content.ReadFromJsonAsync<VaccinationDetailsDto>().ShouldNotBeNull();
    }
    
    [Fact]
    public async Task get_vaccination_details_given_non_existing_health_record_should_return_400_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        Authenticate(user.UserId, user.Role.ToString());

        var nonExistingHealthRecordId = Guid.NewGuid();
        var vaccinationId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync(
            HealthRecordEndpoints.GetVaccinationDetails
                .Replace("{healthRecordId:guid}", nonExistingHealthRecordId.ToString())
                .Replace("{vaccinationId:guid}", vaccinationId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task get_vaccination_details_given_non_existing_vaccination_should_return_400_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        Authenticate(user.UserId, user.Role.ToString());
        
        var pet = _petFactory.CreatePet(user.UserId);
        await AddAsync(pet);
        
        var healthRecord = _healthRecordFactory.CreateHealthRecord(pet.PetId);
        await AddAsync(healthRecord);

        var nonExistingVaccinationId = Guid.NewGuid();
        
        // Act
        var response = await _client.GetAsync(
            HealthRecordEndpoints.GetVaccinationDetails
                .Replace("{healthRecordId:guid}", healthRecord.HealthRecordId.ToString())
                .Replace("{vaccinationId:guid}", nonExistingVaccinationId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
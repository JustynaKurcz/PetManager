using PetManager.Api.Endpoints.HealthRecords;
using PetManager.Tests.Integration.Configuration;
using PetManager.Tests.Integration.HealthRecords.Factories;
using PetManager.Tests.Integration.Pets.Factories;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.HealthRecords.Endpoints.Commands.DeleteVaccinationToHealthRecord;

public class DeleteVaccinationToHealthRecordEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();
    private readonly PetTestFactory _petFactory = new();
    private readonly HealthRecordTestFactory _healthRecordFactory = new();
    private readonly VaccinationTestFactory _vaccinationFactory = new();

    [Fact]
    public async Task delete_vaccination_to_health_record_without_being_authorized_should_return_401_status_code()
    {
        // Arrange
        var command = _healthRecordFactory.DeleteVaccinationToHealthRecordCommand();

        // Act
        var response = await _client.DeleteAsync(
            HealthRecordEndpoints.DeleteVaccination.Replace("{healthRecordId:guid}", command.HealthRecordId.ToString())
                .Replace("{vaccinationId:guid}", command.VaccinationId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task delete_vaccination_to_health_record_with_valid_data_should_return_204_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        Authenticate(user.UserId, user.Role.ToString());

        var pet = _petFactory.CreatePet(user.UserId);
        await AddAsync(pet);

        var vaccination = _vaccinationFactory.CreateVaccination(pet.HealthRecordId);
        await AddAsync(vaccination);

        // Act
        var response = await _client.DeleteAsync(
            HealthRecordEndpoints.DeleteVaccination.Replace("{healthRecordId:guid}", pet.HealthRecordId.ToString())
                .Replace("{vaccinationId:guid}", vaccination.VaccinationId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task
        delete_vaccination_to_health_record_given_non_existing_health_record_should_return_400_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        Authenticate(user.UserId, user.Role.ToString());

        var nonExistingHealthRecordId = Guid.NewGuid();
        var vaccinationId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync(
            HealthRecordEndpoints.DeleteVaccination
                .Replace("{healthRecordId:guid}", nonExistingHealthRecordId.ToString())
                .Replace("{vaccinationId:guid}", vaccinationId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task delete_vaccination_to_health_record_given_non_existing_vaccination_should_return_400_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        Authenticate(user.UserId, user.Role.ToString());

        var pet = _petFactory.CreatePet(user.UserId);
        await AddAsync(pet);

        var nonExistingVaccinationId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync(
            HealthRecordEndpoints.DeleteVaccination.Replace("{healthRecordId:guid}", pet.HealthRecordId.ToString())
                .Replace("{vaccinationId:guid}", nonExistingVaccinationId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
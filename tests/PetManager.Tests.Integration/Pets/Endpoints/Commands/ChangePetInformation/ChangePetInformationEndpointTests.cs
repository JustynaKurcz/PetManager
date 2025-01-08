using PetManager.Api.Endpoints.Pets;
using PetManager.Tests.Integration.Configuration;
using PetManager.Tests.Integration.Pets.Factories;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.Pets.Endpoints.Commands.ChangePetInformation;

public class ChangePetInformationEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();
    private readonly PetTestFactory _petFactory = new();

    [Fact]
    public async Task put_change_pet_information_without_being_authorized_should_return_401_status_code()
    {
        // Arrange
        var command = _petFactory.ChangePetInformationCommand();

        // Act
        var response = await _client.PutAsJsonAsync(
            PetEndpoints.ChangePetInformation.Replace("{petId:guid}", command.PetId.ToString()),
            command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task put_change_pet_information_given_non_existing_pet_should_return_400_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        Authenticate(user.UserId, user.Role.ToString());

        var command = _petFactory.ChangePetInformationCommand();

        // Act
        var response = await _client.PutAsJsonAsync(
            PetEndpoints.ChangePetInformation.Replace("{petId:guid}", command.PetId.ToString()),
            command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task put_change_pet_information_given_pet_owned_by_different_user_should_return_400_status_code()
    {
        // Arrange
        var owner = _userFactory.CreateUser();
        await AddAsync(owner);

        var differentUser = _userFactory.CreateUser();
        Authenticate(differentUser.UserId, differentUser.Role.ToString());

        var pet = _petFactory.CreatePet(owner.UserId);
        await AddAsync(pet);

        var command = _petFactory.ChangePetInformationCommand();

        // Act
        var response = await _client.PutAsJsonAsync(
            PetEndpoints.ChangePetInformation.Replace("{petId:guid}", pet.PetId.ToString()),
            command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task put_change_pet_information_given_valid_data_should_return_204_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        Authenticate(user.UserId, user.Role.ToString());

        var pet = _petFactory.CreatePet(user.UserId);
        await AddAsync(pet);

        var command = _petFactory.ChangePetInformationCommand();

        // Act
        var response = await _client.PutAsJsonAsync(
            PetEndpoints.ChangePetInformation.Replace("{petId:guid}", pet.PetId.ToString()),
            command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}
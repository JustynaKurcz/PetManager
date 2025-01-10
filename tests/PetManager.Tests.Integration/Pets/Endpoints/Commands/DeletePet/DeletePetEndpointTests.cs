using PetManager.Api.Endpoints.Pets;
using PetManager.Tests.Integration.Configuration;
using PetManager.Tests.Integration.Pets.Factories;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.Pets.Endpoints.Commands.DeletePet;

public class DeletePetEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();
    private readonly PetTestFactory _petFactory = new();

    [Fact]
    public async Task delete_pet_without_being_authorized_should_return_401_status_code()
    {
        // Arrange
        var command = _petFactory.DeletePetCommand();

        // Act
        var response =
            await _client.DeleteAsync(PetEndpoints.DeletePet.Replace("{petId:guid}", command.PetId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task delete_pet_given_invalid_pet_id_should_return_400_status_code()
    {
        // Arrange
        var command = _petFactory.DeletePetCommand();
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        Authenticate(user.Id, user.Role.ToString());

        // Act
        var response =
            await _client.DeleteAsync(PetEndpoints.DeletePet.Replace("{petId:guid}", command.PetId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task delete_pet_given_valid_pet_id_should_return_204_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        Authenticate(user.Id, user.Role.ToString());

        var pet = _petFactory.CreatePet(user.Id);
        await AddAsync(pet);

        // Act
        var response = await _client.DeleteAsync(PetEndpoints.DeletePet.Replace("{petId:guid}", pet.Id.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}
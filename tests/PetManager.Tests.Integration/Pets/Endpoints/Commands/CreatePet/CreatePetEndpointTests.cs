using PetManager.Api.Endpoints.Pets;
using PetManager.Application.Pets.Commands.CreatePet;
using PetManager.Tests.Integration.Configuration;
using PetManager.Tests.Integration.Pets.Factories;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.Pets.Endpoints.Commands.CreatePet;

public class CreatePetEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();
    private readonly PetTestFactory _petFactory = new();

    [Fact]
    public async Task create_pet_without_being_authorized_should_return_401_status_code()
    {
        // Arrange
        var command = _petFactory.CreatePetCommand();

        // Act
        var response = await _client.PostAsJsonAsync(PetEndpoints.CreatePet, command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_Pet_Given_Valid_Command_Should_Return_201_Status_Code()
    {
        // Arrange
        var command = _petFactory.CreatePetCommand();
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        await Authenticate(user.Id, user.Role.ToString());

        // Act
        var response = await _client.PostAsJsonAsync(PetEndpoints.CreatePet, command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        await response.Content.ReadFromJsonAsync<CreatePetResponse>().ShouldNotBeNull();
    }

    [Fact]
    public async Task create_pet_given_non_existing_user_should_return_400_status_code()
    {
        // Arrange
        var command = _petFactory.CreatePetCommand();
        var user = _userFactory.CreateUser();
        await Authenticate(user.Id, user.Role.ToString());

        // Act
        var response = await _client.PostAsJsonAsync(PetEndpoints.CreatePet, command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
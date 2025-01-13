using PetManager.Api.Endpoints.Pets;
using PetManager.Application.Pets.Queries.GetPetDetails.DTO;
using PetManager.Tests.Integration.Configuration;
using PetManager.Tests.Integration.Pets.Factories;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.Pets.Endpoints.Queries.GetPetDetails;

public class GetPetDetailsEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();
    private readonly PetTestFactory _petFactory = new();

    [Fact]
    public async Task get_pet_without_being_authorized_should_return_401_status_code()
    {
        // Arrange
        var query = _petFactory.GetPetDetailsQuery();

        // Act
        var response =
            await _client.GetAsync(PetEndpoints.GetPetDetails.Replace("{petId:guid}", query.PetId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task get_pet_given_invalid_pet_id_should_return_404_status_code()
    {
        // Arrange
        var query = _petFactory.GetPetDetailsQuery();

        var user = _userFactory.CreateUser();
        await AddAsync(user);
        await Authenticate(user.Id, user.Role.ToString());

        // Act
        var response =
            await _client.GetAsync(PetEndpoints.GetPetDetails.Replace("{petId:guid}", query.PetId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task get_pet_given_valid_pet_id_should_return_200_status_code_with_pet_details()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        await Authenticate(user.Id, user.Role.ToString());

        var pet = _petFactory.CreatePet(user.Id);
        await AddAsync(pet);

        // Act
        var response = await _client.GetAsync(PetEndpoints.GetPetDetails.Replace("{petId:guid}", pet.Id.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        await response.Content.ReadFromJsonAsync<PetDetailsDto>().ShouldNotBeNull();
    }
}
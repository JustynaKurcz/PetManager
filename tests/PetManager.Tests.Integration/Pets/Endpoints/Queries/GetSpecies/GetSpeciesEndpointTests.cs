using PetManager.Api.Endpoints.Pets;
using PetManager.Application.Pets.Queries.GetSpecies.DTO;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.Pets.Endpoints.Queries.GetSpecies;

public class GetSpeciesEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();
    
    [Fact]
    public async Task get_species_without_being_authorized_should_return_401_status_code()
    {
        // Arrange & Act
        var response = await _client.GetAsync(PetEndpoints.GetSpecies);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task get_species_given_authorized_user_should_return_200_status_code_and_genders()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        Authenticate(user.UserId, user.Role.ToString());
   
        // Act
        var response = await _client.GetAsync(PetEndpoints.GetSpecies);
   
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var speciesDto = await response.Content.ReadFromJsonAsync<SpeciesDto>();
        speciesDto.ShouldNotBeNull();
        speciesDto.Species.ShouldNotBeEmpty();
    }
    
}
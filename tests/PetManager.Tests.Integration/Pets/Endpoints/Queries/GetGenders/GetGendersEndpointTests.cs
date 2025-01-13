using PetManager.Api.Endpoints.Pets;
using PetManager.Application.Pets.Queries.GetGenders.DTO;
using PetManager.Tests.Integration.Configuration;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.Pets.Endpoints.Queries.GetGenders;

public class GetGendersEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();

    [Fact]
    public async Task get_genders_without_being_authorized_should_return_401_status_code()
    {
        // Arrange & Act
        var response = await _client.GetAsync(PetEndpoints.GetGenders);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task get_genders_given_authorized_user_should_return_200_status_code_and_genders()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        await Authenticate(user.Id, user.Role.ToString());

        // Act
        var response = await _client.GetAsync(PetEndpoints.GetGenders);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var gendersDto = await response.Content.ReadFromJsonAsync<GendersDto>();
        gendersDto.ShouldNotBeNull();
        gendersDto.Genders.ShouldNotBeEmpty();
    }
}
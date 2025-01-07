using PetManager.Api.Endpoints.Pets;
using PetManager.Tests.Integration.Pets.Factories;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.Pets.Endpoints.Queries.BrowsePets;

public class BrowsePetsEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();
    private readonly PetTestFactory _petFactory = new();

    private const int PetsToCreate = 10;

    [Theory]
    [InlineData(1, 10, "")]
    [InlineData(2, 5, "a")]
    [InlineData(-3, 5, " ")]
    [InlineData(0, 0, ".")]
    public async Task get_browse_pets_with_valid_pagination_should_return_200_status_code(int pageNumber, int pageSize,
        string search)
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);

        var pets = _petFactory.CreatePets(user.UserId, PetsToCreate);
        await AddRangeAsync(pets);

        // Act
        var response = await _client.GetAsync(PetEndpoints.BrowsePets +
                                              $"?Search={search}&PageNumber={pageNumber}&PageSize={pageSize}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(2, 5)]
    public async Task get_browse_pets_with_empty_database_should_return_empty_list(int pageNumber, int pageSize)
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);

        // Act
        var response =
            await _client.GetAsync(PetEndpoints.BrowsePets + $"?PageNumber={pageNumber}&PageSize={pageSize}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
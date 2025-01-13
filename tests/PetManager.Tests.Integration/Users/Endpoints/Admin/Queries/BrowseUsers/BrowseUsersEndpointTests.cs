using PetManager.Api.Endpoints.Users.Admin;
using PetManager.Core.Users.Enums;
using PetManager.Tests.Integration.Configuration;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.Users.Endpoints.Admin.Queries.BrowseUsers;

public class BrowseUsersEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();

    private const int UsersToCreate = 10;

    [Theory]
    [InlineData(1, 10, " ")]
    [InlineData(2, 5, "john")]
    [InlineData(-3, 5, " ")]
    [InlineData(0, 0, ".")]
    public async Task browse_users_with_valid_pagination_should_return_200_status_code(
        int pageNumber, int pageSize, string search)
    {
        // Arrange
        var adminUser = _userFactory.CreateUser(role: UserRole.Admin);
        await AddAsync(adminUser);
        await Authenticate(adminUser.Id, adminUser.Role.ToString());

        var users =  _userFactory.CreateUsers(UsersToCreate);
        await AddRangeAsync(users);

        // Act
        var response = await _client.GetAsync(
            AdminEndpoints.BrowseUsers + 
            $"?Search={Uri.EscapeDataString(search)}&PageNumber={pageNumber}&PageSize={pageSize}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(2, 5)]
    public async Task browse_users_with_empty_database_should_return_empty_list(
        int pageNumber, int pageSize)
    {
        // Arrange
        var adminUser = _userFactory.CreateUser(role: UserRole.Admin);
        await AddAsync(adminUser);
        await Authenticate(adminUser.Id, adminUser.Role.ToString());

        // Act
        var response = await _client.GetAsync(
            AdminEndpoints.BrowseUsers + 
            $"?PageNumber={pageNumber}&PageSize={pageSize}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task browse_users_without_being_authorized_should_return_401_status_code()
    {
        // Act
        var response = await _client.GetAsync(AdminEndpoints.BrowseUsers);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
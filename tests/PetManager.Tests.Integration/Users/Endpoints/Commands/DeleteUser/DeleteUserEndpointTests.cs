using PetManager.Api.Endpoints.Users;
using PetManager.Core.Users.Enums;
using PetManager.Tests.Integration.Configuration;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.Users.Endpoints.Commands.DeleteUser;

public class DeleteUserEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();

    [Fact]
    public async Task delete_user_without_being_authorized_should_return_401_status_code()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync(UserEndpoints.DeleteUser.Replace("{userId:guid}", userId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task delete_user_own_account_should_return_204_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        Authenticate(user.Id, UserRole.User.ToString());

        // Act
        var response =
            await _client.DeleteAsync(UserEndpoints.DeleteUser.Replace("{userId:guid}", user.Id.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task delete_user_other_user_account_should_return_400_status_code()
    {
        // Arrange
        var userId = Guid.NewGuid();
        Authenticate(userId, UserRole.User.ToString());

        var user = _userFactory.CreateUser();
        await AddAsync(user);

        // Act
        var response =
            await _client.DeleteAsync(UserEndpoints.DeleteUser.Replace("{userId:guid}", user.Id.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
using PetManager.Api.Endpoints.Users.Admin;
using PetManager.Core.Users.Enums;
using PetManager.Tests.Integration.Configuration;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.Users.Endpoints.Admin.Commands.DeleteUser;

public class DeleteUserEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();

    [Fact]
    public async Task delete_user_without_being_authorized_should_return_401_status_code()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync(AdminEndpoints.DeleteUser.Replace("{userId:guid}", userId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task delete_non_existing_user_should_return_400_status_code()
    {
        // Arrange
        var admin = _userFactory.CreateUser(role: UserRole.Admin);
        await AddAsync(admin);
        await Authenticate(admin.Id, admin.Role.ToString());

        var nonExistingUserId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync(
            AdminEndpoints.DeleteUser.Replace("{userId:guid}", nonExistingUserId.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task admin_delete_own_account_should_return_400_status_code()
    {
        // Arrange
        var admin =  _userFactory.CreateUser(role: UserRole.Admin);
        await AddAsync(admin);
        await Authenticate(admin.Id, admin.Role.ToString());

        // Act
        var response = await _client.DeleteAsync(
            AdminEndpoints.DeleteUser.Replace("{userId:guid}", admin.Id.ToString()));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
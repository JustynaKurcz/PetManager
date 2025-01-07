using PetManager.Api.Endpoints.Users;
using PetManager.Core.Users.Enums;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.Users.Endpoints.Commands.ChangeUserInformation;

public class ChangeUserInformationEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();

    [Fact]
    public async Task put_change_user_information_without_being_authorized_should_return_401_status_code()
    {
        // Arrange
        var command = _userFactory.ChangeUserInformationCommand();

        // Act
        var response = await _client.PutAsJsonAsync(UserEndpoints.ChangeUserInformation, command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task put_change_user_information_for_non_existing_user_should_return_400_status_code()
    {
        // Arrange
        var nonExistingUserId = Guid.NewGuid();
        var command = _userFactory.ChangeUserInformationCommand();
        Authenticate(nonExistingUserId, UserRole.User.ToString());

        // Act
        var response = await _client.PutAsJsonAsync(UserEndpoints.ChangeUserInformation, command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task put_change_user_information_with_valid_credentials_should_return_204_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        var command = _userFactory.ChangeUserInformationCommand();
        Authenticate(user.UserId, user.Role.ToString());

        // Act
        var response = await _client.PutAsJsonAsync(UserEndpoints.ChangeUserInformation, command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}
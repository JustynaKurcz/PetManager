using PetManager.Api.Endpoints.Users;
using PetManager.Application.Users.Commands.SignIn;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.Users.Endpoints.Commands.SignIn;

public class SignInEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();

    [Fact]
    public async Task post_sign_in_with_valid_credentials_should_return_200_status_code()
    {
        // Arrange
        var command = _userFactory.CreateSignInCommand();
        var user = _userFactory.CreateUser(command.Email, command.Password);
        await AddAsync(user);

        // Act
        var response = await _client.PostAsJsonAsync(UserEndpoints.SignIn, command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        await response.Content.ReadFromJsonAsync<SignInResponse>().ShouldNotBeNull();
    }

    [Fact]
    public async Task post_sign_in_with_invalid_credentials_should_return_400_status_code()
    {
        // Arrange
        var signInCommand = _userFactory.CreateSignInCommand();

        // Act
        var response = await _client.PostAsJsonAsync(UserEndpoints.SignIn, signInCommand);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task post_sign_in_with_invalid_password_should_return_400_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);

        var signInCommand = _userFactory.CreateSignInCommand() with { Email = user.Email };

        // Act
        var response = await _client.PostAsJsonAsync(UserEndpoints.SignIn, signInCommand);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task post_sign_in_with_uppercase_email_should_return_200_status_code()
    {
        // Arrange
        var command = _userFactory.CreateSignInCommand();
        var user = _userFactory.CreateUser(command.Email, command.Password);
        await AddAsync(user);

        command = command with { Email = command.Email.ToUpperInvariant() };

        // Act
        var response = await _client.PostAsJsonAsync(UserEndpoints.SignIn, command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        await response.Content.ReadFromJsonAsync<SignInResponse>().ShouldNotBeNull();
    }
}
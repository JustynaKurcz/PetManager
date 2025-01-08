using PetManager.Api.Endpoints.Users;
using PetManager.Application.Users.Commands.SignUp;
using PetManager.Tests.Integration.Configuration;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.Users.Endpoints.Commands.SignUp;

public class SignUpEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();

    [Fact]
    public async Task post_sign_up_with_valid_credentials_should_return_201_status_code()
    {
        // Arrange
        var command = _userFactory.CreateSignUpCommand();

        // Act
        var response = await _client.PostAsJsonAsync(UserEndpoints.SignUp, command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        await response.Content.ReadFromJsonAsync<SignUpResponse>().ShouldNotBeNull();
    }

    [Fact]
    public async Task post_sign_up_with_existing_email_should_return_400_status_code()
    {
        // Arrange
        var command = _userFactory.CreateSignInCommand();
        var user = _userFactory.CreateUser(command.Email, command.Password);
        await AddAsync(user);

        // Act 
        var response = await _client.PostAsJsonAsync(UserEndpoints.SignUp, command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
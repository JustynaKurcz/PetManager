using PetManager.Api.Endpoints.Users;
using PetManager.Application.Users.Queries.GetCurrentUserDetails.DTO;
using PetManager.Core.Users.Enums;
using PetManager.Tests.Integration.Configuration;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.Users.Endpoints.Queries.GetCurrentUserDetails;

public class GetCurrentUserDetailsEndpointTests : IntegrationTestBase
{
    private readonly UserTestFactory _userFactory = new();

    [Fact]
    public async Task get_current_user_details_without_being_authorized_should_return_401_status_code()
    {
        // Arrange & Act
        var response = await _client.GetAsync(UserEndpoints.GetCurrentUser);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task get_current_user_details_given_authorized_user_should_return_200_status_code_and_user_details()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await AddAsync(user);
        await Authenticate(user.Id, user.Role.ToString());

        // Act
        var response = await _client.GetAsync(UserEndpoints.GetCurrentUser);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        await response.Content.ReadFromJsonAsync<CurrentUserDetailsDto>().ShouldNotBeNull();
    }

    [Fact]
    public async Task get_current_user_details_with_admin_role_should_return_200_status_code_and_user_details()
    {
        // Arrange
        var adminUser = _userFactory.CreateUser(role: UserRole.Admin);
        await AddAsync(adminUser);
        await Authenticate(adminUser.Id, adminUser.Role.ToString());

        // Act 
        var response = await _client.GetAsync(UserEndpoints.GetCurrentUser);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        await response.Content.ReadFromJsonAsync<CurrentUserDetailsDto>().ShouldNotBeNull();
    }
}
using PetManager.Core.Users.Entities;

namespace PetManager.Tests.Unit.Users.Entities;

public class UserTests()
{
    [Theory]
    [InlineData("test@petmanager.com", "TestPassword")]
    [InlineData("test2@petmanager.com", "TestPassword2")]
    [InlineData("test3@petmanager.com", "TestPassword3")]
    public void given_user_data_when_create_user_then_should_create_user(string email, string password)
    {
        // Act
        var user = User.Create(email, password, Role);

        // Assert
        user.ShouldNotBeNull();
        user.ShouldBeOfType<User>();
        user.UserId.ShouldNotBe(Guid.Empty);
    }

    private Role Role => Role.Create("User");
}
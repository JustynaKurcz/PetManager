using PetManager.Core.Users.Entities;

namespace PetManager.Tests.Unit.Entities;

public class UserTests()
{
    [Fact]
    public void given_user_data_when_create_user_then_should_create_user()
    {
        // Arrange

        var email = "test@petmanager.com";
        var password = "TestPassword";

        // Act
        var user = User.Create(email, password, Guid.NewGuid());

        // Assert
        user.ShouldNotBeNull();
        user.ShouldBeOfType<User>();
        user.UserId.ShouldNotBe(Guid.Empty);
    }
}
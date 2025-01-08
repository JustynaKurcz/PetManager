using PetManager.Core.Users.Entities;

namespace PetManager.Tests.Unit.Users.Entities.Valid;

public sealed class UserEntityValidTestData
{
    [Theory]
    [ClassData(typeof(UserValidTestData))]
    public void given_valid_data_when_create_user_then_should_create_new_user(UserParams userParams)
    {
        // Arrange & Act
        var user = Act(userParams);

        // Assert
        user.ShouldNotBeNull();
        user.ShouldBeOfType<User>();
        user.UserId.ShouldNotBe(Guid.Empty);
        user.Email.ShouldBe(userParams.Email);
        user.Password.ShouldBe(userParams.Password);
        user.Role.ShouldBe(userParams.Role);
        user.CreatedAt.ShouldNotBe(default);
    }


    private static User Act(UserParams userParams)
        => User.Create(userParams.Email, userParams.Password, userParams.Role);
}
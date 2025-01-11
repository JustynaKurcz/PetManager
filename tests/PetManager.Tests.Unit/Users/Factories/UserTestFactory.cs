using PetManager.Application.Users.Commands.ChangeUserInformation;
using PetManager.Application.Users.Commands.DeleteUser;
using PetManager.Application.Users.Commands.ForgotPassword;
using PetManager.Application.Users.Commands.ResetPassword;
using PetManager.Application.Users.Commands.SignIn;
using PetManager.Application.Users.Commands.SignUp;
using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Enums;

namespace PetManager.Tests.Unit.Users.Factories;

internal sealed class UserTestFactory
{
    private readonly Faker _faker = new();

    internal User CreateUser()
        => User.Create(_faker.Person.Email, _faker.Internet.Password(), _faker.PickRandom<UserRole>());

    internal SignUpCommand CreateSignUpCommand()
        => new(_faker.Person.Email, _faker.Internet.Password());

    internal SignInCommand CreateSignInCommand()
        => new(_faker.Person.Email, _faker.Internet.Password());

    internal ChangeUserInformationCommand CreateChangeUserInformationCommand()
        => new(_faker.Person.FirstName, _faker.Person.LastName);

    internal ChangeUserInformationCommand CreateChangeUserInformationCommandWithoutFirstNameCommand()
        => new(null, _faker.Person.LastName);

    internal ChangeUserInformationCommand CreateChangeUserInformationCommandWithoutLastNameCommand()
        => new(_faker.Person.FirstName, null);

    internal DeleteUserCommand CreateDeleteUserCommand()
        => new(_faker.Random.Guid());
    
    internal ResetPasswordCommand CreateResetPasswordCommand()
        => new(_faker.Person.Email, _faker.Internet.Password());
    
    internal ForgotPasswordCommand CreateForgotPasswordCommand()
        => new(_faker.Person.Email);
}
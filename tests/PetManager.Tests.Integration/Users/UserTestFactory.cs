using PetManager.Application.Users.Commands.ChangeUserInformation;
using PetManager.Application.Users.Commands.SignIn;
using PetManager.Application.Users.Commands.SignUp;
using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Enums;

namespace PetManager.Tests.Integration.Users;

internal sealed class UserTestFactory
{
    private readonly Faker _faker = new();
    internal User CreateUser()
        => User.Create(_faker.Person.Email, _faker.Internet.Password(), _faker.PickRandom<UserRole>());
    
    internal User CreateUser(string email, string password)
        => User.Create(email, password, UserRole.User);

    internal SignUpCommand CreateSignUpCommand()
        => new(_faker.Internet.Password(), _faker.Person.Email.ToLowerInvariant());

    internal SignInCommand CreateSignInCommand()
        => new(_faker.Person.Email, _faker.Internet.Password());
    
    internal SignInCommand CreateSignInCommand(string email, string password)
        => new(email, password);

    internal ChangeUserInformationCommand ChangeUserInformationCommand()
        => new(_faker.Person.FirstName, _faker.Person.LastName);
}
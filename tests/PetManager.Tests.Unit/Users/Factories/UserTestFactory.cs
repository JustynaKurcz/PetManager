using Bogus;
using PetManager.Application.Users.Commands.ChangeUserInformation;
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
        => new(_faker.Internet.Password(), _faker.Person.Email);

    internal SignInCommand CreateSignInCommand()
        => new(_faker.Person.Email, _faker.Internet.Password());
    
    internal ChangeUserInformationCommand ChangeUserInformationCommand()
        => new(_faker.Person.FirstName, _faker.Person.LastName);
}
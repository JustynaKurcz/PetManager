using Microsoft.AspNetCore.Identity;
using PetManager.Application.Common.Security.Passwords;
using PetManager.Application.Users.Commands.ChangeUserInformation;
using PetManager.Application.Users.Commands.SignIn;
using PetManager.Application.Users.Commands.SignUp;
using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Enums;
using PetManager.Infrastructure.Common.Security.Passwords;

namespace PetManager.Tests.Integration.Users.Factories;

internal sealed class UserTestFactory
{
    private readonly Faker _faker = new Faker();
    private readonly IPasswordManager _passwordManager = new PasswordManager(new PasswordHasher<User>());

    internal User CreateUser(string? email = null, string? password = null, UserRole role = UserRole.User)
        => User.Create(
            email is null ? _faker.Person.Email.ToLowerInvariant() : email.ToLowerInvariant(),
            _passwordManager.HashPassword(password ?? _faker.Internet.Password()),
            role
        );

    internal SignUpCommand CreateSignUpCommand()
        => new(_faker.Person.Email.ToLowerInvariant(), _faker.Internet.Password());

    internal SignInCommand CreateSignInCommand()
        => new(_faker.Person.Email, _faker.Internet.Password());

    internal ChangeUserInformationCommand ChangeUserInformationCommand()
        => new(_faker.Person.FirstName, _faker.Person.LastName);
}
using Microsoft.AspNetCore.Identity;
using PetManager.Application.Security;
using PetManager.Core.Users.Entities;

namespace PetManager.Infrastructure.Security;

internal sealed class PasswordManager(IPasswordHasher<User> passwordHasher) : IPasswordManager
{
    public string HashPassword(string password)
        => passwordHasher.HashPassword(default, password);

    public bool VerifyPassword(string password, string hashedPassword)
        => passwordHasher.VerifyHashedPassword(default, hashedPassword, password) == PasswordVerificationResult.Success;
}
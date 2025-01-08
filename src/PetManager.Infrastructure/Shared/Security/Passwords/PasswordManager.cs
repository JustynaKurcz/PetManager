using PetManager.Application.Shared.Security.Passwords;
using PetManager.Core.Users.Entities;

namespace PetManager.Infrastructure.Shared.Security.Passwords;

internal sealed class PasswordManager(IPasswordHasher<User> passwordHasher) : IPasswordManager
{
    public string HashPassword(string password)
        => passwordHasher.HashPassword(default, password);

    public bool VerifyPassword(string password, string hashedPassword)
        => passwordHasher.VerifyHashedPassword(default, hashedPassword, password) == PasswordVerificationResult.Success;
}
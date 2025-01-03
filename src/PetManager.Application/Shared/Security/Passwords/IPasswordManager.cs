namespace PetManager.Application.Shared.Security.Passwords;

public interface IPasswordManager
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}
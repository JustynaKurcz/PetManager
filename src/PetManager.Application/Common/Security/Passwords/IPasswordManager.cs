namespace PetManager.Application.Common.Security.Passwords;

public interface IPasswordManager
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}
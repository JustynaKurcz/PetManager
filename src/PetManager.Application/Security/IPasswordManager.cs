namespace PetManager.Application.Security;

public interface IPasswordManager
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}
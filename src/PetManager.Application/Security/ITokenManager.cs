namespace PetManager.Application.Security;

public interface ITokenManager
{
    string GenerateToken(Guid userId, string role, string email);
}
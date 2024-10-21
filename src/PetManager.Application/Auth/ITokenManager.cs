namespace PetManager.Application.Auth;

public interface ITokenManager
{
    string GenerateToken(Guid userId, string role, string email);
}
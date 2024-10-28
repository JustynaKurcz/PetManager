namespace PetManager.Application.Auth;

public interface IAuthManager
{
    string GenerateToken(Guid userId, string role);
}
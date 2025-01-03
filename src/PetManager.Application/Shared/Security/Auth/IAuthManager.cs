namespace PetManager.Application.Shared.Security.Auth;

public interface IAuthManager
{
    string GenerateToken(Guid userId, string role);
}
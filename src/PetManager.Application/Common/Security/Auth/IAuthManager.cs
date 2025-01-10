namespace PetManager.Application.Common.Security.Auth;

public interface IAuthManager
{
    Task<string> GenerateToken(Guid userId, string role);
    string GeneratePasswordResetToken(string email);
    bool VerifyPasswordResetToken(string token, out string email);
}
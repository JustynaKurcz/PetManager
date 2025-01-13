namespace PetManager.Application.Common.Security.Authentication;

public interface IAuthenticationManager
{
    Task<string> GenerateToken(Guid userId, string role);
    string GeneratePasswordResetToken(string email);
    bool VerifyPasswordResetToken(string token, out string email);
}
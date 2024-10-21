using PetManager.Application.Auth;

namespace PetManager.Infrastructure.Auth;

internal sealed class TokenManager : ITokenManager
{
    public string GenerateToken(Guid userId, string role, string email)
    {
        // todo dodać generowanie tokena
        return "token";
    }
}
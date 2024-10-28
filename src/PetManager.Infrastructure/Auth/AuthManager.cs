using PetManager.Application.Auth;

namespace PetManager.Infrastructure.Auth;

internal sealed class AuthManager(AuthOptions authOptions) : IAuthManager
{
    public string GenerateToken(Guid userId, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
            new(ClaimTypes.Role, role)
        };

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(authOptions.JwtKey)),
            SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.Add(authOptions.Expiry);
        var jwt = new JwtSecurityToken(authOptions.Issuer, authOptions.Audience, claims, DateTime.UtcNow, expires,
            signingCredentials);
        var token = tokenHandler.WriteToken(jwt);

        return token;
    }
}
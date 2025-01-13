using PetManager.Application.Common.Security.Authentication;
using PetManager.Infrastructure.Common.Security.Authentication.Options;

namespace PetManager.Infrastructure.Common.Security.Authentication.Services;

internal sealed class AuthenticationManager(AuthenticationOptions authenticationOptions) : IAuthenticationManager
{
    private readonly string _key = authenticationOptions.JwtKey;
    private readonly string _issuer = authenticationOptions.Issuer;
    private readonly string _audience = authenticationOptions.Audience;

    public Task<string> GenerateToken(Guid userId, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
            new(ClaimTypes.Role, role)
        };

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_key)),
            SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.Add(authenticationOptions.Expiry);
        var jwt = new JwtSecurityToken(_issuer, _audience, claims, DateTime.UtcNow, expires,
            signingCredentials);
        var token = tokenHandler.WriteToken(jwt);

        return Task.FromResult(token);
    }

    public string GeneratePasswordResetToken(string email)
    {
        var claims = new[]
        {
            new Claim("jti", Guid.NewGuid().ToString()),
            new Claim("purpose", "password_reset"),
            new Claim("email", email)
        };

        var jwt = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public bool VerifyPasswordResetToken(string token, out string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidateAudience = true,
            ValidAudience = _audience,
            ValidateLifetime = true
        };

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            email = jwtToken.Claims.FirstOrDefault(x => x.Type == "email")?.Value ?? string.Empty;

            return true;
        }
        catch
        {
            email = string.Empty;
            return false;
        }
    }
}
namespace PetManager.Infrastructure.Common.Security.Authentication.Options;

public class AuthenticationOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string JwtKey { get; set; }
    public TimeSpan Expiry { get; set; }
}
namespace PetManager.Infrastructure.Auth;

public class AuthOptions
{
    public string Issuer { get; set; }     
    public string Audience { get; set; } 
    public string JwtKey { get; set; } 
    public TimeSpan Expiry { get; set; }  
}
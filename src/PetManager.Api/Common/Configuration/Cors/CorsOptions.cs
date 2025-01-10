namespace PetManager.Api.Common.Configuration.Cors;

public sealed record CorsOptions
{
    public bool AllowCredentials { get; init; }
    public IReadOnlyCollection<string> AllowedOrigins { get; init; } = Array.Empty<string>();
}
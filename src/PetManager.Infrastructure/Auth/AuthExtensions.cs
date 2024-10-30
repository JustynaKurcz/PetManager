using PetManager.Application.Auth;

namespace PetManager.Infrastructure.Auth;

internal static class AuthExtensions
{
    private const string SectionName = "Authentication";

    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var authOptions = new AuthOptions();
        configuration.GetSection(SectionName).Bind(authOptions);

        services.AddSingleton(authOptions);
        services.AddSingleton<IAuthManager, AuthManager>();

        return services;
    }
}
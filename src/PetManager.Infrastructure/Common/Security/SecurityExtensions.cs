using PetManager.Infrastructure.Common.Security.Authentication;
using PetManager.Infrastructure.Common.Security.Authorization;
using PetManager.Infrastructure.Common.Security.Passwords;

namespace PetManager.Infrastructure.Common.Security;

public static class SecurityExtensions
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        AuthorizationExtensions.AddAuthorization(services
                .AddAuthentication(configuration))
            .AddPasswords();

        return services;
    }
}
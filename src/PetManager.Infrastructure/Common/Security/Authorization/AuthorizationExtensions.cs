using PetManager.Core.Users.Enums;
using PetManager.Infrastructure.Common.Security.Authorization.Policies;

namespace PetManager.Infrastructure.Common.Security.Authorization;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(AuthorizationPolicies.RequireAdminRole,
                policy => policy.RequireRole(UserRole.Admin.ToString()))
            .AddPolicy(AuthorizationPolicies.RequireUserRole,
                policy => policy.RequireRole(UserRole.User.ToString(), UserRole.Admin.ToString()));

        return services;
    }
}
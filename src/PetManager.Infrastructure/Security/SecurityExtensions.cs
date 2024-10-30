using PetManager.Application.Security;
using PetManager.Core.Users.Entities;

namespace PetManager.Infrastructure.Security;

internal static class SecurityExtensions
{
    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services
            .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>()
            .AddSingleton<IPasswordManager, PasswordManager>();

        return services;
    }
}
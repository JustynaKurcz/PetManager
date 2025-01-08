using PetManager.Application.Shared.Security.Passwords;
using PetManager.Core.Users.Entities;

namespace PetManager.Infrastructure.Shared.Security.Passwords;

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
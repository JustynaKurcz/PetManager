using PetManager.Application.Common.Security.Passwords;
using PetManager.Core.Users.Entities;

namespace PetManager.Infrastructure.Common.Security.Passwords;

public static class PasswordExtensions
{
    public static IServiceCollection AddPasswords(this IServiceCollection services)
    {
        services
            .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>()
            .AddSingleton<IPasswordManager, PasswordManager>();

        return services;
    }
}
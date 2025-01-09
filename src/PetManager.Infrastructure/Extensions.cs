using System.Reflection;
using System.Runtime.CompilerServices;
using PetManager.Infrastructure.EF;
using PetManager.Infrastructure.EF.DbContext;
using PetManager.Infrastructure.Shared.Context;
using PetManager.Infrastructure.Shared.Emails.Configuration;
using PetManager.Infrastructure.Shared.Exceptions;
using PetManager.Infrastructure.Shared.Security.Auth;
using PetManager.Infrastructure.Shared.Security.Passwords;

[assembly: InternalsVisibleTo("PetManager.Api")]

namespace PetManager.Infrastructure;

internal static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["PostgresConnection"];

        services.AddDbContext<PetManagerDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddSingleton<ExceptionMiddleware>();

        services.AddDataAccess();

        services.AddSecurity();
        services.AddAuth(configuration);
        services.AddContext();
        services.AddEmails(configuration);
 
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}
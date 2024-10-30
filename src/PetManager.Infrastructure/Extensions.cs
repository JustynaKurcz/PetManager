using System.Reflection;
using System.Runtime.CompilerServices;
using PetManager.Infrastructure.Auth;
using PetManager.Infrastructure.Contexts;
using PetManager.Infrastructure.EF;
using PetManager.Infrastructure.EF.DbContext;
using PetManager.Infrastructure.Exceptions;
using PetManager.Infrastructure.Security;

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

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}
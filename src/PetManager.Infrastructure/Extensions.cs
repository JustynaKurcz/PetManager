using System.Reflection;
using System.Runtime.CompilerServices;
using PetManager.Application.Auth;
using PetManager.Core.HealthRecords.Repositories;
using PetManager.Core.Pets.Repositories;
using PetManager.Core.Users.Repositories;
using PetManager.Infrastructure.Auth;
using PetManager.Infrastructure.EF.Context;
using PetManager.Infrastructure.EF.HealthRecords.Repositories;
using PetManager.Infrastructure.EF.Pets.Repositories;
using PetManager.Infrastructure.EF.Users.Repositories;
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

        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IPetRepository, PetRepository>()
            .AddScoped<IHealthRecordRepository, HealthRecordRepository>();

        services.AddSecurity();

        services.AddSingleton<ITokenManager, TokenManager>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }
}
using System.Reflection;
using System.Runtime.CompilerServices;
using PetManager.Infrastructure.Common;
using PetManager.Infrastructure.Common.Context;
using PetManager.Infrastructure.Common.Emails.Configuration;
using PetManager.Infrastructure.Common.Exceptions;
using PetManager.Infrastructure.Common.Integrations.BlobStorage;
using PetManager.Infrastructure.Common.Security.Auth;
using PetManager.Infrastructure.Common.Security.Passwords;
using PetManager.Infrastructure.EF;
using PetManager.Infrastructure.EF.DbContext;

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
        services.AddBlobStorage(configuration);
        
        services.AddHostedService<VaccinationReminderService>();
 
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}
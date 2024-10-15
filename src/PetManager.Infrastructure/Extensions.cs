using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetManager.Infrastructure.EF.Context;

namespace PetManager.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["PostgresConnection"];
        services.AddDbContext<PetManagerContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
    
}
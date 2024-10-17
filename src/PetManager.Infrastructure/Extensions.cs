using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetManager.Core.Users.Repositories;
using PetManager.Infrastructure.EF.Context;
using PetManager.Infrastructure.EF.Users.Repositories;
using PetManager.Infrastructure.EF.Users.Seeder;
using PetManager.Infrastructure.Exceptions;

namespace PetManager.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["PostgresConnection"];
        services.AddDbContext<PetManagerDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddSingleton<ExceptionMiddleware>();

        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IRoleRepository, RoleRepository>();

        services.AddScoped<RoleSeeder>();

        return services;
    }

    public static IApplicationBuilder UseSeeder(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PetManagerDbContext>();

        var roleSeeder = new RoleSeeder(dbContext);
        roleSeeder.Seed();

        return app;
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Repositories;
using PetManager.Infrastructure.EF.Context;
using PetManager.Infrastructure.EF.Users.Repositories;

namespace PetManager.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["PostgresConnection"];
        services.AddDbContext<PetManagerDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        services
            .AddScoped<IUserRepository, UserRepository>();
        
        services
            .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
        
        return services;
    }
    
    
}
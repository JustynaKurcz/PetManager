using Microsoft.AspNetCore.Authentication.JwtBearer;
using PetManager.Application.Shared.Security.Auth;

namespace PetManager.Infrastructure.Shared.Security.Auth;

internal static class AuthExtensions
{
    private const string SectionName = "Authentication";

    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var authOptions = new AuthOptions();
        configuration.GetSection(SectionName).Bind(authOptions);

        services.AddSingleton(authOptions);
        services.AddSingleton<IAuthManager, AuthManager>();

        services
            .AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidIssuer = authOptions.Issuer,
                    ValidAudience = authOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.JwtKey))
                };
            });

        return services;
    }
}
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PetManager.Application.Common.Security.Authentication;
using PetManager.Infrastructure.Common.Security.Authentication.Options;
using PetManager.Infrastructure.Common.Security.Authentication.Services;

namespace PetManager.Infrastructure.Common.Security.Authentication;

internal static class AuthenticationExtensions
{
    private const string SectionName = "Authentication";

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var authOptions = new AuthenticationOptions();
        configuration.GetSection(SectionName).Bind(authOptions);

        services.AddSingleton(authOptions);
        services.AddSingleton<IAuthenticationManager, AuthenticationManager>();

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
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PetManager.Api.Common.Configuration.Swagger;

public static class SwaggerConfiguration
{
    private const string Version = "v1";
    private const string ApiTitle = "PetManager API";
    private const string SecurityScheme = "Bearer";

    public static void AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(swagger =>
        {
            swagger.CustomSchemaIds(x => x.FullName?.Replace("+", "."));
            swagger.SwaggerDoc(Version, new OpenApiInfo
            {
                Title = ApiTitle,
                Version = Version
            });

            ConfigureSwaggerSecurity(swagger);
        });
    }

    private static void ConfigureSwaggerSecurity(SwaggerGenOptions swaggerOptions)
    {
        var securityScheme = new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please insert JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        };

        var securityRequirement = new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = SecurityScheme
                    }
                },
                Array.Empty<string>()
            }
        };

        swaggerOptions.AddSecurityDefinition(SecurityScheme, securityScheme);
        swaggerOptions.AddSecurityRequirement(securityRequirement);
    }
}
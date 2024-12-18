using Swashbuckle.AspNetCore.SwaggerGen;

namespace PetManager.Api.Configuration;

public static class SwaggerConfiguration
{
    public static void AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(swagger =>
        {
            swagger.CustomSchemaIds(x => x.FullName?.Replace("+", "."));
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "PetManager API",
                Version = "v1"
            });

            ConfigureSwaggerSecurity(swagger);
        });
    }

    private static void ConfigureSwaggerSecurity(SwaggerGenOptions swagger)
    {
        swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please insert JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });

        swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    }
}
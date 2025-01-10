namespace PetManager.Api.Common.Configuration.Cors;

public static class CorsConfiguration
{
    private const string DefaultPolicyName = "AllowAngularApp";
    private const string CorsSectionName = "Cors";

    public static void AddCorsConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        var corsOptions = configuration
            .GetSection(CorsSectionName)
            .Get<CorsOptions>() ?? new CorsOptions();

        services
            .AddSingleton(corsOptions)
            .AddCors(options =>
            {
                options.AddPolicy(DefaultPolicyName, corsBuilder =>
                {
                    corsBuilder
                        .WithOrigins(corsOptions.AllowedOrigins.ToArray())
                        .AllowAnyHeader()
                        .AllowAnyMethod();

                    if (corsOptions.AllowCredentials)
                    {
                        corsBuilder.AllowCredentials();
                    }
                });
            });
    }
}
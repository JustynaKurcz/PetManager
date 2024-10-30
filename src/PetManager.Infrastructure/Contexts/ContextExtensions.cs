namespace PetManager.Infrastructure.Contexts;

internal static class ContextExtensions
{
    public static IServiceCollection AddContext(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IContext, Context>();

        return services;
    }
}
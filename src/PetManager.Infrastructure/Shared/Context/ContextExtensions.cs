using Microsoft.AspNetCore.Http;
using PetManager.Application.Shared.Context;

namespace PetManager.Infrastructure.Shared.Context;

internal static class ContextExtensions
{
    public static IServiceCollection AddContext(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IContext, Context>();

        return services;
    }
}
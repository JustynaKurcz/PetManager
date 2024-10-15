using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace PetManager.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}
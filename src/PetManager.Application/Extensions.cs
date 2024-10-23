using System.Runtime.CompilerServices;
using PetManager.Application.Users.Commands.SignUp;

[assembly: InternalsVisibleTo("PetManager.Api")]

namespace PetManager.Application;

internal static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssemblyContaining<SignUpCommandValidator>();

        return services;
    }
}
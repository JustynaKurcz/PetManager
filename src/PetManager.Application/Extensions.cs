using System.Runtime.CompilerServices;
using PetManager.Application.Users.Commands.SignUp;

[assembly: InternalsVisibleTo("PetManager.Api")]
[assembly: InternalsVisibleTo("PetManager.Infrastructure")]

namespace PetManager.Application;

internal static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddValidatorsFromAssemblyContaining<SignUpCommandValidator>();

        return services;
    }
}
using PetManager.Application.Users.Commands.SignUp;

namespace PetManager.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssemblyContaining<SignUpCommandValidator>();

        return services;
    }
}
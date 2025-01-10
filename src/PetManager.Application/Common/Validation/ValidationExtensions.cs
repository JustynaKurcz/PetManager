namespace PetManager.Application.Common.Validation;

public static class ValidationExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services, Assembly assembly)
    {
        var validatorTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)))
            .ToList();

        foreach (var validatorType in validatorTypes)
        {
            var validatorInterface = validatorType.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>));
            services.AddScoped(validatorInterface, validatorType);
        }

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


        return services;
    }
}
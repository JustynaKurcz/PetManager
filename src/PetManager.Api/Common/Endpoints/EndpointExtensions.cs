namespace PetManager.Api.Common.Endpoints;

public static class EndpointExtensions
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder app)
    {
        var endpointDefinitions = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && typeof(IEndpointDefinition).IsAssignableFrom(t));

        foreach (var definition in endpointDefinitions)
        {
            var instance = Activator.CreateInstance(definition) as IEndpointDefinition;
            instance?.DefineEndpoint(app);
        }
    }
}
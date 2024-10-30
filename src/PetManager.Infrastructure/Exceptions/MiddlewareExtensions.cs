namespace PetManager.Infrastructure.Exceptions;

internal static class MiddlewareExtensions
{
    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }
}
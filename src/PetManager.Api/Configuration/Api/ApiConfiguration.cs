using PetManager.Api.Common.Endpoints;
using PetManager.Infrastructure.Shared.Exceptions;

namespace PetManager.Api.Configuration.Api;

public static class ApiConfiguration
{
    private const string DefaultPolicyName = "AllowAngularApp";
    private const string SwaggerEndpoint = "/swagger/v1/swagger.json";
    private const string ApiTitle = "PetManager API";

    public static void UseApiConfiguration(this WebApplication app)
    {
        app.UseHttpsRedirection();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger()
                .UseSwaggerUI(options =>
                    options.SwaggerEndpoint(SwaggerEndpoint, ApiTitle));
        }

        app.UseCors(DefaultPolicyName)
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization();

        app.UseInfrastructure()
            .RegisterEndpoints();
    }
}
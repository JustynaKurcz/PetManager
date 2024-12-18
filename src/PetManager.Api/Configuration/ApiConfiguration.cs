using PetManager.Api.Abstractions;
using PetManager.Infrastructure.Exceptions;

namespace PetManager.Api.Configuration;

public static class ApiConfiguration
{
    public static void UseApiConfiguration(this WebApplication app)
    {
        app.UseHttpsRedirection();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetManager API"));
        }

        app.UseCors("AllowAngularApp");
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseInfrastructure();
        app.RegisterEndpoints();
    }
}
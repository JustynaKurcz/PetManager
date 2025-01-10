using PetManager.Api.Common.Endpoints;
using PetManager.Application.Users.Commands.ForgotPassword;

namespace PetManager.Api.Endpoints.Users.Commands.ForgotPassword;

public sealed class ForgotPasswordEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UserEndpoints.ForgotPassword, async (
                [FromBody] ForgotPasswordCommand command,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                await mediator.Send(command, cancellationToken);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(UserEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Request password reset",
                Description = "This endpoint allows users to request a password reset.",
            });
    }
}
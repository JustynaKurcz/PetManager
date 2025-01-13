using PetManager.Api.Common.Endpoints;

namespace PetManager.Api.Endpoints.Users.Commands.ResetPassword;

public sealed class ResetPasswordEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UserEndpoints.ResetPassword, async (
                [AsParameters] ResetPasswordEndpointRequest request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                await mediator.Send(request.Command, cancellationToken);

                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest)
            .WithTags(UserEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Reset password",
                Description = "This endpoint allows users to reset their password.",
            });
    }
}
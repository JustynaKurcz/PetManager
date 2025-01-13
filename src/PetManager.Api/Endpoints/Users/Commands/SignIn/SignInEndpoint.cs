using PetManager.Api.Common.Endpoints;
using PetManager.Application.Users.Commands.SignIn;

namespace PetManager.Api.Endpoints.Users.Commands.SignIn;

public class SignInEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UserEndpoints.SignIn, async (
                [FromBody] SignInCommand command,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);
                return Results.Ok(result);
            })
            .Produces<SignInResponse>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest)
            .WithTags(UserEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Sign in",
                Description = "This endpoint allows users to sign in to the application.",
            });
    }
}
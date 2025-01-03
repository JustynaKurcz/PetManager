using PetManager.Api.Abstractions;
using PetManager.Application.Users.Commands.SignUp;

namespace PetManager.Api.Endpoints.Users.Commands.SignUp;

internal sealed class SignUpEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(UsersEndpoint.SignUp, async (
                [FromBody] SignUpCommand command,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);
                return Results.Created(UsersEndpoint.Base, result);
            })
            .Produces<SignUpResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(UsersEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Sign up",
                Description = "This endpoint allows users to sign up to the application.",
            });
    }
}
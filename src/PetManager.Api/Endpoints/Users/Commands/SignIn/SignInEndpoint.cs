using PetManager.Api.Abstractions;
using PetManager.Application.Users.Commands.SignIn;

namespace PetManager.Api.Endpoints.Users.Commands.SignIn;

public class SignInEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"{UsersEndpoint.Url}/sign-in", async (
                [FromBody] SignInCommand command, 
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(command, cancellationToken);
                    return Results.Ok(result);
                })
            .Produces<SignInResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(UsersEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Sign in",
                Description = "Sign in to the application",
            });
    }
}
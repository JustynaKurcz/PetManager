using PetManager.Application.Users.Commands.SignIn;

namespace PetManager.Endpoints.Users.Commands.SignIn;

public class SignInEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"{UsersEndpoint.Url}/sign-in",
                async ([FromBody] SignInCommand command, IMediator mediator) =>
                {
                    var result = await mediator.Send(command);
                    return Results.Created("/users/sign-in", result);
                })
            .Produces<SignInResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(UsersEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Sign in",
                Description = "Sign in to the application",
            });
    }
}
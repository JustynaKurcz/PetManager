using PetManager.Application.Users.Commands.SignUp;

namespace PetManager.Endpoints.Users.Commands.SignUp;

public class SignUpEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"{UsersEndpoint.Url}/sign-up",
                async (SignUpCommand command, IMediator mediator) =>
                {
                    var result = await mediator.Send(command);
                    return Results.Created(UsersEndpoint.Url, result);
                })
            .Produces<SignUpResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(UsersEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Sign up",
                Description = "Sign up to the application",
            });
    }
}
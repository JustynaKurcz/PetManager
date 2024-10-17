using MediatR;
using Microsoft.OpenApi.Models;
using PetManager.Abstractions;
using PetManager.Application.Users.Commands.SignUp;

namespace PetManager.Endpoints.Users.Commands.SignUp;

public class SignUpEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/sign-up",
                async (SignUpCommand command, IMediator mediator) =>
                {
                    var result = await mediator.Send(command);
                    return Results.Created("/users/sign-up", result);
                })
            .Produces<SignUpResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags("Users")
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Sign up",
                Description = "Sign up to the application",
            });
    }
}
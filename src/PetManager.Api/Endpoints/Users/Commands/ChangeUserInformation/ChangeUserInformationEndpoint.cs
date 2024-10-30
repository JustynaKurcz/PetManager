using PetManager.Api.Abstractions;
using PetManager.Application.Users.Commands.ChangeUserInformation;

namespace PetManager.Api.Endpoints.Users.Commands.ChangeUserInformation;

internal sealed class ChangeUserInformationEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut($"{UsersEndpoint.Url}", async (
                [FromBody] ChangeUserInformationCommand command,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                await mediator.Send(command, cancellationToken);

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(UsersEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Change User Information",
                Description = "Changes the information of a user."
            })
            .RequireAuthorization();
    }
}
using PetManager.Api.Abstractions;
using PetManager.Application.Users.Commands.DeleteUser;

namespace PetManager.Api.Endpoints.Admin.Commands.DeleteUser;

internal sealed class DeleteUserEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete($"{AdminEndpoint.Url}/users/{{userId:guid}}", async (
                [FromRoute] Guid userId,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                await mediator.Send(new DeleteUserCommand(userId), cancellationToken);
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(AdminEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Delete a user",
                Description = "This endpoint allows admin to delete a user.",
            })
            .RequireAuthorization();
    }
}
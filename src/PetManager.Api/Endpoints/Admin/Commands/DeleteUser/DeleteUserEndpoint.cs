using PetManager.Api.Common.Endpoints;
using PetManager.Application.Users.Commands.DeleteUser;
using PetManager.Infrastructure.Common.Security.Authorization.Policies;

namespace PetManager.Api.Endpoints.Admin.Commands.DeleteUser;

internal sealed class DeleteUserEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(AdminEndpoints.DeleteUser, async (
                [FromRoute] Guid userId,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                await mediator.Send(new DeleteUserCommand(userId), cancellationToken);
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(AdminEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Delete a user",
                Description = "This endpoint allows admin to delete a user.",
            })
            .RequireAuthorization(AuthorizationPolicies.RequireAdminRole);
    }
}
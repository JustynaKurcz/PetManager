using PetManager.Api.Common.Endpoints;
using PetManager.Application.Common.Context;
using PetManager.Application.Users.Commands.DeleteUser;
using PetManager.Infrastructure.Common.Security.Authorization.Policies;

namespace PetManager.Api.Endpoints.Users.Commands.DeleteUser;

internal sealed class DeleteUserEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(UserEndpoints.DeleteUser, async (
                [FromServices] IMediator mediator,
                [FromServices] IContext context,
                CancellationToken cancellationToken) =>
            {
                await mediator.Send(new DeleteUserCommand(context.UserId), cancellationToken);
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(UserEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Delete user",
                Description = "This endpoint allows users to delete their account.",
            })
            .RequireAuthorization(AuthorizationPolicies.RequireUserRole);
    }
}
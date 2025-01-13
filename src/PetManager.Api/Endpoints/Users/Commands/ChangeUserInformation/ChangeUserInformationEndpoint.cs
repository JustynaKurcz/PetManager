using PetManager.Api.Common.Endpoints;
using PetManager.Application.Users.Commands.ChangeUserInformation;
using PetManager.Infrastructure.Common.Security.Authorization.Policies;

namespace PetManager.Api.Endpoints.Users.Commands.ChangeUserInformation;

internal sealed class ChangeUserInformationEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(UserEndpoints.ChangeUserInformation, async (
                [FromBody] ChangeUserInformationCommand command,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                await mediator.Send(command, cancellationToken);

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
           .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(UserEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Change user information",
                Description = "This endpoint allows users to change their information.",
            })
            .RequireAuthorization(AuthorizationPolicies.RequireUserRole);
    }
}
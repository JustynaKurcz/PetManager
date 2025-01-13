using PetManager.Api.Common.Endpoints;
using PetManager.Application.Pets.Commands.DeletePet;
using PetManager.Infrastructure.Common.Security.Authorization.Policies;

namespace PetManager.Api.Endpoints.Pets.Commands.DeletePet;

internal sealed class DeletePetEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(PetEndpoints.DeletePet, async (
                [FromRoute] Guid petId,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                await mediator.Send(new DeletePetCommand(petId), cancellationToken);
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(PetEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Delete a pet",
                Description = "This endpoint allows users to delete a pet.",
            })
            .RequireAuthorization(AuthorizationPolicies.RequireUserRole);
    }
}
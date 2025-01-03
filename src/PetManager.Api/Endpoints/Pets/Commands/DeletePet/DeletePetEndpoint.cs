using PetManager.Api.Common.Endpoints;
using PetManager.Application.Pets.Commands.DeletePet;

namespace PetManager.Api.Endpoints.Pets.Commands.DeletePet;

internal sealed class DeletePetEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(PetsEndpoint.DeletePet, async (
                [FromRoute] Guid petId,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                await mediator.Send(new DeletePetCommand(petId), cancellationToken);
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(PetsEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Delete a pet",
                Description = "This endpoint allows users to delete a pet.",
            })
            .RequireAuthorization();
    }
}
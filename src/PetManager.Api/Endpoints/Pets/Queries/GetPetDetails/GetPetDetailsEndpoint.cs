using PetManager.Api.Abstractions;
using PetManager.Application.Pets.Queries.GetPetDetails;
using PetManager.Application.Pets.Queries.GetPetDetails.DTO;

namespace PetManager.Api.Endpoints.Pets.Queries.GetPetDetails;

public class GetPetDetailsEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("pets/{petId:guid}", async (
                [FromRoute] Guid petId,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var query = new GetPetDetailsQuery(petId);
                var response = await mediator.Send(query, cancellationToken);
                
                return Results.Ok(response);
            })
            .Produces<PetDetailsDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(PetsEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Get Pet Details",
                Description = "Retrieves the details of a pet by its unique identifier."
            });
    }
}
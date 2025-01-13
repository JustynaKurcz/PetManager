using PetManager.Api.Common.Endpoints;
using PetManager.Application.Pets.Queries.GetPetDetails;
using PetManager.Application.Pets.Queries.GetPetDetails.DTO;
using PetManager.Infrastructure.Common.Security.Authorization.Policies;

namespace PetManager.Api.Endpoints.Pets.Queries.GetPetDetails;

internal sealed class GetPetDetailsEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(PetEndpoints.GetPetDetails, async (
                [FromRoute] Guid petId,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var query = new GetPetDetailsQuery(petId);
                var response = await mediator.Send(query, cancellationToken);

                return Results.Ok(response);
            })
            .Produces<PetDetailsDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(PetEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Get pet details",
                Description = "This endpoint allows users to get details of a pet.",
            })
            .RequireAuthorization(AuthorizationPolicies.RequireUserRole);
    }
}
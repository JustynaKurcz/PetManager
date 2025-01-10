using PetManager.Api.Common.Endpoints;
using PetManager.Application.Common.Pagination;
using PetManager.Application.Pets.Queries.BrowsePets;
using PetManager.Application.Pets.Queries.BrowsePets.DTO;

namespace PetManager.Api.Endpoints.Pets.Queries.BrowsePets;

internal sealed class BrowsePetsEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(PetEndpoints.BrowsePets, async (
                [FromServices] IMediator mediator,
                [AsParameters] BrowsePetsQuery query,
                CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(query, cancellationToken);

                return Results.Ok(response);
            })
            .Produces<PaginationResult<PetDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(PetEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Browse pets",
                Description = "This endpoint allows users to browse pets.",
            });
    }
}
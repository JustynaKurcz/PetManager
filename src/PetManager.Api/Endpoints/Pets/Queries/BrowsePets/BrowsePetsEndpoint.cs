using PetManager.Api.Abstractions;
using PetManager.Application.Pagination;
using PetManager.Application.Pets.Queries.BrowsePets;
using PetManager.Application.Pets.Queries.BrowsePets.DTO;

namespace PetManager.Api.Endpoints.Pets.Queries.BrowsePets;

internal sealed class BrowsePetsEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(PetsEndpoint.BrowsePets, async (
                [FromServices] IMediator mediator,
                [AsParameters] BrowsePetsQuery query,
                CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(query, cancellationToken);

                return Results.Ok(response);
            })
            .Produces<PaginationResult<PetDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(PetsEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Browse pets",
                Description = "This endpoint allows users to browse pets.",
            });
    }
}
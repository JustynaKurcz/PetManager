using PetManager.Api.Abstractions;
using PetManager.Application.Pets.Queries.BrowsePets;
using PetManager.Application.Pets.Queries.BrowsePets.DTO;

namespace PetManager.Api.Endpoints.Pets.Queries.BrowsePets;

internal sealed class BrowsePetsEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(PetsEndpoint.Url, async (
                [FromServices] IMediator mediator,
                [FromQuery] string search,
                CancellationToken cancellationToken) =>
            {
                var query = new BrowsePetsQuery(search);
                var response = await mediator.Send(query, cancellationToken);

                return Results.Ok(response);
            })
            .Produces<List<PetDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(PetsEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Browse Pets",
                Description = "Retrieves a list of pets."
            })
            .RequireAuthorization();
    }
}
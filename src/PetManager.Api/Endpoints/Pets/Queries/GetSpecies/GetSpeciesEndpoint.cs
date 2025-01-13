using PetManager.Api.Common.Endpoints;
using PetManager.Application.Pets.Queries.GetSpecies.DTO;
using PetManager.Core.Common.EnumHelper;
using PetManager.Core.Pets.Enums;
using PetManager.Infrastructure.Common.Security.Authorization.Policies;

namespace PetManager.Api.Endpoints.Pets.Queries.GetSpecies;

internal sealed class GetSpeciesEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(PetEndpoints.GetSpecies, async (
                CancellationToken cancellationToken) =>
            {
                var species = EnumHelper.GetEnumValues<Species>();
                var response = new SpeciesDto(species);
                return Results.Ok(response);
            })
            .Produces<SpeciesDto>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(PetEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Get list of species types",
                Description = "This endpoint allows users to get a list of species.",
            })
            .RequireAuthorization(AuthorizationPolicies.RequireUserRole);
    }
}
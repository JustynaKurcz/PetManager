using PetManager.Api.Abstractions;
using PetManager.Application.EnumHelper;
using PetManager.Application.Pets.Queries.GetGenders.DTO;
using PetManager.Core.Pets.Enums;

namespace PetManager.Api.Endpoints.Pets.Queries.GetGenders;

internal sealed class GetGendersEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{PetsEndpoint.Url}/gender-types", async (
                CancellationToken cancellationToken) =>
            {
                var genders = EnumHelper.GetEnumValues<Gender>();
                var response = new GendersDto(genders);
                return Results.Ok(response);
            })
            .Produces<GendersDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(PetsEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Get Genders",
                Description = "Retrieves the list of all available genders."
            })
            .RequireAuthorization();
    }
}
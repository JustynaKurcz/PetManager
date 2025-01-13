using PetManager.Api.Common.Endpoints;
using PetManager.Application.Pets.Queries.GetGenders.DTO;
using PetManager.Core.Common.EnumHelper;
using PetManager.Core.Pets.Enums;
using PetManager.Infrastructure.Common.Security.Authorization.Policies;

namespace PetManager.Api.Endpoints.Pets.Queries.GetGenders;

internal sealed class GetGendersEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(PetEndpoints.GetGenders, async (
                CancellationToken cancellationToken) =>
            {
                var genders = EnumHelper.GetEnumValues<Gender>();
                var response = new GendersDto(genders);
                return Results.Ok(response);
            })
            .Produces<GendersDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(PetEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Get list of gender types",
                Description = "This endpoint allows users to get a list of genders.",
            })
            .RequireAuthorization(AuthorizationPolicies.RequireUserRole);
    }
}
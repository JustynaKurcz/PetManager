using PetManager.Api.Common.Endpoints;
using PetManager.Application.Common.Pagination;
using PetManager.Application.HealthRecords.Queries.BrowseVaccinations;
using PetManager.Application.HealthRecords.Queries.BrowseVaccinations.DTO;
using PetManager.Infrastructure.Common.Security.Authorization.Policies;

namespace PetManager.Api.Endpoints.HealthRecords.Queries.BrowseVaccinations;

internal sealed class BrowseVaccinationsEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(HealthRecordEndpoints.BrowseVaccinations, async (
                [FromServices] IMediator mediator,
                [AsParameters] BrowseVaccinationsQuery query,
                CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(query, cancellationToken);
                return Results.Ok(response);
            })
            .Produces<PaginationResult<VaccinationDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(HealthRecordEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Browse vaccinations",
                Description = "This endpoint allows users to browse vaccinations.",
            })
            .RequireAuthorization(AuthorizationPolicies.RequireUserRole);
    }
}
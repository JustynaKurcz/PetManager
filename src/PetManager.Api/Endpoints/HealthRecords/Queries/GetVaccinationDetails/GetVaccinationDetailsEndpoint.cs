using PetManager.Api.Common.Endpoints;
using PetManager.Application.HealthRecords.Queries.GetVaccinationDetails;
using PetManager.Application.HealthRecords.Queries.GetVaccinationDetails.DTO;
using PetManager.Infrastructure.Common.Security.Authorization.Policies;

namespace PetManager.Api.Endpoints.HealthRecords.Queries.GetVaccinationDetails;

internal sealed class GetVaccinationDetailsEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(HealthRecordEndpoints.GetVaccinationDetails, async (
                [AsParameters] GetVaccinationDetailsEndpointRequest request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var query = new GetVaccinationDetailsQuery(request.HealthRecordId, request.VaccinationId);
                var response = await mediator.Send(query, cancellationToken);

                return Results.Ok(response);
            })
            .Produces<VaccinationDetailsDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(HealthRecordEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Get vaccination details",
                Description = "This endpoint allows users to get details of a vaccination.",
            })
            .RequireAuthorization(AuthorizationPolicies.RequireUserRole);
    }
}
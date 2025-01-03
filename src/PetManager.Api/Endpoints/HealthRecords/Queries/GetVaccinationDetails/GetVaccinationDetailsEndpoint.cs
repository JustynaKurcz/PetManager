using PetManager.Api.Common.Endpoints;
using PetManager.Application.HealthRecords.Queries.GetVaccinationDetails;
using PetManager.Application.HealthRecords.Queries.GetVaccinationDetails.DTO;

namespace PetManager.Api.Endpoints.HealthRecords.Queries.GetVaccinationDetails;

internal sealed class GetVaccinationDetailsEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(HealthRecordsEndpoint.GetVaccinationDetails, async (
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
            .WithTags(HealthRecordsEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Get vaccination details",
                Description = "This endpoint allows users to get details of a vaccination.",
            })
            .RequireAuthorization();
    }
}
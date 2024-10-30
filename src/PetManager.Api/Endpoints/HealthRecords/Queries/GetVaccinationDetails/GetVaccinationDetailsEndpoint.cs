using PetManager.Api.Abstractions;
using PetManager.Application.HealthRecords.Queries.GetVaccinationDetails;
using PetManager.Application.HealthRecords.Queries.GetVaccinationDetails.DTO;

namespace PetManager.Api.Endpoints.HealthRecords.Queries.GetVaccinationDetails;

internal sealed class GetVaccinationDetailsEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                $"{HealthRecordsEndpoint.Url}/{{healthRecordId:guid}}/vaccinations/{{vaccinationId:guid}}",
                async (
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
                Description = "Retrieves the details of an vaccination by its unique identifier."
            })
            .RequireAuthorization();
    }
}
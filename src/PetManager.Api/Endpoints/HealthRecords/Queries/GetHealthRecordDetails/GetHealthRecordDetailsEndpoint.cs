using PetManager.Api.Abstractions;
using PetManager.Application.HealthRecords.Queries.GetHealthRecordDetails;
using PetManager.Application.HealthRecords.Queries.GetHealthRecordDetails.DTO;

namespace PetManager.Api.Endpoints.HealthRecords.Queries.GetHealthRecordDetails;

internal sealed class GetHealthRecordDetailsEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(HealthRecordsEndpoint.GetHealthRecordDetails, async (
                [FromRoute] Guid healthRecordId,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var query = new GetHealthRecordDetailsQuery(healthRecordId);
                var response = await mediator.Send(query, cancellationToken);

                return Results.Ok(response);
            })
            .Produces<HealthRecordDetailsDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(HealthRecordsEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Get health record details",
                Description = "This endpoint allows users to get details of a health record.",
            })
            .RequireAuthorization();
    }
}
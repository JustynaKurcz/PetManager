using PetManager.Api.Abstractions;
using PetManager.Application.HealthRecords.Queries.GetAppointmentDetails;
using PetManager.Application.HealthRecords.Queries.GetAppointmentDetails.DTO;

namespace PetManager.Api.Endpoints.HealthRecords.Queries.GetAppointmentDetails;

internal sealed class GetAppointmentDetailsEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(HealthRecordsEndpoint.GetAppointmentDetails, async (
                [AsParameters] GetAppointmentDetailsEndpointRequest request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAppointmentDetailsQuery(request.HealthRecordId, request.AppointmentId);
                var response = await mediator.Send(query, cancellationToken);

                return Results.Ok(response);
            })
            .Produces<AppointmentDetailsDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(HealthRecordsEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Get appointment details",
                Description = "This endpoint allows users to get details of an appointment.",
            })
            .RequireAuthorization();
    }
}
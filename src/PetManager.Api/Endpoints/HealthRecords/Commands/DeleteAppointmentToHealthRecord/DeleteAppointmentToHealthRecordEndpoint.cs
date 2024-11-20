using PetManager.Api.Abstractions;
using PetManager.Application.HealthRecords.Commands.DeleteAppointmentToHealthRecord;

namespace PetManager.Api.Endpoints.HealthRecords.Commands.DeleteAppointmentToHealthRecord;

internal sealed class DeleteAppointmentToHealthRecordEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                $"{HealthRecordsEndpoint.Url}/{{healthRecordId:guid}}/appointments/{{appointmentId:guid}}",
                async (
                    [AsParameters] DeleteAppointmentToHealthRecordEndpointRequest request,
                    [FromServices] IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var command =
                        new DeleteAppointmentToHealthRecordCommand(request.HealthRecordId, request.AppointmentId);
                    await mediator.Send(command, cancellationToken);

                    return Results.NoContent();
                })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(HealthRecordsEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Delete an appointment to a health record",
                Description = "This endpoint allows users to delete an appointment to a health record.",
            })
            .RequireAuthorization();
    }
}
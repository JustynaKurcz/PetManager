using PetManager.Api.Abstractions;
using PetManager.Application.HealthRecords.Commands.AddAppointmentToHealthRecord;

namespace PetManager.Api.Endpoints.HealthRecords.Commands.AddAppointmentToHealthRecord;

internal sealed class AddAppointmentToHealthRecordEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"{HealthRecordsEndpoint.Url}/{{healthRecordId:guid}}/appointments", async (
                [AsParameters] AddAppointmentToHealthRecordEndpointRequest request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = request.Command with { HealthRecordId = request.HealthRecordId };
                var response = await mediator.Send(command, cancellationToken);

                return Results.Created(HealthRecordsEndpoint.Url, response);
            })
            .Produces<AddAppointmentToHealthRecordResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(HealthRecordsEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Add an appointment to a health record",
                Description = "This endpoint allows users to add an appointment to a health record.",
            })
            .RequireAuthorization();
    }
}
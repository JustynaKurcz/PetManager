using PetManager.Api.Common.Endpoints;
using PetManager.Application.HealthRecords.Commands.AddAppointmentToHealthRecord;

namespace PetManager.Api.Endpoints.HealthRecords.Commands.AddAppointmentToHealthRecord;

internal sealed class AddAppointmentToHealthRecordEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(HealthRecordEndpoints.AddAppointment, async (
                [AsParameters] AddAppointmentToHealthRecordEndpointRequest request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = request.Command with { HealthRecordId = request.HealthRecordId };
                var response = await mediator.Send(command, cancellationToken);

                return Results.Created(HealthRecordEndpoints.Base, response);
            })
            .Produces<AddAppointmentToHealthRecordResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(HealthRecordEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Add an appointment to a health record",
                Description = "This endpoint allows users to add an appointment to a health record.",
            })
            .RequireAuthorization();
    }
}
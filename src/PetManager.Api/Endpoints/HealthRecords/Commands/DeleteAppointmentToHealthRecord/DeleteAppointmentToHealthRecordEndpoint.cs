using PetManager.Api.Common.Endpoints;
using PetManager.Application.HealthRecords.Commands.DeleteAppointmentToHealthRecord;
using PetManager.Infrastructure.Common.Security.Authorization.Policies;

namespace PetManager.Api.Endpoints.HealthRecords.Commands.DeleteAppointmentToHealthRecord;

internal sealed class DeleteAppointmentToHealthRecordEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(HealthRecordEndpoints.DeleteAppointment, async (
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
            .WithTags(HealthRecordEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Delete an appointment to a health record",
                Description = "This endpoint allows users to delete an appointment to a health record.",
            })
            .RequireAuthorization(AuthorizationPolicies.RequireUserRole);
    }
}
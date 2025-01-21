using PetManager.Api.Common.Endpoints;
using PetManager.Infrastructure.Common.Security.Authorization.Policies;

namespace PetManager.Api.Endpoints.HealthRecords.Commands.ChangeAppointmentInformation;

internal sealed class ChangeAppointmentInformationEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(HealthRecordEndpoints.ChangeAppointmentInformation, async (
                [AsParameters] ChangeAppointmentInformationEndpointRequest request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = request.Command with { HealthRecordId = request.HealthRecordId, AppointmentId = request.AppointmentId };
                await mediator.Send(command, cancellationToken);

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(HealthRecordEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Change appointment information",
                Description = "This endpoint allows users to change appointment information.",
            })
            .RequireAuthorization(AuthorizationPolicies.RequireUserRole);
    }
}
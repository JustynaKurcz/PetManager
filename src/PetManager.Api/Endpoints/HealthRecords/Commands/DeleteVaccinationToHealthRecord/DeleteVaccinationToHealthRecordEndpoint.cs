using PetManager.Api.Common.Endpoints;
using PetManager.Application.HealthRecords.Commands.DeleteVaccinationToHealthRecord;
using PetManager.Infrastructure.Common.Security.Authorization.Policies;

namespace PetManager.Api.Endpoints.HealthRecords.Commands.DeleteVaccinationToHealthRecord;

internal sealed class DeleteVaccinationToHealthRecordEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(HealthRecordEndpoints.DeleteVaccination, async (
                [AsParameters] DeleteVaccinationToHealthRecordEndpointRequest request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command =
                    new DeleteVaccinationToHealthRecordCommand(request.HealthRecordId, request.VaccinationId);
                await mediator.Send(command, cancellationToken);

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(HealthRecordEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Delete a vaccination to a health record",
                Description = "This endpoint allows users to delete a vaccination to a health record.",
            })
            .RequireAuthorization(AuthorizationPolicies.RequireUserRole);
    }
}
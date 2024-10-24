using PetManager.Api.Abstractions;
using PetManager.Application.HealthRecords.Commands.DeleteVaccinationToHealthRecord;

namespace PetManager.Api.Endpoints.HealthRecords.Commands.DeleteVaccinationToHealthRecord;

internal sealed class DeleteVaccinationToHealthRecordEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                $"{HealthRecordsEndpoint.Url}/{{healthRecordId:guid}}/vaccinations/{{vaccinationId:guid}}",
                async (
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
            .WithTags(HealthRecordsEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Delete Vaccination to Health Record",
                Description = "Delete a vaccination to a health record."
            });
    }
}
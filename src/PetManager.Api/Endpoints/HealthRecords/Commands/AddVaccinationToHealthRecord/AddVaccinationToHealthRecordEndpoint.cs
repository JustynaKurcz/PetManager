using PetManager.Api.Abstractions;
using PetManager.Application.HealthRecords.Commands.AddVaccinationToHealthRecord;

namespace PetManager.Api.Endpoints.HealthRecords.Commands.AddVaccinationToHealthRecord;

internal sealed class AddVaccinationToHealthRecordEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"{HealthRecordsEndpoint.Url}/{{healthRecordId:guid}}/vaccinations", async (
                [AsParameters] AddVaccinationToHealthRecordEndpointRequest request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = request.Command with { HealthRecordId = request.HealthRecordId };
                var response = await mediator.Send(command, cancellationToken);

                return Results.Created(HealthRecordsEndpoint.Url, response);
            })
            .Produces<AddVaccinationToHealthRecordResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(HealthRecordsEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Add Vaccination to Health Record",
                Description = "Adds a vaccination to a health record."
            });
    }
}
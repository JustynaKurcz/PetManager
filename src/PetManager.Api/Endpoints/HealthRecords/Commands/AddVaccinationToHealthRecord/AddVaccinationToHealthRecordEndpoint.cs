using PetManager.Api.Common.Endpoints;
using PetManager.Application.HealthRecords.Commands.AddVaccinationToHealthRecord;

namespace PetManager.Api.Endpoints.HealthRecords.Commands.AddVaccinationToHealthRecord;

internal sealed class AddVaccinationToHealthRecordEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(HealthRecordEndpoints.AddVaccination, async (
                [AsParameters] AddVaccinationToHealthRecordEndpointRequest request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = request.Command with { HealthRecordId = request.HealthRecordId };
                var response = await mediator.Send(command, cancellationToken);

                return Results.Created(HealthRecordEndpoints.Base, response);
            })
            .Produces<AddVaccinationToHealthRecordResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(HealthRecordEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Add a vaccination to a health record",
                Description = "This endpoint allows users to add a vaccination to a health record.",
            })
            .RequireAuthorization();
    }
}
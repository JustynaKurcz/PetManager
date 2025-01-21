using PetManager.Api.Common.Endpoints;
using PetManager.Infrastructure.Common.Security.Authorization.Policies;

namespace PetManager.Api.Endpoints.HealthRecords.Commands.ChangeVaccinationInformation;

internal sealed class ChangeVaccinationInformationEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(HealthRecordEndpoints.ChangeVaccinationInformation, async (
                [AsParameters] ChangeVaccinationInformationEndpointRequest request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = request.Command with { HealthRecordId = request.HealthRecordId, VaccinationId = request.VaccinationId };
                await mediator.Send(command, cancellationToken);

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(HealthRecordEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Change vaccination information",
                Description = "This endpoint allows users to change vaccination information.",
            })
            .RequireAuthorization(AuthorizationPolicies.RequireUserRole);
    }
    
}
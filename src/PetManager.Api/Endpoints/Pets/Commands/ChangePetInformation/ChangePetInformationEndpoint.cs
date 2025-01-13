using PetManager.Api.Common.Endpoints;
using PetManager.Infrastructure.Common.Security.Authorization.Policies;

namespace PetManager.Api.Endpoints.Pets.Commands.ChangePetInformation;

internal sealed class ChangePetInformationEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(PetEndpoints.ChangePetInformation, async (
                [AsParameters] ChangePetInformationEndpointRequest request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = request.Command with { PetId = request.PetId };
                await mediator.Send(command, cancellationToken);

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
           .Produces(StatusCodes.Status400BadRequest)
            .WithTags(PetEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Change pet information",
                Description = "This endpoint allows users to change the information of a pet.",
            })
            .RequireAuthorization(AuthorizationPolicies.RequireUserRole);
    }
}
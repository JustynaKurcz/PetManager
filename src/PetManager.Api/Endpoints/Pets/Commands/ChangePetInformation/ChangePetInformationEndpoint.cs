using PetManager.Api.Abstractions;

namespace PetManager.Api.Endpoints.Pets.Commands.ChangePetInformation;

internal sealed class ChangePetInformationEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut($"{PetsEndpoint.Url}/{{petId:guid}}", async (
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
            .WithTags(PetsEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Change Pet Information",
                Description = "Changes the information of a pet."
            })
            .RequireAuthorization();
    }
}
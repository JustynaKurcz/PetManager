using PetManager.Api.Common.Endpoints;
using PetManager.Application.Pets.Commands.AddImageToPet;
using PetManager.Infrastructure.Common.Security.Authorization.Policies;

namespace PetManager.Api.Endpoints.Pets.Commands.AddImageToPet;

internal sealed class AddImageToPetEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(PetEndpoints.AddImageToPet, async (
                [FromForm] IFormFile file,
                [FromRoute] Guid petId,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = new AddImageToPetCommand(petId, file);
                var response = await mediator.Send(command, cancellationToken);

                return Results.Ok(response);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(PetEndpoints.Tag)
            .DisableAntiforgery()
            .Accepts<IFormFile>("multipart/form-data")
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Add image to pet",
                Description = "This endpoint allows users to add an image to a pet.",
            })
            .RequireAuthorization(AuthorizationPolicies.RequireUserRole);
    }
}
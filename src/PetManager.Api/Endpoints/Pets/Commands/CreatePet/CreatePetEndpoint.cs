using PetManager.Api.Common.Endpoints;
using PetManager.Application.Pets.Commands.CreatePet;
using PetManager.Infrastructure.Common.Security.Authorization.Policies;

namespace PetManager.Api.Endpoints.Pets.Commands.CreatePet;

internal sealed class CreatePetEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(PetEndpoints.CreatePet, async (
                [FromBody] CreatePetCommand command,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(command, cancellationToken);
                return Results.Created(PetEndpoints.Base, response);
            })
            .Produces<CreatePetResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(PetEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Create a pet",
                Description = "This endpoint allows users to create a pet.",
            })
            .RequireAuthorization(AuthorizationPolicies.RequireUserRole);
    }
}
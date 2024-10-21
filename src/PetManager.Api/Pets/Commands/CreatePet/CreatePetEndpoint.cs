using PetManager.Application.Pets.Commands.CreatePet;

namespace PetManager.Pets.Commands.CreatePet;

public class CreatePetEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"{PetsEndpoint.Url}",
                async ([FromBody] CreatePetCommand command, IMediator mediator) =>
                {
                    var response = await mediator.Send(command);
                    return Results.Created(PetsEndpoint.Url, response);
                })
            .Produces<CreatePetResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(PetsEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Create a pet",
                Description = "Create a pet in the application",
            });
    }
}
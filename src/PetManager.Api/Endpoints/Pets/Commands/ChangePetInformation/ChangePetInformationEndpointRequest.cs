using PetManager.Application.Pets.Commands.ChangePetInformation;

namespace PetManager.Api.Endpoints.Pets.Commands.ChangePetInformation;

internal sealed class ChangePetInformationEndpointRequest
{
    [FromRoute(Name = "petId")] public Guid PetId { get; init; }
    [FromBody] public ChangePetInformationCommand Command { get; init; } = default!;
}
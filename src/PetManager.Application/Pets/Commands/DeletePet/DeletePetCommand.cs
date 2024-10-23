namespace PetManager.Application.Pets.Commands.DeletePet;

internal record DeletePetCommand(Guid PetId) : IRequest;
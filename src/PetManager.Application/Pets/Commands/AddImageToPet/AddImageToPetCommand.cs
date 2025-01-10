using Microsoft.AspNetCore.Http;

namespace PetManager.Application.Pets.Commands.AddImageToPet;

internal record AddImageToPetCommand(Guid PetId, IFormFile File): IRequest<AddImageToPetResponse>;
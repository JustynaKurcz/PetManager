using PetManager.Application.Pets.Queries.GetPetDetails.DTO;

namespace PetManager.Application.Pets.Queries.GetPetDetails;

public record GetPetDetailsQuery(Guid PetId) : IRequest<PetDetailsDto>;
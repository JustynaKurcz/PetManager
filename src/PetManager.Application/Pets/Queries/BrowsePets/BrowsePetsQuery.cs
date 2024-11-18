using PetManager.Application.Pets.Queries.BrowsePets.DTO;

namespace PetManager.Application.Pets.Queries.BrowsePets;

public record BrowsePetsQuery(string Search) : IRequest<List<PetDto>>;
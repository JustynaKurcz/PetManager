using PetManager.Application.Pets.Queries.BrowsePets.DTO;
using PetManager.Application.Shared.Pagination;

namespace PetManager.Application.Pets.Queries.BrowsePets;

internal sealed class BrowsePetsQuery : PaginationRequest, IRequest<PaginationResult<PetDto>>
{
    public string Search { get; set; }
}
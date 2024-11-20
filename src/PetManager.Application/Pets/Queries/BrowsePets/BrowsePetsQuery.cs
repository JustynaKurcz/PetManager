using PetManager.Application.Pagination;
using PetManager.Application.Pets.Queries.BrowsePets.DTO;

namespace PetManager.Application.Pets.Queries.BrowsePets;

internal sealed class BrowsePetsQuery : PaginationRequest, IRequest<PaginationResult<PetDto>>
{
    public string Search { get; set; }
}
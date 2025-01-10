using PetManager.Application.Common.Pagination;
using PetManager.Application.Pets.Queries.BrowsePets;
using PetManager.Application.Pets.Queries.BrowsePets.DTO;
using PetManager.Core.Pets.Entities;
using PetManager.Core.Pets.Repositories;

namespace PetManager.Infrastructure.EF.Pets.Queries.BrowsePets;

internal sealed class BrowsePetsQueryHandler(IPetRepository petRepository)
    : IRequestHandler<BrowsePetsQuery, PaginationResult<PetDto>>
{
    public async Task<PaginationResult<PetDto>> Handle(BrowsePetsQuery query, CancellationToken cancellationToken)
    {
        var pets = await petRepository.BrowseAsync(cancellationToken);

        pets = Search(query, pets);


        return await pets.AsNoTracking()
            .Select(x => new PetDto(x.Id, x.Name))
            .PaginateAsync(query, cancellationToken);
    }

    private IQueryable<Pet> Search(BrowsePetsQuery query, IQueryable<Pet> pets)
    {
        if (string.IsNullOrWhiteSpace(query.Search)) return pets;
        var searchTxt = $"%{query.Search}%";
        return pets.Where(pet =>
            Microsoft.EntityFrameworkCore.EF.Functions.ILike(pet.Name, searchTxt));
    }
}
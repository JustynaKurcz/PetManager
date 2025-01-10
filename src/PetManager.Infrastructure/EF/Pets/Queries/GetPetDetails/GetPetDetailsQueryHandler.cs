using PetManager.Application.Pets.Queries.GetPetDetails;
using PetManager.Application.Pets.Queries.GetPetDetails.DTO;
using PetManager.Core.Pets.Exceptions;
using PetManager.Core.Pets.Repositories;

namespace PetManager.Infrastructure.EF.Pets.Queries.GetPetDetails;

internal sealed class GetPetDetailsQueryHandler(IPetRepository petRepository)
    : IRequestHandler<GetPetDetailsQuery, PetDetailsDto>
{
    public async Task<PetDetailsDto> Handle(GetPetDetailsQuery query, CancellationToken cancellationToken)
    {
        var pet = await petRepository.GetByIdAsync(x => x.Id == query.PetId, cancellationToken)
                  ?? throw new PetNotFoundException(query.PetId);

        return pet.AsDetailsDto();
    }
}
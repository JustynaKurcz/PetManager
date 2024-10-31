using PetManager.Application.EnumHelper;

namespace PetManager.Application.Pets.Queries.GetGenders.DTO;

internal record GendersDto(IEnumerable<EnumItem> Genders);
using PetManager.Core.Common.EnumHelper;

namespace PetManager.Application.Pets.Queries.GetGenders.DTO;

internal record GendersDto(IEnumerable<EnumItem> Genders);
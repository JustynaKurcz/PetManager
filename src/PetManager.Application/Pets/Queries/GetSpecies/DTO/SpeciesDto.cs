using PetManager.Core.Common.EnumHelper;

namespace PetManager.Application.Pets.Queries.GetSpecies.DTO;

internal record SpeciesDto(IEnumerable<EnumItem> Species);
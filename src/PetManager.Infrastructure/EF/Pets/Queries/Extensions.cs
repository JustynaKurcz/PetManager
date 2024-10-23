using PetManager.Application.Pets.Queries.GetPetDetails.DTO;
using PetManager.Core.Pets.Entities;

namespace PetManager.Infrastructure.EF.Pets.Queries;

internal static class Extensions
{
    public static PetDetailsDto AsDetailsDto(this Pet pet)
        => new(
            pet.PetId,
            pet.Name,
            pet.Species,
            pet.Breed,
            pet.Gender,
            pet.BirthDate,
            pet.UserId,
            pet.HealthRecordId
        );
}
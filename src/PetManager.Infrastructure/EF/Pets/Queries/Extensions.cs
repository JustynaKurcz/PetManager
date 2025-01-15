using System.ComponentModel.DataAnnotations;
using PetManager.Application.Pets.Queries.GetPetDetails.DTO;
using PetManager.Core.Pets.Entities;

namespace PetManager.Infrastructure.EF.Pets.Queries;

internal static class Extensions
{
    public static PetDetailsDto AsDetailsDto(this Pet pet)
        => new(
            pet.Id,
            pet.Name,
            pet.Species.GetDisplayName(),
            pet.Breed,
            pet.Gender.GetDisplayName(),
            pet.BirthDate,
            pet.UserId,
            pet.HealthRecordId,
            pet.Image?.BlobUrl
        );
    
    public static string GetDisplayName(this Enum enumValue)
    {
        var displayAttribute = enumValue.GetType()
            .GetField(enumValue.ToString())!
            .GetCustomAttributes(typeof(DisplayAttribute), false)
            .FirstOrDefault() as DisplayAttribute;

        return displayAttribute?.Name ?? enumValue.ToString();
    }
}
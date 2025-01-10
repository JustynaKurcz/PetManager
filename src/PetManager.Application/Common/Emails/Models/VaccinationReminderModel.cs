using PetManager.Core.Pets.Enums;

namespace PetManager.Application.Common.Emails.Models;

public record VaccinationReminderModel
{
    public string Email { get; init; }
    public string PetName { get; init; }
    public Species Species { get; init; }
    public string VaccinationName { get; init; }
    public DateTimeOffset VaccinationDate { get; init; }
}
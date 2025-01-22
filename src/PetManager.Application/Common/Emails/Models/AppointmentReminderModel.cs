namespace PetManager.Application.Common.Emails.Models;

public record AppointmentReminderModel
{
    public string Email { get; init; }
    public string PetName { get; init; }
    public string Title { get; init; }
    public string AppointmentDate { get; init; }
    public string Notes { get; init; }
}
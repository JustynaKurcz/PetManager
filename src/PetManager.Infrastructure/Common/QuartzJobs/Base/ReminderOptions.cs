namespace PetManager.Infrastructure.Common.QuartzJobs.Base;

public record ReminderOptions
{
    public int VaccinationReminderDays { get; init; }
    public int AppointmentsReminderDays { get; init; }
}
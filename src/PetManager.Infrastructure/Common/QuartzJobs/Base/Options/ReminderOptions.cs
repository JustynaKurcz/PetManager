namespace PetManager.Infrastructure.Common.QuartzJobs.Base.Options;

public record ReminderOptions
{
    public int VaccinationReminderDays { get; init; }
    public int AppointmentsReminderDays { get; init; }
}
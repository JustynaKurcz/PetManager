using PetManager.Application.Common.Emails;
using PetManager.Application.Common.Emails.Models;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Repositories;
using PetManager.Infrastructure.Common.QuartzJobs.Base;
using PetManager.Infrastructure.Common.QuartzJobs.Base.Options;

namespace PetManager.Infrastructure.Common.QuartzJobs.Jobs.AppointmentReminder;

public class AppointmentReminderJob(
    IServiceScopeFactory scopeFactory,
    IEmailService emailService,
    ReminderOptions options) : BaseReminderJob<Appointment>(scopeFactory, emailService, options)
{
    protected override async Task<IEnumerable<Appointment>> GetScheduledItems(IServiceScope scope)
    {
        var repository = scope.ServiceProvider.GetRequiredService<IAppointmentRepository>();
        return await repository.GetScheduledAppointmentsAsync(options.AppointmentsReminderDays, CancellationToken.None);
    }

    protected override async Task UpdateItem(Appointment appointment, IServiceScope scope)
    {
        var repository = scope.ServiceProvider.GetRequiredService<IAppointmentRepository>();
        appointment.MarkNotificationAsSent();
        await repository.UpdateAppointmentAsync(appointment, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);
    }

    protected override async Task SendReminderEmail(Appointment appointment, CancellationToken cancellationToken)
    {
        var emailModel = new AppointmentReminderModel
        {
            Email = appointment.HealthRecord.Pet.User.Email,
            PetName = appointment.HealthRecord.Pet.Name,
            Title = appointment.Title,
            AppointmentDate = appointment.AppointmentDate.ToString("dd-MM-yyyy HH:mm"),
            Notes = appointment.Notes
        };

        await emailService.SendEmailAsync(
            emailModel.Email,
            EmailTemplateConstants.AppointmentReminderTemplatePath,
            emailModel,
            EmailTemplateConstants.AppointmentReminderSubject,
            cancellationToken
        );
    }
}
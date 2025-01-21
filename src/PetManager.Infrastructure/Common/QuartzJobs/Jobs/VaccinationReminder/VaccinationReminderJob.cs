using PetManager.Application.Common.Emails;
using PetManager.Application.Common.Emails.Models;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Repositories;
using PetManager.Infrastructure.Common.QuartzJobs.Base;
using PetManager.Infrastructure.Common.QuartzJobs.Base.Options;

namespace PetManager.Infrastructure.Common.QuartzJobs.Jobs.VaccinationReminder;

public class VaccinationReminderJob(
    IServiceScopeFactory scopeFactory,
    IEmailService emailService,
    ReminderOptions options) : BaseReminderJob<Vaccination>(scopeFactory, emailService, options)
{
    protected override async Task<IEnumerable<Vaccination>> GetScheduledItems(IServiceScope scope)
    {
        var repository = scope.ServiceProvider.GetRequiredService<IVaccinationRepository>();
        return await repository.GetScheduledVaccinationsAsync(options.VaccinationReminderDays, CancellationToken.None);
    }

    protected override async Task UpdateItem(Vaccination vaccination, IServiceScope scope)
    {
        var repository = scope.ServiceProvider.GetRequiredService<IVaccinationRepository>();
        vaccination.MarkNotificationAsSent();
        await repository.UpdateVaccinationAsync(vaccination, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);
    }

    protected override async Task SendReminderEmail(Vaccination vaccination, CancellationToken cancellationToken)
    {
        var emailModel = new VaccinationReminderModel
        {
            Email = vaccination.HealthRecord.Pet.User.Email,
            PetName = vaccination.HealthRecord.Pet.Name,
            Species = vaccination.HealthRecord.Pet.Species,
            VaccinationName = vaccination.VaccinationName,
            VaccinationDate = vaccination.NextVaccinationDate.GetValueOrDefault()
        };

        await emailService.SendEmailAsync(
            emailModel.Email,
            EmailTemplateConstants.VaccinationReminderTemplatePath,
            emailModel,
            EmailTemplateConstants.VaccinationReminderSubject,
            cancellationToken
        );
    }
}
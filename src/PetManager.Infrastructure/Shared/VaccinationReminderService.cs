using Microsoft.Extensions.Hosting;
using PetManager.Application.Shared.Emails;
using PetManager.Application.Shared.Emails.Models;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Repositories;

namespace PetManager.Infrastructure.Shared;

public class VaccinationReminderService(IServiceScopeFactory scopeFactory, IEmailService emailService)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckAndSendReminders();
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    private async Task CheckAndSendReminders()
    {
        using var scope = scopeFactory.CreateScope();
        var vaccinationRepository = scope.ServiceProvider.GetRequiredService<IVaccinationRepository>();

        var scheduledVaccinations = await vaccinationRepository.GetScheduledVaccinationsAsync(CancellationToken.None);

        foreach (var vaccination in scheduledVaccinations)
        {
            await SendReminderEmail(vaccination, CancellationToken.None);

            vaccination.MarkNotificationAsSent();
            await vaccinationRepository.UpdateVaccinationAsync(vaccination, CancellationToken.None);
            await vaccinationRepository.SaveChangesAsync(CancellationToken.None);
        }
    }

    private async Task SendReminderEmail(Vaccination vaccination, CancellationToken cancellationToken)
    {
        var emailModel = new VaccinationReminderModel
        {
            Email = vaccination.HealthRecord.Pet.User.Email,
            PetName = vaccination.HealthRecord.Pet.Name,
            Species = vaccination.HealthRecord.Pet.Species,
            VaccinationName = vaccination.VaccinationName,
            VaccinationDate = vaccination.NextVaccinationDate
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
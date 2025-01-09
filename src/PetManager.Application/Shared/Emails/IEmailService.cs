namespace PetManager.Application.Shared.Emails;

public interface IEmailService
{
    Task SendEmailAsync<T>(string recipientEmail, string templatePath, T model, string subject,
        CancellationToken cancellationToken);
}
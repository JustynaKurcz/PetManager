using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using PetManager.Application.Common.Emails;
using PetManager.Infrastructure.Common.Emails.Configuration.Options;

namespace PetManager.Infrastructure.Common.Emails.Configuration;

internal sealed class EmailService(EmailOptions emailOptions) : IEmailService
{
    public async Task SendEmailAsync<T>(string recipientEmail, string templatePath, T model, string subject,
        CancellationToken cancellationToken)
    {
        var assembly = typeof(EmailService).Assembly;
        var template = string.Empty;

        await using (var stream = assembly.GetManifestResourceStream(templatePath))
        {
            if (stream == null)
            {
                throw new FileNotFoundException($"Template email not found: {templatePath}");
            }

            using var reader = new StreamReader(stream);
            template = await reader.ReadToEndAsync(cancellationToken);
        }

        var emailContent = ReplaceTemplateVariables(template, model);

        var message = CreateEmailMessage(recipientEmail, subject, emailContent);

        using var client = CreateSmtpClient();

        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }

    private MimeMessage CreateEmailMessage(string recipientEmail, string subject, string htmlContent)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(emailOptions.SenderName, emailOptions.SenderEmail));
        message.To.Add(new MailboxAddress("", recipientEmail));
        message.Subject = subject;

        var builder = new BodyBuilder
        {
            HtmlBody = htmlContent
        };
        message.Body = builder.ToMessageBody();

        return message;
    }

    private SmtpClient CreateSmtpClient()
    {
        var client = new SmtpClient();

        client.Connect(emailOptions.SmtpHost, emailOptions.SmtpPort,
            emailOptions.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
        client.Authenticate(emailOptions.UserName, emailOptions.Password);

        return client;
    }

    private string ReplaceTemplateVariables<T>(string template, T model)
    {
        var properties = typeof(T).GetProperties();
        var content = template;

        foreach (var prop in properties)
        {
            var placeholder = $"{{{prop.Name}}}";
            var value = prop.GetValue(model)?.ToString() ?? string.Empty;
            content = content.Replace(placeholder, value);
        }

        return content;
    }
}
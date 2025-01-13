namespace PetManager.Infrastructure.Common.Emails.Configuration.Options;

public record EmailOptions
{
    public string SmtpHost { get; init; }
    public int SmtpPort { get; init; }
    public string UserName { get; init; }
    public string Password { get; init; }
    public string SenderEmail { get; init; }
    public string SenderName { get; init; }
    public bool EnableSsl { get; init; }
}
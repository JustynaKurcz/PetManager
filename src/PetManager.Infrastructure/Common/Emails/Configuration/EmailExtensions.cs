using PetManager.Application.Common.Emails;

namespace PetManager.Infrastructure.Common.Emails.Configuration;

internal static class EmailExtensions
{
    private const string SectionName = "EmailSettings";

    public static IServiceCollection AddEmails(this IServiceCollection services, IConfiguration configuration)
    {
        var emailOptions = configuration.GetSection(SectionName).Get<EmailOptions>();
        services.AddSingleton(emailOptions);
        
        services.AddSingleton<IEmailService, EmailService>();

        return services;
    }
}
using PetManager.Application.Shared.Emails;
using PetManager.Application.Shared.Emails.Models;

namespace PetManager.Application.Users.Commands.SignUp.Events;

internal sealed class SignedUpEventHandler(IEmailService emailService) : INotificationHandler<SignedUpEvent>
{
    public async Task Handle(SignedUpEvent notification, CancellationToken cancellationToken)
    {
        var emailModel = new SignUpEmailModel { Email = notification.Email };

        await emailService.SendEmailAsync(
            notification.Email,
            EmailTemplateConstants.SignUpTemplatePath,
            emailModel,
            EmailTemplateConstants.SignUpSubject,
            cancellationToken
        );
    }
}
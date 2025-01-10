using PetManager.Application.Shared.Emails;
using PetManager.Application.Shared.Emails.Models;

namespace PetManager.Application.Users.Commands.ForgotPassword.Events;

internal sealed class ForgotPasswordRequestedEventHandler(IEmailService emailService) 
    : INotificationHandler<ForgotPasswordRequestedEvent>
{
    public async Task Handle(ForgotPasswordRequestedEvent notification, CancellationToken cancellationToken)
    {
        var emailModel = new PasswordResetEmailModel
        {
            UserName = notification.Email,
            ResetLink = notification.ResetLink
        };
        
        await emailService.SendEmailAsync(
            notification.Email,
            EmailTemplateConstants.PasswordResetTemplatePath,
            emailModel,
            EmailTemplateConstants.PasswordResetSubject,
            cancellationToken
        );
    }
}
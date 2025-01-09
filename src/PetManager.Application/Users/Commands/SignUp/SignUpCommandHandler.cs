using PetManager.Application.Shared.Emails;
using PetManager.Application.Shared.Emails.Templates;
using PetManager.Application.Shared.Emails.Templates.Models;
using PetManager.Application.Shared.Security.Passwords;
using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Enums;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;

namespace PetManager.Application.Users.Commands.SignUp;

internal sealed class SignUpCommandHandler(
    IUserRepository userRepository,
    IPasswordManager passwordManager,
    IEmailService emailService
) : IRequestHandler<SignUpCommand, SignUpResponse>
{
    public async Task<SignUpResponse> Handle(SignUpCommand command, CancellationToken cancellationToken = default)
    {
        var email = command.Email.ToLowerInvariant();

        var existingUser = await userRepository.GetByEmailAsync(x => x.Email == email, cancellationToken);
        if (existingUser is not null)
            throw new UserAlreadyExistsException(email);

        var hashPassword = passwordManager.HashPassword(command.Password);

        var user = User.Create(email, hashPassword, UserRole.User);

        await userRepository.AddAsync(user, cancellationToken);

        var emailModel = new SignUpEmailModel { Email = email };

        await emailService.SendEmailAsync(
            email,
            EmailTemplateConstants.SignUpTemplatePath,
            emailModel,
            EmailTemplateConstants.SignUpSubject,
            cancellationToken
        );

        return new SignUpResponse(user.UserId);
    }
}
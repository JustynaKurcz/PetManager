using PetManager.Application.Common.Security.Authentication;
using PetManager.Application.Users.Queries.GetResetPasswordForm;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;

namespace PetManager.Infrastructure.EF.Users.Queries.GetResetPasswordForm;

public class GetResetPasswordFormQueryHandler(
    IAuthenticationManager authenticationManager,
    IUserRepository userRepository
) : IRequestHandler<GetResetPasswordFormQuery, string>
{
    private const string ResetPasswordFormPath =
        "PetManager.Infrastructure.Common.Emails.Templates.Views.ResetPasswordForm.html";

    public async Task<string> Handle(GetResetPasswordFormQuery query, CancellationToken cancellationToken)
    {
        if (!authenticationManager.VerifyPasswordResetToken(query.Token, out var email))
        {
            throw new InvalidOperationException("Invalid password reset token.");
        }

        var user = await userRepository.GetAsync(x => x.Email == email, cancellationToken)
                   ?? throw new UserNotFoundException(email);


        var template = await LoadTemplateFromResources(cancellationToken);

        return CustomizeTemplate(template, query.Token, user.Email);
    }

    private async Task<string> LoadTemplateFromResources(CancellationToken cancellationToken)
    {
        var assembly = typeof(GetResetPasswordFormQueryHandler).Assembly;
        await using var stream = assembly.GetManifestResourceStream(ResetPasswordFormPath)
                                 ?? throw new InvalidOperationException(
                                     $"Could not find embedded resource '{ResetPasswordFormPath}'");

        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync(cancellationToken);
    }

    private static string CustomizeTemplate(string template, string token, string email)
    {
        return template
            .Replace("{token}", token)
            .Replace("{email}", email);
    }
}
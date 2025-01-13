using PetManager.Application.Common.Security.Authentication;
using PetManager.Application.Users.Commands.ForgotPassword.Events;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;

namespace PetManager.Application.Users.Commands.ForgotPassword;

internal sealed class ForgotPasswordCommandHandler(
    IUserRepository userRepository,
    IAuthenticationManager authenticationManager,
    IMediator mediator,
    IConfiguration configuration
) : IRequestHandler<ForgotPasswordCommand>
{
    private const string ResetPasswordPath = "api/v1/users/reset-password";

    public async Task Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var email = command.Email.ToLowerInvariant();
        var user = await userRepository.GetAsync(x => x.Email == email, cancellationToken)
                   ?? throw new UserNotFoundException(email);

        var resetToken = authenticationManager.GeneratePasswordResetToken(user.Email);

        var resetLink = BuildResetLink(resetToken);

        await mediator.Publish(
            new ForgotPasswordRequestedEvent(user.Email, resetLink),
            cancellationToken);
    }

    private string BuildResetLink(string token)
    {
        var apiUrl = configuration["Application:ApiBaseUrl"]
                     ?? throw new InvalidOperationException("Missing ApiBaseUrl configuration");

        return string.Join("/",
            apiUrl.TrimEnd('/'),
            ResetPasswordPath,
            Uri.EscapeDataString(token));
    }
}
namespace PetManager.Application.Users.Commands.ResetPassword;

internal sealed record ResetPasswordCommand(
    string Email,
    string NewPassword
) : IRequest
{
    internal string Token { get; init; } = default!;
}
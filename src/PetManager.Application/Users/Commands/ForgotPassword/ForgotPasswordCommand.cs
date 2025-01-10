namespace PetManager.Application.Users.Commands.ForgotPassword;

internal sealed record ForgotPasswordCommand(string Email) : IRequest;
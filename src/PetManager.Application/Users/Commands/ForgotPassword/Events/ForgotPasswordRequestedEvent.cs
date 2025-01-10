namespace PetManager.Application.Users.Commands.ForgotPassword.Events;

internal sealed record ForgotPasswordRequestedEvent(string Email, string ResetLink) : INotification;

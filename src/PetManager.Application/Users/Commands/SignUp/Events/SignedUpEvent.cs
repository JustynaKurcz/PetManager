namespace PetManager.Application.Users.Commands.SignUp.Events;

internal sealed record SignedUpEvent(string Email) : INotification;
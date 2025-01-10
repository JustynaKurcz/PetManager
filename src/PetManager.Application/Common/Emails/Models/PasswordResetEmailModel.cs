namespace PetManager.Application.Common.Emails.Models;

public record PasswordResetEmailModel
{
    public string UserName { get; init; }
    public string ResetLink { get; init; }
}
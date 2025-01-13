namespace PetManager.Application.Users.Queries.GetResetPasswordForm;

public record GetResetPasswordFormQuery(string Token) : IRequest<string>;
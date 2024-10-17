using MediatR;

namespace PetManager.Application.Users.Commands.SignIn;

public record SignInCommand(string Email, string Password) : IRequest<SignInResponse>;
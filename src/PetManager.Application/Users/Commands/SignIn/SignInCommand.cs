namespace PetManager.Application.Users.Commands.SignIn;

internal record SignInCommand(
    string Email,
    string Password
) : IRequest<SignInResponse>;
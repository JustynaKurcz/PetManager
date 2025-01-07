namespace PetManager.Application.Users.Commands.SignUp;

internal record SignUpCommand(string Email, string Password)
    : IRequest<SignUpResponse>;
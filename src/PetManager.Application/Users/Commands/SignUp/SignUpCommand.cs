namespace PetManager.Application.Users.Commands.SignUp;

public record SignUpCommand(string Password, string Email)
    : IRequest<SignUpResponse>;
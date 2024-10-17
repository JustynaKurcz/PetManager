namespace PetManager.Application.Users.Commands.SignUp;

public record SignUpCommand(string FirstName, string LastName, string Password, string Email)
    : IRequest<SignUpResponse>;
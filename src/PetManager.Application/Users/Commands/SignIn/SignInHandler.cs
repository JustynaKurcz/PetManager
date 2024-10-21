using PetManager.Application.Auth;
using PetManager.Application.Security;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;

namespace PetManager.Application.Users.Commands.SignIn;

internal sealed class SignInHandler(
    IUserRepository userRepository,
    IPasswordManager passwordManager,
    ITokenManager tokenManager)
    : IRequestHandler<SignInCommand, SignInResponse>
{
    public async Task<SignInResponse> Handle(SignInCommand command, CancellationToken cancellationToken = default)
    {
        var email = command.Email.ToLowerInvariant();

        var user = await userRepository.GetByEmailAsync(email, cancellationToken);
        if (user is null)
            throw new InvalidCredentialsException();

        var passwordIsValid = passwordManager.VerifyPassword(command.Password, user.Password);
        if (!passwordIsValid)
            throw new InvalidCredentialsException();

        var token = tokenManager.GenerateToken(user.UserId, user.Role.Name, user.Email);

        return new SignInResponse(token);
    }
}
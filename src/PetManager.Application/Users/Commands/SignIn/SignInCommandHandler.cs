using PetManager.Application.Shared.Security.Auth;
using PetManager.Application.Shared.Security.Passwords;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;

namespace PetManager.Application.Users.Commands.SignIn;

internal sealed class SignInCommandHandler(
    IUserRepository userRepository,
    IPasswordManager passwordManager,
    IAuthManager authManager
) : IRequestHandler<SignInCommand, SignInResponse>
{
    public async Task<SignInResponse> Handle(SignInCommand command, CancellationToken cancellationToken = default)
    {
        var email = command.Email.ToLowerInvariant();

        var user = await userRepository.GetByEmailAsync(x => x.Email == email, cancellationToken);
        if (user is null)
            throw new InvalidCredentialsException();

        var passwordIsValid = passwordManager.VerifyPassword(command.Password, user.Password);
        if (!passwordIsValid)
            throw new InvalidCredentialsException();

        var token = authManager.GenerateToken(user.UserId, user.Role.ToString());

        return new SignInResponse(token);
    }
}
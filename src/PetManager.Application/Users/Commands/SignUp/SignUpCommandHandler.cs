using PetManager.Application.Shared.Security.Passwords;
using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Enums;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;

namespace PetManager.Application.Users.Commands.SignUp;

internal sealed class SignUpCommandHandler(
    IUserRepository userRepository,
    IPasswordManager passwordManager
) : IRequestHandler<SignUpCommand, SignUpResponse>
{
    public async Task<SignUpResponse> Handle(SignUpCommand command, CancellationToken cancellationToken = default)
    {
        var email = command.Email.ToLowerInvariant();

        var userExists = await userRepository.ExistsByEmailAsync(email, cancellationToken);
        if (userExists)
            throw new UserAlreadyExistsException(email);

        var hashPassword = passwordManager.HashPassword(command.Password);

        var user = User.Create(email, hashPassword, UserRole.User);

        await userRepository.AddAsync(user, cancellationToken);

        return new SignUpResponse(user.UserId);
    }
}
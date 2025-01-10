using PetManager.Application.Common.Security.Passwords;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;

namespace PetManager.Application.Users.Commands.ResetPassword;

internal sealed class ResetPasswordCommandHandler(
    IUserRepository userRepository,
    IPasswordManager passwordManager) : IRequestHandler<ResetPasswordCommand>
{
    public async Task Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(x => x.Email == command.Email, cancellationToken)
            ?? throw new UserNotFoundException(command.Email);

        var hashedPassword = passwordManager.HashPassword(command.NewPassword);
        user.ChangePassword(hashedPassword);

        await userRepository.SaveChangesAsync(cancellationToken);
    }
}
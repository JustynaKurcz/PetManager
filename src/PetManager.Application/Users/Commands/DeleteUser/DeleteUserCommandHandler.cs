using PetManager.Application.Shared.Context;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;

namespace PetManager.Application.Users.Commands.DeleteUser;

internal sealed class DeleteUserCommandHandler(
    IUserRepository userRepository,
    IContext context
) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(command.UserId, cancellationToken)
                   ?? throw new UserNotFoundException(command.UserId);

        ValidateDeletePermissions(command.UserId);

        await userRepository.DeleteAsync(user, cancellationToken);
    }

    private void ValidateDeletePermissions(Guid userToDeleteId)
    {
        var isOwnAccount = context.UserId == userToDeleteId;

        if (context.IsAdmin && isOwnAccount)
            throw new AdminCannotDeleteOwnAccountException(context.UserId);

        if (!context.IsAdmin && !isOwnAccount)
            throw new UserCannotDeleteOtherUserException(context.UserId, userToDeleteId);
    }
}
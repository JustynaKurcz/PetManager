using PetManager.Application.Common.Context;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;

namespace PetManager.Application.Users.Admin.Commands.DeleteUser;

internal sealed class DeleteUserByAdminCommandHandler(
    IUserRepository userRepository,
    IContext context
) : IRequestHandler<DeleteUserByAdminCommand>
{
    public async Task Handle(DeleteUserByAdminCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(x => x.Id == command.UserId, cancellationToken)
                   ?? throw new UserNotFoundException(command.UserId);

        ThrowIfDeletingOwnAccount(command.UserId);

        await userRepository.DeleteAsync(user, cancellationToken);
    }

    private void ThrowIfDeletingOwnAccount(Guid userToDeleteId)
    {
        var isOwnAccount = context.UserId == userToDeleteId;

        if (isOwnAccount)
            throw new AdminCannotDeleteOwnAccountException(context.UserId);
    }
}
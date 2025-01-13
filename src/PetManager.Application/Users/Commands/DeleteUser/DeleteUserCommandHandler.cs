using PetManager.Application.Common.Context;
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
        var currentLoggedInUserId = context.UserId;

        var user = await userRepository.GetAsync(x => x.Id == context.UserId, cancellationToken)
                   ?? throw new UserNotFoundException(context.UserId);

        if (context.IsAdmin)
            throw new AdminCannotDeleteOwnAccountException(context.UserId);

        if (user.Id != currentLoggedInUserId)
            throw new UserCannotDeleteOtherUserException(currentLoggedInUserId, user.Id);

        await userRepository.DeleteAsync(user, cancellationToken);
    }
}
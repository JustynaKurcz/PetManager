using PetManager.Application.Context;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;

namespace PetManager.Application.Users.Commands.ChangeUserInformation;

internal sealed class ChangeUserInformationCommandHandler(IUserRepository userRepository, IContext context)
    : IRequestHandler<ChangeUserInformationCommand>
{
    public async Task Handle(ChangeUserInformationCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(context.UserId, cancellationToken)
                   ?? throw new UserNotFoundException(context.UserId);

        user.ChangeInformation(command.FirstName, command.LastName);

        await userRepository.SaveChangesAsync(cancellationToken);
    }
}
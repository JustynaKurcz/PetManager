using PetManager.Application.Common.Security.Passwords;
using PetManager.Application.Users.Commands.SignUp.Events;
using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Enums;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;

namespace PetManager.Application.Users.Commands.SignUp;

internal sealed class SignUpCommandHandler(
    IUserRepository userRepository,
    IPasswordManager passwordManager,
    IMediator mediator
) : IRequestHandler<SignUpCommand, SignUpResponse>
{
    public async Task<SignUpResponse> Handle(SignUpCommand command, CancellationToken cancellationToken = default)
    {
        var email = command.Email.ToLowerInvariant();

        var existingUser = await userRepository.GetByEmailAsync(x => x.Email == email, cancellationToken);
        if (existingUser is not null)
            throw new UserAlreadyExistsException(email);

        var hashPassword = passwordManager.HashPassword(command.Password);

        var user = User.Create(email, hashPassword, UserRole.User);

        await userRepository.AddAsync(user, cancellationToken);

        await mediator.Publish(new SignedUpEvent(email), cancellationToken);

        return new SignUpResponse(user.Id);
    }
}
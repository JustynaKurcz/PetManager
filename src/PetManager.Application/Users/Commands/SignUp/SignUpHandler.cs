using PetManager.Application.Security;
using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;

namespace PetManager.Application.Users.Commands.SignUp;

public class SignUpHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IPasswordManager passwordManager)
    : IRequestHandler<SignUpCommand, SignUpResponse>
{
    public async Task<SignUpResponse> Handle(SignUpCommand command, CancellationToken cancellationToken = default)
    {
        var email = command.Email.ToLowerInvariant();

        var userExists = await userRepository.GetByEmailAsync(email, cancellationToken);
        if (userExists is not null)
            throw new UserAlreadyExistsException(email);

        var hashPassword = passwordManager.HashPassword(command.Password);

        var roleId = await roleRepository.GetRoleIdByNameAsync("User", cancellationToken);

        var user = User.Create(email, hashPassword, roleId);
        await userRepository.AddAsync(user, cancellationToken);

        return new SignUpResponse(user.UserId);
    }
}
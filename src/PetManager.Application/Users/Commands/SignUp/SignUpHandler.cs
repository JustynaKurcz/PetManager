using MediatR;
using Microsoft.AspNetCore.Identity;
using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Repositories;

namespace PetManager.Application.Users.Commands.SignUp;

public class SignUpHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher) : IRequestHandler<SignUpCommand, SignUpResponse>
{
    public async Task<SignUpResponse> Handle(SignUpCommand command, CancellationToken cancellationToken = default)
    {
        var email = command.Email.ToLowerInvariant();
        // todo sprawdzić czy ten email jest już w użyciu
        

        var hashPassword = passwordHasher.HashPassword(default, command.Password);

        var user = User.Create(command.FirstName, command.LastName, email, hashPassword);
        await userRepository.AddAsync(user, cancellationToken);
        
        return new SignUpResponse("User created successfully");
    }
}
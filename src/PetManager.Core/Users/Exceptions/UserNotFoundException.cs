using PetManager.Core.Common.Exceptions;

namespace PetManager.Core.Users.Exceptions;

public sealed class UserNotFoundException : PetManagerException
{
    public Guid UserId { get; }
    public string Email { get; }

    public UserNotFoundException(Guid userId) : base($"User with id {userId} was not found.")
    {
        UserId = userId;
    }
    
    public UserNotFoundException(string email) : base($"User with email {email} was not found.")
    {
        Email = email;
    }
}
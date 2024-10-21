namespace PetManager.Core.Users.Exceptions;

public sealed class UserNotFoundException : PetManagerException
{
    public Guid UserId { get; }

    public UserNotFoundException(Guid userId) : base($"User with id {userId} was not found.")
    {
        UserId = userId;
    }
}
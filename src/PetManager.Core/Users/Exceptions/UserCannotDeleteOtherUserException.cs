using PetManager.Core.Common.Exceptions;

namespace PetManager.Core.Users.Exceptions;

public sealed class UserCannotDeleteOtherUserException : PetManagerException
{
    public Guid RequestingUserId { get; }
    public Guid UserToDeleteId { get; }

    public UserCannotDeleteOtherUserException(Guid requestingUserId, Guid userToDeleteId) 
        : base($"User with ID {requestingUserId} cannot delete user with ID {userToDeleteId}.")
    {
        RequestingUserId = requestingUserId;
        UserToDeleteId = userToDeleteId;
    }
}
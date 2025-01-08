using PetManager.Core.Common.Exceptions;

namespace PetManager.Core.Users.Exceptions;

public sealed class AdminCannotDeleteOwnAccountException : PetManagerException
{
    public Guid UserId { get; }

    public AdminCannotDeleteOwnAccountException(Guid userId)
        : base($"Admin with ID {userId} cannot delete their own account.")
    {
        UserId = userId;
    }
}
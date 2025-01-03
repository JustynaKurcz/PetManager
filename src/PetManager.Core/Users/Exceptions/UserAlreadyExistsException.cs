using PetManager.Core.Common.Exceptions;

namespace PetManager.Core.Users.Exceptions;

public sealed class UserAlreadyExistsException : PetManagerException
{
    public string Email { get; }

    public UserAlreadyExistsException(string email) : base($"User with email {email} already exists.")
    {
        Email = email;
    }
}
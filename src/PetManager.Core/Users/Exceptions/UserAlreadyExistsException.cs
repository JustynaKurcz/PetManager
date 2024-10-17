namespace PetManager.Core.Users.Exceptions;

public sealed class UserAlreadyExistsException : CustomException
{
    public string Email { get; }

    public UserAlreadyExistsException(string email) : base($"User with email {email} already exists.")
    {
        Email = email;
    }
}
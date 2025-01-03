using PetManager.Core.Common.Exceptions;

namespace PetManager.Core.Users.Exceptions;

public sealed class InvalidCredentialsException : PetManagerException
{
    public InvalidCredentialsException() : base("Invalid credentials")
    {
    }
}
namespace PetManager.Core.Common.Exceptions;

public abstract class PetManagerException : Exception
{
    protected PetManagerException(string message) : base(message)
    {
    }
}
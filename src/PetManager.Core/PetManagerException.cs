namespace PetManager.Core;

public abstract class PetManagerException : Exception
{
    protected PetManagerException(string message) : base(message)
    {
    }
}
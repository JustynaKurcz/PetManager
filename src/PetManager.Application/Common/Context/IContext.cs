namespace PetManager.Application.Common.Context;

public interface IContext
{
    public Guid UserId { get; }
    bool IsAdmin { get; }
}
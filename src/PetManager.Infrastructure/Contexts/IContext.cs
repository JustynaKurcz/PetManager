namespace PetManager.Infrastructure.Contexts;

public interface IContext
{
    public Guid UserId { get; }
}
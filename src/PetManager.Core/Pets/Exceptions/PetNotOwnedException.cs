using PetManager.Core.Common.Exceptions;

namespace PetManager.Core.Pets.Exceptions;

public sealed class PetNotOwnedException : PetManagerException
{
    public Guid PetId { get; }

    public PetNotOwnedException(Guid petId) : base($"Pet with id {petId} is not owned by the current user.")
    {
        PetId = petId;
    }
}
using PetManager.Core.Common.Exceptions;

namespace PetManager.Core.Pets.Exceptions;

public sealed class PetNotFoundException : PetManagerException
{
    public Guid PetId { get; }

    public PetNotFoundException(Guid petId) : base($"Pet with id {petId} was not found.")
    {
        PetId = petId;
    }
}
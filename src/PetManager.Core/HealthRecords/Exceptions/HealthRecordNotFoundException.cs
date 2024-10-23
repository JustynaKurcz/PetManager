namespace PetManager.Core.HealthRecords.Exceptions;

public sealed class HealthRecordNotFoundException : PetManagerException
{
    public Guid HealthRecordId { get; }

    public HealthRecordNotFoundException(Guid healthRecordId) : base(
        $"Health record with ID {healthRecordId} was not found.")
    {
        HealthRecordId = healthRecordId;
    }
}
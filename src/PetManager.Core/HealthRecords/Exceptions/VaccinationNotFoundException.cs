using PetManager.Core.Common.Exceptions;

namespace PetManager.Core.HealthRecords.Exceptions;

public sealed class VaccinationNotFoundException : PetManagerException
{
    public Guid VaccinationId { get; }

    public VaccinationNotFoundException(Guid vaccinationId) : base(
        $"Vaccination with ID {vaccinationId} was not found.")
    {
        VaccinationId = vaccinationId;
    }
}
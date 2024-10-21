using PetManager.Core.Pets.Entities;

namespace PetManager.Core.HealthRecords.Entitites;

public class HealthRecord
{
    public Guid HealthRecordId { get; private set; }
    private string Notes { get; set; }
    public Guid PetId { get; private set; }
    public Pet Pet { get; private set; }
    private HashSet<Appointment> _appointments = [];
    private HashSet<Vaccination> _vaccinations = [];

    public IEnumerable<Appointment> Appointments
    {
        get => _appointments;
        set => _appointments = new HashSet<Appointment>(value);
    }

    public IEnumerable<Vaccination> Vaccinations
    {
        get => _vaccinations;
        set => _vaccinations = new HashSet<Vaccination>(value);
    }

    private HealthRecord()
    {
    }

    private HealthRecord(Guid petId)
    {
        HealthRecordId = Guid.NewGuid();
        PetId = petId;
    }

    public static HealthRecord Create(Guid petId)
        => new(petId);
}
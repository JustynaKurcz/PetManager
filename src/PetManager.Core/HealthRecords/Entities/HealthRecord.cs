using PetManager.Core.Pets.Entities;

namespace PetManager.Core.HealthRecords.Entities;

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

    public void AddVaccination(Vaccination vaccination)
        => _vaccinations.Add(vaccination);

    public void DeleteVaccination(Vaccination vaccination)
        => _vaccinations.Remove(vaccination);

    public void AddAppointment(Appointment appointment)
        => _appointments.Add(appointment);

    public void DeleteAppointment(Appointment appointment)
        => _appointments.Remove(appointment);
}
namespace PetManager.Core.HealthRecords.Entities;

public class Appointment
{
    public Guid AppointmentId { get; private set; }
    private string Title { get; set; }
    private string Diagnosis { get; set; }
    private DateTimeOffset AppointmentDate { get; set; }
    public string Notes { get; private set; }

    public Guid HealthRecordId { get; private set; }
    public virtual HealthRecord HealthRecord { get; private set; }

    private Appointment()
    {
    }

    public Appointment(string title, string diagnosis, DateTimeOffset appointmentDate, string notes,
        Guid healthRecordId)
    {
        AppointmentId = Guid.NewGuid();
        Title = title;
        Diagnosis = diagnosis;
        AppointmentDate = appointmentDate;
        Notes = notes;
        HealthRecordId = healthRecordId;
    }

    public static Appointment Create(string title, string diagnosis, DateTimeOffset appointmentDate, string notes,
        Guid healthRecordId)
        => new(title, diagnosis, appointmentDate, notes, healthRecordId);
}
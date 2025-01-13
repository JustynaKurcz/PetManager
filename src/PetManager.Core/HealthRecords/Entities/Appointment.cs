namespace PetManager.Core.HealthRecords.Entities;

public class Appointment
{
    public Guid Id { get; private set; }
    public string Title { get; set; }
    public string Diagnosis { get; set; }
    public DateTimeOffset AppointmentDate { get; set; }
    public string Notes { get; private set; }
    public bool IsNotificationSent { get; set; } = false;
    public Guid HealthRecordId { get; private set; }
    public virtual HealthRecord HealthRecord { get; private set; }

    private Appointment()
    {
    }

    public Appointment(string title, string diagnosis, DateTimeOffset appointmentDate, string notes,
        Guid healthRecordId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Diagnosis = diagnosis;
        AppointmentDate = appointmentDate;
        Notes = notes;
        HealthRecordId = healthRecordId;
    }

    public static Appointment Create(string title, string diagnosis, DateTimeOffset appointmentDate, string notes,
        Guid healthRecordId)
        => new(title, diagnosis, appointmentDate, notes, healthRecordId);

    public void MarkNotificationAsSent()
        => IsNotificationSent = true;
}
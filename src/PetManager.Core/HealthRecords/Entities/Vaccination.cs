namespace PetManager.Core.HealthRecords.Entities;

public class Vaccination
{
    public Guid VaccinationId { get; private set; }
    public string VaccinationName { get; set; }
    public DateTimeOffset VaccinationDate { get; set; }
    public DateTimeOffset NextVaccinationDate { get; set; }
    public Guid HealthRecordId { get; private set; }
    public HealthRecord HealthRecord { get; set; }

    private Vaccination()
    {
    }

    private Vaccination(string vaccinationName, DateTimeOffset vaccinationDate, DateTimeOffset nextVaccinationDate,
        Guid healthRecordId)
    {
        VaccinationId = Guid.NewGuid();
        VaccinationName = vaccinationName;
        VaccinationDate = vaccinationDate;
        NextVaccinationDate = nextVaccinationDate;
        HealthRecordId = healthRecordId;
    }

    public static Vaccination Create(string vaccinationName, DateTimeOffset vaccinationDate,
        DateTimeOffset nextVaccinationDate, Guid healthRecordId)
        => new(vaccinationName, vaccinationDate, nextVaccinationDate, healthRecordId);
}
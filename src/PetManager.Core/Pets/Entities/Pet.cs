using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.Pets.Enums;
using PetManager.Core.Users.Entities;

namespace PetManager.Core.Pets.Entities;

public class Pet
{
    public Guid PetId { get; private set; }
    public string Name { get; set; }
    public Species Species { get; set; }
    public string Breed { get; set; }
    public Gender Gender { get; set; }
    public DateTimeOffset BirthDate { get; set; }
    public Guid UserId { get; private set; }
    public virtual User User { get; private set; }
    public Guid HealthRecordId { get; set; }
    public HealthRecord HealthRecord { get; set; }

    private Pet()
    {
    }

    private Pet(string name, Species species, string breed, Gender gender, DateTimeOffset birthDate, Guid userId,
        HealthRecord healthRecord)
    {
        PetId = Guid.NewGuid();
        Name = name;
        Species = species;
        Breed = breed;
        Gender = gender;
        BirthDate = birthDate;
        UserId = userId;
        HealthRecordId = healthRecord.HealthRecordId;
        HealthRecord = healthRecord;
    }

    public static Pet Create(string name, Species species, string breed, Gender gender, DateTimeOffset birthDate,
        Guid userId, HealthRecord healthRecord)
        => new(name, species, breed, gender, birthDate, userId, healthRecord);

    public void AddHealthRecord(Guid healthRecordId)
        => HealthRecordId = healthRecordId;

    public void ChangeInformation(Species species, string breed, Gender gender)
    {
        Species = species;
        Breed = breed;
        Gender = gender;
    }
}
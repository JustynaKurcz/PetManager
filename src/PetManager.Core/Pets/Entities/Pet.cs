using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.Pets.Enums;
using PetManager.Core.Users.Entities;

namespace PetManager.Core.Pets.Entities;

public class Pet
{
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public Species Species { get; set; }
    public string Breed { get; set; }
    public Gender Gender { get; set; }
    public DateTimeOffset BirthDate { get; set; }
    public Guid UserId { get; private set; }
    public virtual User User { get; private set; }
    public Guid HealthRecordId { get; set; }
    public virtual HealthRecord HealthRecord { get; set; }
    public Guid? ImageId { get; set; }
    public virtual Image? Image { get; set; }

    private Pet()
    {
    }

    private Pet(string name, Species species, string breed, Gender gender, DateTimeOffset birthDate, Guid userId,
        HealthRecord healthRecord)
    {
        Id = Guid.NewGuid();
        Name = name;
        Species = species;
        Breed = breed;
        Gender = gender;
        BirthDate = birthDate;
        UserId = userId;
        HealthRecordId = healthRecord.Id;
        HealthRecord = healthRecord;
    }

    public static Pet Create(string name, Species species, string breed, Gender gender, DateTimeOffset birthDate,
        Guid userId, HealthRecord healthRecord)
        => new(name, species, breed, gender, birthDate, userId, healthRecord);

    public void ChangeInformation(Species species, string breed, Gender gender)
    {
        Species = species;
        Breed = breed;
        Gender = gender;
    }

    public void SetImage(Image image)
    {
        Image = image;
        ImageId = image.Id;
    }
}
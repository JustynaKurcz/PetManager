using PetManager.Core.HealthRecords.Entitites;
using PetManager.Core.Pets.Enums;
using PetManager.Core.Users.Entities;

namespace PetManager.Core.Pets.Entities;

public class Pet
{
    public Guid PetId { get; private set; }
    private string Name { get; set; }
    private Species Species { get; set; }
    private string Breed { get; set; }
    private Gender Gender { get; set; }
    private DateTimeOffset BirthDate { get; set; }

    public Guid UserId { get; private set; }
    public User User { get; private set; }

    public Guid HealthRecordId { get; private set; }
    public virtual HealthRecord HealthRecord { get; private set; }

    private Pet()
    {
    }

    private Pet(string name, Species species, string breed, Gender gender, DateTimeOffset birthDate, Guid userId,
        Guid healthRecordId)
    {
        PetId = Guid.NewGuid();
        Name = name;
        Species = species;
        Breed = breed;
        Gender = gender;
        BirthDate = birthDate;
        UserId = userId;
        HealthRecordId = healthRecordId;
    }

    public static Pet Create(string name, Species species, string breed, Gender gender, DateTimeOffset birthDate,
        Guid userId, Guid healthRecordId)
        => new(name, species, breed, gender, birthDate, userId, healthRecordId);
}
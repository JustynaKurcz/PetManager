using System.ComponentModel.DataAnnotations;

namespace PetManager.Core.Pets.Enums;

public enum Species : short
{
    [Display(Name = "Pies")] Dog = 1,

    [Display(Name = "Kot")] Cat = 2,

    [Display(Name = "Królik")] Rabbit = 3,

    [Display(Name = "Chomik")] Hamster = 4,

    [Display(Name = "Ptak")] Bird = 5
}
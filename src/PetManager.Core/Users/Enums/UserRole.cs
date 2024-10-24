namespace PetManager.Core.Users.Enums;

public enum UserRole : short
{
    [Display(Name = "Brak")] None = 1,
    [Display(Name = "Klient")] Client = 2,
    [Display(Name = "Admin")] Admin = 3,
}
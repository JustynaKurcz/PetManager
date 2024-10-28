namespace PetManager.Core.Users.Enums;

public enum UserRole : short
{
    [Display(Name = "Użytkownik")] User = 1,
    [Display(Name = "Administrator")] Admin = 2,
}
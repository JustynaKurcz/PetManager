namespace PetManager.Application.Common.Emails;

public sealed class EmailTemplateConstants
{
    public const string SignUpSubject = "Witamy w Pet Manager";
    public const string SignUpTemplatePath = "PetManager.Infrastructure.Common.Emails.Templates.Views.SignUpEmail.html";
    
    public const string VaccinationReminderSubject = "Przypomnienie o szczepieniu";
    public const string VaccinationReminderTemplatePath = "PetManager.Infrastructure.Common.Emails.Templates.Views.VaccinationReminder.html";
    
    public const string AppointmentReminderSubject = "Przypomnienie o wizycie";
    public const string AppointmentReminderTemplatePath = "PetManager.Infrastructure.Common.Emails.Templates.Views.AppointmentReminder.html";
    
    public const string PasswordResetSubject = "Resetowanie hasła";
    public const string PasswordResetTemplatePath = "PetManager.Infrastructure.Common.Emails.Templates.Views.PasswordReset.html";
}
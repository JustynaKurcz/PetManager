namespace PetManager.Application.Shared.Emails;

public sealed class EmailTemplateConstants
{
    public const string SignUpSubject = "Witamy w Pet Manager";
    public const string SignUpTemplatePath = "PetManager.Infrastructure.Shared.Emails.Templates.Views.SignUpEmail.html";
    
    public const string VaccinationReminderSubject = "Przypomnienie o szczepieniu";
    public const string VaccinationReminderTemplatePath = "PetManager.Infrastructure.Shared.Emails.Templates.Views.VaccinationReminder.html";
}
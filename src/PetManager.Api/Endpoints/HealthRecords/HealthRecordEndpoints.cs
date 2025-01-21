namespace PetManager.Api.Endpoints.HealthRecords;

internal static class HealthRecordEndpoints
{
    internal const string Base = $"{Routing.BaseUrl}/health-records";
    internal const string Tag = "HealthRecords";

    internal static string AddAppointment => $"{Base}/{{healthRecordId:guid}}/appointments";
    internal static string DeleteAppointment => $"{Base}/{{healthRecordId:guid}}/appointments/{{appointmentId:guid}}";
    internal static string GetAppointmentDetails =>
        $"{Base}/{{healthRecordId:guid}}/appointments/{{appointmentId:guid}}";
    internal static string AddVaccination => $"{Base}/{{healthRecordId:guid}}/vaccinations";
    internal static string DeleteVaccination => $"{Base}/{{healthRecordId:guid}}/vaccinations/{{vaccinationId:guid}}";
    internal static string GetVaccinationDetails =>
        $"{Base}/{{healthRecordId:guid}}/vaccinations/{{vaccinationId:guid}}";
    internal static string BrowseAppointments => $"{Base}/{{healthRecordId:guid}}/appointments";
    internal static string BrowseVaccinations => $"{Base}/{{healthRecordId:guid}}/vaccinations";
    internal static string ChangeAppointmentInformation =>
        $"{Base}/{{healthRecordId:guid}}/appointments/{{appointmentId:guid}}"; 
    internal static string ChangeVaccinationInformation =>
        $"{Base}/{{healthRecordId:guid}}/vaccinations/{{vaccinationId:guid}}";
}
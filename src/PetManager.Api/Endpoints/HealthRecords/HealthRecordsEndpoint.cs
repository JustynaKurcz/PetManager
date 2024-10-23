using PetManager.Api.Common.Endpoints;

namespace PetManager.Api.Endpoints.HealthRecords;

internal static class HealthRecordsEndpoint
{
    internal const string Url = $"{Routing.BaseUrl}/health-records";
    internal const string Tag = "Health Records";
}
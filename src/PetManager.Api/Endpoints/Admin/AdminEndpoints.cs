namespace PetManager.Api.Endpoints.Admin;

internal static class AdminEndpoints
{
    private const string Base = $"{Routing.BaseUrl}/admin";
    internal const string Tag = "Admin";

    internal static string BrowseUsers => $"{Base}/users";
    internal static string DeleteUser => $"{Base}/users/{{userId:guid}}";
}
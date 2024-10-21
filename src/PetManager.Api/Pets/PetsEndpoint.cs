using PetManager.Common.Endpoints;

namespace PetManager.Pets;

internal static class PetsEndpoint
{
    internal const string Url = $"{Routing.BaseUrl}/pets";
    internal const string Tag = "Pets";
}
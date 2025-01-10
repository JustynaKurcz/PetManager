namespace PetManager.Api.Endpoints.Pets;

internal static class PetEndpoints
{
    internal const string Base = $"{Routing.BaseUrl}/pets";
    internal const string Tag = "Pets";

    internal static string CreatePet => Base;
    internal static string DeletePet => $"{Base}/{{petId:guid}}";
    internal static string ChangePetInformation => $"{Base}/{{petId:guid}}";
    internal static string BrowsePets => Base;
    internal static string GetPetDetails => $"{Base}/{{petId:guid}}";
    internal static string GetGenders => $"{Base}/gender-types";
    internal static string GetSpecies => $"{Base}/species-types";
    internal static string AddImageToPet => $"{Base}/{{petId:guid}}/images";
}
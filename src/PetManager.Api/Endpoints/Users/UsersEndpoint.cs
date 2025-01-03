using PetManager.Api.Common.Endpoints;

namespace PetManager.Api.Endpoints.Users;

internal static class UsersEndpoint
{
    internal static string Base => $"{Routing.BaseUrl}/users";
    internal const string Tag = "Users";

    internal static string SignIn => $"{Base}/sign-in";
    internal static string SignUp => $"{Base}/sign-up";
    internal static string DeleteUser => $"{Base}/{{userId:guid}}";
    internal static string ChangeUserInformation => Base;
    internal static string GetCurrentUser => $"{Base}/user";
}
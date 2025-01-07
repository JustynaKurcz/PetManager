using Microsoft.AspNetCore.Http;
using PetManager.Application.Shared.Context;

namespace PetManager.Infrastructure.Shared.Context;

internal class Context : IContext
{
    public Guid UserId { get; }
    public bool IsAdmin { get; }


    public Context(IHttpContextAccessor httpContextAccessor)
    {
        var identity = httpContextAccessor.HttpContext?.User.Identity;
        var claims = httpContextAccessor.HttpContext?.User.Claims;

        UserId = Guid.Parse(identity!.Name!);
        IsAdmin = claims?.Any(c => c is { Type: "role", Value: "Admin" }) ?? false;
        
    }
}
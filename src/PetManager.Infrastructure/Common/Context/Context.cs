using Microsoft.AspNetCore.Http;
using PetManager.Application.Common.Context;

namespace PetManager.Infrastructure.Common.Context;

internal class Context : IContext
{
    public Guid UserId { get; }
    public bool IsAdmin { get; }


    public Context(IHttpContextAccessor httpContextAccessor)
    {
        var identity = httpContextAccessor.HttpContext?.User.Identity;
        var claims = httpContextAccessor.HttpContext?.User.Claims;
        
        UserId = Guid.Parse(identity!.Name!);
        IsAdmin = claims?.Any(c => c is { Type: ClaimTypes.Role, Value: "Admin" }) ?? false;
    }
}
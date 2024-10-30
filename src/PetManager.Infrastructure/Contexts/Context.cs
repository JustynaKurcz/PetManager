using Microsoft.AspNetCore.Http;

namespace PetManager.Infrastructure.Contexts;

internal class Context : IContext
{
    public Guid UserId { get; }


    public Context(IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext;

        UserId = httpContext!.User.Identity!.IsAuthenticated ? Guid.Parse(httpContext.User.Identity.Name!) : Guid.Empty;
    }
}
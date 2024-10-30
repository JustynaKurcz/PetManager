using Microsoft.AspNetCore.Http;
using PetManager.Application.Context;

namespace PetManager.Infrastructure.Context;

internal class Context : IContext
{
    public Guid UserId { get; }


    public Context(IHttpContextAccessor httpContextAccessor)
    {
        var identity = httpContextAccessor.HttpContext?.User.Identity;

        UserId = Guid.Parse(identity.Name);
    }
}
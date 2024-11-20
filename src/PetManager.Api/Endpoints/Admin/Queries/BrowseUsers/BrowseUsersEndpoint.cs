using PetManager.Api.Abstractions;
using PetManager.Application.Admin.Queries.BrowseUsers;
using PetManager.Application.Pagination;
using PetManager.Application.Users.Commands.SignIn;
using PetManager.Application.Users.Queries.GetCurrentUserDetails.DTO;

namespace PetManager.Api.Endpoints.Admin.Queries.BrowseUsers;

internal sealed  class BrowseUsersEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(AdminEndpoint.Url, async (
                [FromServices] IMediator mediator,
                [AsParameters] BrowseUsersQuery query,
                CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(query, cancellationToken);

                return Results.Ok(response);
            })
            .Produces<PaginationResult<CurrentUserDetailsDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(AdminEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Browse users",
                Description = "This endpoint allows admin to browse users.",
            });
    }
}
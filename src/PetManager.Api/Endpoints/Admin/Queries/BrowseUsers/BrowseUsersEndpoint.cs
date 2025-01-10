using PetManager.Api.Common.Endpoints;
using PetManager.Application.Admin.Queries.BrowseUsers;
using PetManager.Application.Common.Pagination;
using PetManager.Application.Users.Queries.GetCurrentUserDetails.DTO;

namespace PetManager.Api.Endpoints.Admin.Queries.BrowseUsers;

internal sealed class BrowseUsersEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(AdminEndpoints.BrowseUsers, async (
                [FromServices] IMediator mediator,
                [AsParameters] BrowseUsersQuery query,
                CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(query, cancellationToken);

                return Results.Ok(response);
            })
            .Produces<PaginationResult<CurrentUserDetailsDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(AdminEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Browse users",
                Description = "This endpoint allows admin to browse users.",
            });
    }
}
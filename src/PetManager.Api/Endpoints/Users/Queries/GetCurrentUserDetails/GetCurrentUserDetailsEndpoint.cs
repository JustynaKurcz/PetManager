using PetManager.Api.Abstractions;
using PetManager.Application.Users.Queries.GetCurrentUserDetails;
using PetManager.Application.Users.Queries.GetCurrentUserDetails.DTO;

namespace PetManager.Api.Endpoints.Users.Queries.GetCurrentUserDetails;

internal sealed class GetCurrentUserDetailsEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/user", async (
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var query = new GetCurrentUserDetailsQuery();
                var response = await mediator.Send(query, cancellationToken);

                return Results.Ok(response);
            })
            .Produces<CurrentUserDetailsDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(UsersEndpoint.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Get Details Of The Logged In User",
                Description = "Retrieves the details of the user who is currently logged in."
            })
            .RequireAuthorization();
    }
}
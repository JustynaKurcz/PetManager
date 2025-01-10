
using PetManager.Api.Common.Endpoints;
using PetManager.Application.Users.Queries.GetResetPasswordForm;

namespace PetManager.Api.Endpoints.Users.Queries.GetResetPasswordForm;

internal sealed class GetResetPasswordFormEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(UserEndpoints.ResetPassword, async (
                [FromRoute] string token,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var formHtml = await mediator.Send(new GetResetPasswordFormQuery(token), cancellationToken);
                return Results.Content(formHtml, "text/html");
            })
            .WithTags(UserEndpoints.Tag);
    }
}
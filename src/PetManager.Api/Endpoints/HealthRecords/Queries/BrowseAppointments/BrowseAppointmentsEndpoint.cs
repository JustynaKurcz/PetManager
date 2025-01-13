using PetManager.Api.Common.Endpoints;
using PetManager.Application.Common.Pagination;
using PetManager.Application.HealthRecords.Queries.BrowseAppointments;
using PetManager.Application.HealthRecords.Queries.BrowseAppointments.DTO;
using PetManager.Infrastructure.Common.Security.Authorization.Policies;

namespace PetManager.Api.Endpoints.HealthRecords.Queries.BrowseAppointments;

internal sealed class BrowseAppointmentsEndpoint : IEndpointDefinition
{
    public void DefineEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(HealthRecordEndpoints.BrowseAppointments, async (
                [FromServices] IMediator mediator,
                [AsParameters] BrowseAppointmentsEndpointRequest request,
                CancellationToken cancellationToken) =>
            {
                var query = new BrowseAppointmentsQuery 
                {
                    Search = request.Search,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    HealthRecordId = request.HealthRecordId
                };
                
                var response = await mediator.Send(query, cancellationToken);
                return Results.Ok(response);
            })
            .Produces<PaginationResult<AppointmentDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithTags(HealthRecordEndpoints.Tag)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Browse appointments",
                Description = "This endpoint allows users to browse appointments.",
            })
            .RequireAuthorization(AuthorizationPolicies.RequireUserRole);
    }
}
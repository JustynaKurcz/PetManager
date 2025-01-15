// using PetManager.Api.Common.Endpoints;
// using PetManager.Application.HealthRecords.Queries.GetHealthRecordDetails;
// using PetManager.Application.HealthRecords.Queries.GetHealthRecordDetails.DTO;
// using PetManager.Infrastructure.Common.Security.Authorization.Policies;
//
// namespace PetManager.Api.Endpoints.HealthRecords.Queries.GetHealthRecordDetails;
//
// internal sealed class GetHealthRecordDetailsEndpoint : IEndpointDefinition
// {
//     public void DefineEndpoint(IEndpointRouteBuilder app)
//     {
//         app.MapGet(HealthRecordEndpoints.GetHealthRecordDetails, async (
//                 [FromRoute] Guid healthRecordId,
//                 [FromServices] IMediator mediator,
//                 CancellationToken cancellationToken) =>
//             {
//                 var query = new GetHealthRecordDetailsQuery(healthRecordId);
//                 var response = await mediator.Send(query, cancellationToken);
//
//                 return Results.Ok(response);
//             })
//             .Produces<HealthRecordDetailsDto>(StatusCodes.Status200OK)
//             .Produces(StatusCodes.Status400BadRequest)
//             .WithTags(HealthRecordEndpoints.Tag)
//             .WithOpenApi(o => new OpenApiOperation(o)
//             {
//                 Summary = "Get health record details",
//                 Description = "This endpoint allows users to get details of a health record.",
//             })
//             .RequireAuthorization(AuthorizationPolicies.RequireUserRole);
//     }
// }
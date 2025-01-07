using PetManager.Application.Users.Queries.GetCurrentUserDetails.DTO;

namespace PetManager.Application.Users.Queries.GetCurrentUserDetails;

internal record GetCurrentUserDetailsQuery : IRequest<CurrentUserDetailsDto>;
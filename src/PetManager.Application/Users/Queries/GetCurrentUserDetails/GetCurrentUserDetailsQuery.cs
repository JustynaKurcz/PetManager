using PetManager.Application.Users.Queries.GetCurrentUserDetails.DTO;

namespace PetManager.Application.Users.Queries.GetCurrentUserDetails;

public record GetCurrentUserDetailsQuery : IRequest<CurrentUserDetailsDto>;
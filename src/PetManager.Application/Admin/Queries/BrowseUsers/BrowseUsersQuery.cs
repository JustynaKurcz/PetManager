using PetManager.Application.Shared.Pagination;
using PetManager.Application.Users.Queries.GetCurrentUserDetails.DTO;

namespace PetManager.Application.Admin.Queries.BrowseUsers;

internal sealed class BrowseUsersQuery : PaginationRequest, IRequest<PaginationResult<CurrentUserDetailsDto>>
{
    public string Search { get; set; }
}
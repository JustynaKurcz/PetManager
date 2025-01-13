using PetManager.Application.Common.Pagination;
using PetManager.Application.Users.Admin.Queries.BrowseUsers.DTO;

namespace PetManager.Application.Users.Admin.Queries.BrowseUsers;

internal sealed class BrowseUsersQuery : PaginationRequest, IRequest<PaginationResult<UserDto>>
{
    public string Search { get; set; }
}
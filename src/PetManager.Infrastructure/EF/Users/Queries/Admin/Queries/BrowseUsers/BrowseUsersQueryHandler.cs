using PetManager.Application.Common.Pagination;
using PetManager.Application.Users.Admin.Queries.BrowseUsers;
using PetManager.Application.Users.Admin.Queries.BrowseUsers.DTO;
using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Repositories;

namespace PetManager.Infrastructure.EF.Users.Queries.Admin.Queries.BrowseUsers;

internal sealed class BrowseUsersQueryHandler(
    IUserRepository userRepository
) : IRequestHandler<BrowseUsersQuery, PaginationResult<UserDto>>
{
    public async Task<PaginationResult<UserDto>> Handle(BrowseUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await userRepository.BrowseAsync(cancellationToken);

        users = Search(query, users);

        return await users
            .AsNoTracking()
            .Select(x => x.AsUserDto())
            .PaginateAsync(query, cancellationToken);
    }

    private IQueryable<User> Search(BrowseUsersQuery query, IQueryable<User> users)
    {
        if (string.IsNullOrWhiteSpace(query.Search)) return users;
        var searchTxt = $"%{query.Search}%";
        return users.Where(user =>
            Microsoft.EntityFrameworkCore.EF.Functions.ILike(user.Email, searchTxt) ||
            Microsoft.EntityFrameworkCore.EF.Functions.ILike(user.FirstName, searchTxt) ||
            Microsoft.EntityFrameworkCore.EF.Functions.ILike(user.LastName, searchTxt));
    }
}
using PetManager.Application.Common.Pagination;
using PetManager.Application.Users.Admin.Queries.BrowseUsers;
using PetManager.Application.Users.Admin.Queries.BrowseUsers.DTO;
using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Repositories;
using PetManager.Infrastructure.EF.Users.Queries.Admin.Queries.BrowseUsers;
using PetManager.Tests.Unit.Users.Factories;

namespace PetManager.Tests.Unit.Users.Handlers.Admin.BrowseUsers;

public sealed class BrowseUsersQueryHandlerTests
{
    private async Task<PaginationResult<UserDto>> Act(BrowseUsersQuery query)
        => await _handler.Handle(query, CancellationToken.None);

    [Fact]
    public async Task given_valid_query_when_browse_users_then_should_return_users()
    {
        // Arrange
        var query = new BrowseUsersQuery();
        var users = await _userFactory.CreateUsers();

        _userRepository.BrowseAsync(Arg.Any<CancellationToken>())
            .Returns(users);

        // Act
        var result = await Act(query);

        // Assert
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(users.Count());
    }

    [Fact]
    public async Task given_valid_query_when_empty_users_then_should_return_empty_list()
    {
        // Arrange
        var query = new BrowseUsersQuery();

        _userRepository.BrowseAsync(Arg.Any<CancellationToken>())
            .Returns(Enumerable.Empty<User>().AsQueryable());

        // Act
        var result = await Act(query);

        // Assert
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(0);
    }

    private readonly IUserRepository _userRepository;
    private readonly UserTestFactory _userFactory = new();
    
    private readonly IRequestHandler<BrowseUsersQuery, PaginationResult<UserDto>> _handler;

    public BrowseUsersQueryHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        
        _handler = new BrowseUsersQueryHandler(_userRepository);
    }
}
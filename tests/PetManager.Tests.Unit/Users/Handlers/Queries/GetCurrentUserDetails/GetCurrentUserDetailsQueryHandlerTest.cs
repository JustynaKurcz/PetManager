using PetManager.Application.Context;
using PetManager.Application.Users.Queries.GetCurrentUserDetails;
using PetManager.Application.Users.Queries.GetCurrentUserDetails.DTO;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;
using PetManager.Infrastructure.EF.Users.Queries.GetCurrentUserDetails;
using PetManager.Tests.Unit.Users.Factories;

namespace PetManager.Tests.Unit.Users.Handlers.Queries.GetCurrentUserDetails;

public sealed class GetCurrentUserDetailsQueryHandlerTest
{
    private async Task<CurrentUserDetailsDto> Act(GetCurrentUserDetailsQuery query)
        => await _handler.Handle(query, CancellationToken.None);

    [Fact]
    public async Task given_invalid_user_id_when_get_current_user_details_then_should_throw_user_not_found_exception()
    {
        // Arrange
        var query = new GetCurrentUserDetailsQuery();
        _userRepository
            .GetByIdAsync(_context.UserId, Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var exception = await Record.ExceptionAsync(() => Act(query));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UserNotFoundException>();
        exception.Message.ShouldBe($"User with id {_context.UserId} was not found.");

        await _userRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task given_valid_user_id_when_get_current_user_details_then_should_return_current_user_details()
    {
        // Arrange
        var query = new GetCurrentUserDetailsQuery();
        var user = _userFactory.CreateUser();

        _userRepository
            .GetByIdAsync(_context.UserId, Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        var response = await Act(query);

        // Assert
        response.ShouldNotBeNull();
        response.ShouldBeOfType<CurrentUserDetailsDto>();
    }

    private readonly IUserRepository _userRepository;
    private readonly IContext _context;

    private readonly IRequestHandler<GetCurrentUserDetailsQuery, CurrentUserDetailsDto> _handler;

    private readonly UserTestFactory _userFactory = new();

    public GetCurrentUserDetailsQueryHandlerTest()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _context = Substitute.For<IContext>();

        _handler = new GetCurrentUserDetailsQueryHandler(_userRepository, _context);
    }
}
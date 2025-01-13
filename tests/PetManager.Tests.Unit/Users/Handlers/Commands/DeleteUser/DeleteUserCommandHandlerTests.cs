using PetManager.Application.Common.Context;
using PetManager.Application.Users.Commands.DeleteUser;
using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;
using PetManager.Tests.Unit.Users.Factories;

namespace PetManager.Tests.Unit.Users.Handlers.Commands.DeleteUser;

public sealed class DeleteUserCommandHandlerTests
{
    private async Task Act(DeleteUserCommand command)
        => await _handler.Handle(command, CancellationToken.None);

    [Fact]
    public async Task given_user_not_found_when_delete_user_then_should_throw_user_not_found_exception()
    {
        var currentLoggedInUserId = Guid.NewGuid();
        _context.UserId.Returns(currentLoggedInUserId);
        // Arrange
        var command = _userFactory.CreateDeleteUserCommand();
        _userRepository
            .GetAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UserNotFoundException>();
        exception.Message.ShouldBe($"User with id {currentLoggedInUserId} was not found.");

        await _userRepository
            .Received(1)
            .GetAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>());

        await _userRepository
            .DidNotReceive()
            .DeleteAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task given_user_deleting_own_account_then_should_delete_user()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        _context.IsAdmin.Returns(false);
        _context.UserId.Returns(user.Id);
        var command = _userFactory.CreateDeleteUserCommand();

        _userRepository
            .GetAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        await Act(command);

        // Assert
        await _userRepository
            .Received(1)
            .DeleteAsync(user, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task given_admin_deleting_own_account_then_should_throw_admin_cannot_delete_own_account_exception()
    {
        // Arrange
        _context.IsAdmin.Returns(true);
        _context.UserId.Returns(Guid.NewGuid());
        var command = new DeleteUserCommand();
        var user = _userFactory.CreateUser();

        _userRepository
            .GetAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<AdminCannotDeleteOwnAccountException>();
        exception.Message.ShouldBe($"Admin with ID {_context.UserId} cannot delete their own account.");

        await _userRepository
            .DidNotReceive()
            .DeleteAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task given_user_deleting_other_user_account_then_should_throw_user_cannot_delete_other_user_exception()
    {
        // Arrange
        var currentUserId = Guid.NewGuid();
        var user = _userFactory.CreateUser();
        // var otherUserId = Guid.NewGuid();
        _context.IsAdmin.Returns(false);
        _context.UserId.Returns(currentUserId);
        var command = new DeleteUserCommand();
        // var user = _userFactory.CreateUser(id: otherUserId);

        _userRepository
            .GetAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UserCannotDeleteOtherUserException>();
        exception.Message.ShouldBe($"User with ID {currentUserId} cannot delete user with ID {user.Id}.");

        await _userRepository
            .DidNotReceive()
            .DeleteAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    private readonly IUserRepository _userRepository;
    private readonly IContext _context;
    private readonly IRequestHandler<DeleteUserCommand> _handler;
    private readonly UserTestFactory _userFactory = new();

    public DeleteUserCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _context = Substitute.For<IContext>();

        _handler = new DeleteUserCommandHandler(_userRepository, _context);
    }
}
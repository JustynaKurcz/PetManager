using PetManager.Application.Shared.Context;
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
        // Arrange
        var command = _userFactory.DeleteUserCommand();
        _userRepository
            .GetByIdAsync(command.UserId, Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UserNotFoundException>();
        exception.Message.ShouldBe($"User with id {command.UserId} was not found.");

        await _userRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());

        await _userRepository
            .DidNotReceive()
            .DeleteAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task given_admin_trying_to_delete_own_account_then_should_throw_admin_cannot_delete_own_account_exception()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _context.UserId.Returns(userId);
        _context.IsAdmin.Returns(true);
        var command = new DeleteUserCommand(userId);
        var user = _userFactory.CreateUser();
        
        _userRepository
            .GetByIdAsync(command.UserId, Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<AdminCannotDeleteOwnAccountException>();
        exception.Message.ShouldBe($"Admin with ID {_context.UserId} cannot delete their own account.");

        await _userRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        
        await _userRepository
            .DidNotReceive()
            .DeleteAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task given_regular_user_trying_to_delete_other_user_then_should_throw_user_cannot_delete_other_user_exception()
    {
        // Arrange
        _context.IsAdmin.Returns(false);
        var otherUserId = Guid.NewGuid();
        var command = new DeleteUserCommand(otherUserId);
        var user = _userFactory.CreateUser();

        _userRepository
            .GetByIdAsync(command.UserId, Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UserCannotDeleteOtherUserException>();
        exception.Message.ShouldBe($"User with ID {_context.UserId} cannot delete user with ID {otherUserId}.");

        await _userRepository
            .DidNotReceive()
            .DeleteAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task given_admin_deleting_other_user_then_should_delete_user()
    {
        // Arrange
        _context.IsAdmin.Returns(true);
        var otherUserId = Guid.NewGuid();
        var command = new DeleteUserCommand(otherUserId);
        var user = _userFactory.CreateUser();

        _userRepository
            .GetByIdAsync(command.UserId, Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        await Act(command);

        // Assert
        await _userRepository
            .Received(1)
            .DeleteAsync(user, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task given_user_deleting_own_account_then_should_delete_user()
    {
        // Arrange
        _context.IsAdmin.Returns(false);
        var command = new DeleteUserCommand(_context.UserId);
        var user = _userFactory.CreateUser();

        _userRepository
            .GetByIdAsync(command.UserId, Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        await Act(command);

        // Assert
        await _userRepository
            .Received(1)
            .DeleteAsync(user, Arg.Any<CancellationToken>());
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
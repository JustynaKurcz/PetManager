using PetManager.Application.Common.Context;
using PetManager.Application.Users.Admin.Commands.DeleteUser;
using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Enums;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;
using PetManager.Tests.Unit.Users.Factories;

namespace PetManager.Tests.Unit.Users.Handlers.Admin.DeleteUser;

public class DeleteUserByAdminCommandHandlerTests
{
    private async Task Act(DeleteUserByAdminCommand command)
        => await _handler.Handle(command, CancellationToken.None);

    [Fact]
    public async Task given_user_not_found_when_delete_user_then_should_throw_user_not_found_exception()
    {
        // Arrange
        var command = _userFactory.CreateDeleteUserByAdminCommand();
        _userRepository
            .GetByIdAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UserNotFoundException>();
        exception.Message.ShouldBe($"User with id {command.UserId} was not found.");

        await _userRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>());

        await _userRepository
            .DidNotReceive()
            .DeleteAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task given_admin_deleting_own_account_then_should_throw_admin_cannot_delete_own_account_exception()
    {
        // Arrange
        var admin = _userFactory.CreateUser(UserRole.Admin);
        _context.UserId.Returns(admin.Id);
        var command = new DeleteUserByAdminCommand(admin.Id);

        _userRepository
            .GetByIdAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(admin);

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<AdminCannotDeleteOwnAccountException>();
        exception.Message.ShouldBe($"Admin with ID {admin.Id} cannot delete their own account.");

        await _userRepository
            .DidNotReceive()
            .DeleteAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task given_admin_deleting_other_user_then_should_delete_user()
    {
        // Arrange
        var admin = _userFactory.CreateUser(UserRole.Admin);
        _context.UserId.Returns(admin.Id);
        
        var user = _userFactory.CreateUser();
        var command = new DeleteUserByAdminCommand(user.Id);

        _userRepository
            .GetByIdAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>())
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
    private readonly IRequestHandler<DeleteUserByAdminCommand> _handler;
    private readonly UserTestFactory _userFactory = new();

    public DeleteUserByAdminCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _context = Substitute.For<IContext>();

        _handler = new DeleteUserByAdminCommandHandler(_userRepository, _context);
    }
}
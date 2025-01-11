using PetManager.Application.Common.Context;
using PetManager.Application.Users.Commands.ChangeUserInformation;
using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;
using PetManager.Tests.Unit.Users.Factories;

namespace PetManager.Tests.Unit.Users.Handlers.Commands.ChangeUserInformation;

public sealed class ChangeUserInformationCommandHandlerTests
{
    private async Task Act(ChangeUserInformationCommand command)
        => await _handler.Handle(command, CancellationToken.None);

    [Fact]
    public async Task given_user_not_found_when_change_user_information_then_should_throw_user_not_found_exception()
    {
        // Arrange
        var command = _userFactory.CreateChangeUserInformationCommand();
        _userRepository
            .GetByIdAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UserNotFoundException>();
        exception.Message.ShouldBe($"User with id {_context.UserId} was not found.");

        await _userRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>());

        await _userRepository
            .DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task given_valid_data_when_change_user_information_then_should_change_user_information()
    {
        // Arrange
        var command = _userFactory.CreateChangeUserInformationCommand();
        var user = _userFactory.CreateUser();

        _userRepository
            .GetByIdAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        await Act(command);

        // Assert
        await _userRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>());

        await _userRepository
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());

        user.FirstName.ShouldBe(command.FirstName);
        user.LastName.ShouldBe(command.LastName);
    }

    [Fact]
    public async Task given_only_first_name_when_change_user_information_then_should_update_only_first_name()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        var existingLastName = user.LastName;
        var command = _userFactory.CreateChangeUserInformationCommandWithoutLastNameCommand();

        _userRepository
            .GetByIdAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        await Act(command);

        // Assert
        await _userRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>());

        await _userRepository
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());

        user.FirstName.ShouldBe(command.FirstName);
        user.LastName.ShouldBe(existingLastName);
    }

    [Fact]
    public async Task given_only_last_name_when_change_user_information_then_should_update_only_last_name()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        var existingFirstName = user.FirstName;
        var command = _userFactory.CreateChangeUserInformationCommandWithoutFirstNameCommand();

        _userRepository
            .GetByIdAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        await Act(command);

        // Assert
        await _userRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>());

        await _userRepository
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());

        user.FirstName.ShouldBe(existingFirstName);
        user.LastName.ShouldBe(command.LastName);
    }

    private readonly IUserRepository _userRepository;
    private readonly IContext _context;
    private readonly IRequestHandler<ChangeUserInformationCommand> _handler;
    private readonly UserTestFactory _userFactory = new();

    public ChangeUserInformationCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _context = Substitute.For<IContext>();

        _handler = new ChangeUserInformationCommandHandler(_userRepository, _context);
    }
}
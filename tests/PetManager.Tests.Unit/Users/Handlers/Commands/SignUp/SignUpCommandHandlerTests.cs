using PetManager.Application.Security;
using PetManager.Application.Users.Commands.SignUp;
using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Enums;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;

namespace PetManager.Tests.Unit.Users.Handlers.Commands.SignUp;

public sealed class SignUpCommandHandlerTests
{
    private async Task<SignUpResponse> Act(SignUpCommand command)
        => await _handler.Handle(command, CancellationToken.None);

    [Fact]
    public async Task given_valid_data_when_sign_up_then_should_create_user()
    {
        // Arrange
        var command = CreateSignUpCommand();
        var user = User.Create(command.Email, command.Password, UserRole.Client);

        _userRepository
            .ExistsByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(false);

        _passwordManager
            .HashPassword(command.Password)
            .Returns("hashedPassword");

        _userRepository
            .AddAsync(user, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        var response = await Act(command);

        // Assert
        response.ShouldNotBeNull();
        response.ShouldBeOfType<SignUpResponse>();
        response.UserId.ShouldNotBe(Guid.Empty);
        await _userRepository
            .Received(1)
            .ExistsByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());

        _passwordManager
            .Received(1)
            .HashPassword(Arg.Any<string>());

        await _userRepository
            .Received(1)
            .AddAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task given_existing_user_email_when_sign_up_then_should_throw_user_already_exists_exception()
    {
        // Arrange
        var command = CreateSignUpCommand();
        _userRepository
            .ExistsByEmailAsync(command.Email.ToLowerInvariant(), Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UserAlreadyExistsException>();
        await _userRepository
            .Received(1)
            .ExistsByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());

        await _userRepository
            .DidNotReceive()
            .AddAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    private SignUpCommand CreateSignUpCommand() =>
        new("TestFirstName", "TestLastName", "TestPassword", "test@petmanager.com");

    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly IRequestHandler<SignUpCommand, SignUpResponse> _handler;

    public SignUpCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordManager = Substitute.For<IPasswordManager>();

        _handler = new SignUpCommandHandler(_userRepository, _passwordManager);
    }
}
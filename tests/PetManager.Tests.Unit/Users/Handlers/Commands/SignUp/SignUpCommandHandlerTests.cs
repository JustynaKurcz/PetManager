using PetManager.Application.Shared.Security.Passwords;
using PetManager.Application.Users.Commands.SignUp;
using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;
using PetManager.Tests.Unit.Users.Factories;

namespace PetManager.Tests.Unit.Users.Handlers.Commands.SignUp;

public sealed class SignUpCommandHandlerTests
{
    private async Task<SignUpResponse> Act(SignUpCommand command)
        => await _handler.Handle(command, CancellationToken.None);

    [Fact]
    public async Task given_valid_data_when_sign_up_then_should_create_user()
    {
        // Arrange
        var command = _userFactory.CreateSignUpCommand();
        var user = _userFactory.CreateUser();

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
        var command = _userFactory.CreateSignUpCommand();
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
    
    [Fact]
    public async Task given_email_with_uppercase_letters_when_sign_up_then_should_convert_to_lowercase()
    {
        // Arrange
        const string upperCaseEmail = "TEST@EMAIL.COM";
        const string lowerCaseEmail = "test@email.com";
    
        var command = _userFactory.CreateSignUpCommand() with { Email = upperCaseEmail };

        _userRepository
            .ExistsByEmailAsync(lowerCaseEmail, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        await Act(command);

        // Assert
        await _userRepository
            .Received(1)
            .ExistsByEmailAsync(lowerCaseEmail, Arg.Any<CancellationToken>());
    }

    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;

    private readonly IRequestHandler<SignUpCommand, SignUpResponse> _handler;

    private readonly UserTestFactory _userFactory = new();

    public SignUpCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordManager = Substitute.For<IPasswordManager>();

        _handler = new SignUpCommandHandler(_userRepository, _passwordManager);
    }
}
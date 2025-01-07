using PetManager.Application.Shared.Security.Auth;
using PetManager.Application.Shared.Security.Passwords;
using PetManager.Application.Users.Commands.SignIn;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;
using PetManager.Tests.Unit.Users.Factories;

namespace PetManager.Tests.Unit.Users.Handlers.Commands.SignIn;

public sealed class SignInCommandHandlerTests
{
    private async Task<SignInResponse> Act(SignInCommand command)
        => await _handler.Handle(command, CancellationToken.None);

    [Fact]
    public async Task given_invalid_email_when_sign_in_then_should_throw_invalid_credentials_exception()
    {
        // Arrange
        var command = _userFactory.CreateSignInCommand();

        _userRepository
            .GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidCredentialsException>();
        exception.Message.ShouldBe("Invalid credentials");

        await _userRepository
            .Received(1)
            .GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());

        _passwordManager
            .DidNotReceive()
            .VerifyPassword(Arg.Any<string>(), Arg.Any<string>());

        _authManager
            .DidNotReceive()
            .GenerateToken(Arg.Any<Guid>(), Arg.Any<string>());
    }

    [Fact]
    public async Task given_invalid_password_when_sign_in_then_should_throw_invalid_credentials_exception()
    {
        // Arrange
        var command = _userFactory.CreateSignInCommand();
        var user = _userFactory.CreateUser();

        _userRepository
            .GetByEmailAsync(command.Email.ToLowerInvariant(), Arg.Any<CancellationToken>())
            .Returns(user);

        _passwordManager
            .VerifyPassword(command.Password, user.Password)
            .Returns(false);

        // Act

        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidCredentialsException>();
        exception.Message.ShouldBe("Invalid credentials");

        await _userRepository
            .Received(1)
            .GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());

        _passwordManager
            .Received(1)
            .VerifyPassword(Arg.Any<string>(), Arg.Any<string>());

        _authManager
            .DidNotReceive()
            .GenerateToken(Arg.Any<Guid>(), Arg.Any<string>());
    }

    [Fact]
    public async Task given_valid_credentials_when_sign_in_then_should_return_token()
    {
        // Arrange
        var command = _userFactory.CreateSignInCommand();
        var user = _userFactory.CreateUser();

        _userRepository
            .GetByEmailAsync(command.Email.ToLowerInvariant(), Arg.Any<CancellationToken>())
            .Returns(user);

        _passwordManager
            .VerifyPassword(command.Password, user.Password)
            .Returns(true);

        _authManager
            .GenerateToken(user.UserId, user.Role.ToString())
            .Returns("token");

        // Act
        var response = await Act(command);

        // Assert
        response.ShouldNotBeNull();
        response.ShouldBeOfType<SignInResponse>();
        await _userRepository
            .Received(1)
            .GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());

        _passwordManager
            .Received(1)
            .VerifyPassword(Arg.Any<string>(), Arg.Any<string>());

        _authManager
            .Received(1)
            .GenerateToken(Arg.Any<Guid>(), Arg.Any<string>());
    }
    
    [Fact]
    public async Task given_email_with_uppercase_letters_when_sign_in_then_should_convert_to_lowercase()
    {
        // Arrange
        const string upperCaseEmail = "TEST@EMAIL.COM";
        const string lowerCaseEmail = "test@email.com";
        
        var command = _userFactory.CreateSignInCommand() with { Email = upperCaseEmail };
        var user = _userFactory.CreateUser();

        _userRepository
            .GetByEmailAsync(lowerCaseEmail, Arg.Any<CancellationToken>())
            .Returns(user);

        _passwordManager
            .VerifyPassword(command.Password, user.Password)
            .Returns(true);

        _authManager
            .GenerateToken(user.UserId, user.Role.ToString())
            .Returns("token");

        // Act
        var response = await Act(command);

        // Assert
        response.ShouldNotBeNull();
        await _userRepository
            .Received(1)
            .GetByEmailAsync(lowerCaseEmail, Arg.Any<CancellationToken>());
    }

    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly IAuthManager _authManager;

    private readonly IRequestHandler<SignInCommand, SignInResponse> _handler;

    private readonly UserTestFactory _userFactory = new();

    public SignInCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordManager = Substitute.For<IPasswordManager>();
        _authManager = Substitute.For<IAuthManager>();

        _handler = new SignInCommandHandler(_userRepository, _passwordManager, _authManager);
    }
}
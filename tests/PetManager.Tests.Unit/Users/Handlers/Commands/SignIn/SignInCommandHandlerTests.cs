using NSubstitute.ReturnsExtensions;
using PetManager.Application.Auth;
using PetManager.Application.Security;
using PetManager.Application.Users.Commands.SignIn;
using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;

namespace PetManager.Tests.Unit.Users.Handlers.Commands.SignIn;

public sealed class SignInCommandHandlerTests
{
    [Fact]
    public async Task
        given_sign_in_command_when_sign_in_and_user_with_given_email_does_not_exist_then_should_throw_invalid_credentials_exception()
    {
        // Arrange
        var command = CreateSignInCommand();

        _userRepository
            .GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None));

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

        _tokenManager
            .DidNotReceive()
            .GenerateToken(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public async Task
        given_sign_in_command_when_sign_in_and_password_is_invalid_then_should_throw_invalid_credentials_exception()
    {
        // Arrange
        var command = CreateSignInCommand();

        _userRepository
            .GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(User.Create(command.Email, command.Password, CreateRole()));

        _passwordManager
            .VerifyPassword(command.Password, Arg.Any<string>())
            .Returns(false);

        // Act

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None));

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

        _tokenManager
            .DidNotReceive()
            .GenerateToken(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public async Task given_sign_in_command_when_sign_in_then_should_return_sign_in_response()
    {
        // Arrange
        var command = CreateSignInCommand();
        var user = User.Create(command.Email, command.Password, CreateRole());

        _userRepository
            .GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
            .Returns(user);

        _passwordManager
            .VerifyPassword(command.Password, Arg.Any<string>())
            .Returns(true);

        _tokenManager
            .GenerateToken(user.UserId, user.Role.Name, user.Email)
            .Returns("token");

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        response.ShouldNotBeNull();
        response.ShouldBeOfType<SignInResponse>();
        await _userRepository
            .Received(1)
            .GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());

        _passwordManager
            .Received(1)
            .VerifyPassword(Arg.Any<string>(), Arg.Any<string>());

        _tokenManager
            .Received(1)
            .GenerateToken(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>());
    }


    private SignInCommand CreateSignInCommand() =>
        new("test@petmanager.com", "TestPassword");

    private Role CreateRole() =>
        Role.Create(TestRole);

    private const string TestRole = "User";

    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly ITokenManager _tokenManager;
    private readonly IRequestHandler<SignInCommand, SignInResponse> _handler;

    public SignInCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordManager = Substitute.For<IPasswordManager>();
        _tokenManager = Substitute.For<ITokenManager>();

        _handler = new SignInCommandHandler(_userRepository, _passwordManager, _tokenManager);
    }
}
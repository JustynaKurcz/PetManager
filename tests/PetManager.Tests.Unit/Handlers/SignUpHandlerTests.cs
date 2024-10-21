using PetManager.Application.Security;
using PetManager.Application.Users.Commands.SignUp;
using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;

namespace PetManager.Tests.Unit.Handlers;

public sealed class SignUpHandlerTests
{
    [Fact]
    public async Task
        given_sign_up_command_when_sign_up_and_user_with_given_email_does_not_exist_then_should_create_user()
    {
        // Arrange
        var command = CreateSignUpCommand();
        _userRepository.ExistsByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);
        _passwordManager.HashPassword(Arg.Any<string>()).Returns("hashedPassword");
        _userRepository.AddAsync(Arg.Any<User>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        response.ShouldNotBeNull();
        response.ShouldBeOfType<SignUpResponse>();
        response.UserId.ShouldNotBe(Guid.Empty);
        await _userRepository.Received(1).ExistsByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
        _passwordManager.Received(1).HashPassword(Arg.Any<string>());
        await _userRepository.Received(1).AddAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }


    [Fact]
    public async Task
        given_sign_up_command_when_sign_up_and_user_with_given_email_exists_then_should_throw_user_already_exists_exception()
    {
        // Arrange
        var command = CreateSignUpCommand();
        _userRepository.ExistsByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UserAlreadyExistsException>();
        await _userRepository.Received(1).ExistsByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
        await _userRepository.DidNotReceive().AddAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    private SignUpCommand CreateSignUpCommand() =>
        new("TestFirstName", "TestLastName", "TestPassword", "test@petmanager.com");


    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly IRequestHandler<SignUpCommand, SignUpResponse> _handler;

    public SignUpHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _roleRepository = Substitute.For<IRoleRepository>();
        _passwordManager = Substitute.For<IPasswordManager>();

        _handler = new SignUpHandler(_userRepository, _roleRepository, _passwordManager);
    }
}
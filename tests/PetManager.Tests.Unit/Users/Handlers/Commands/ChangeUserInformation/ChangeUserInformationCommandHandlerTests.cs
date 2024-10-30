using PetManager.Application.Context;
using PetManager.Application.Users.Commands.ChangeUserInformation;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;
using PetManager.Tests.Unit.Users.Factories;

namespace PetManager.Tests.Unit.Users.Handlers.Commands.ChangeUserInformation;

public sealed class ChangeUserInformationCommandHandlerTests
{
    private async Task Act(ChangeUserInformationCommand command)
        => await _handler.Handle(command, CancellationToken.None);
    
    [Fact]
    public async Task given_invalid_user_id_when_change_user_information_then_should_throw_unauthorized_exception()
    {
        // Arrange
        var command = _userFactory.ChangeUserInformationCommand();
        _userRepository
            .GetByIdAsync(_context.UserId, Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));
        
        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UserNotFoundException>();
        exception.Message.ShouldBe($"User with id {_context.UserId} was not found.");
        
        
        await _userRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        
        await _userRepository
            .DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task given_valid_data_when_change_user_information_then_should_change_user_information()
    {
        // Arrange
        var command = _userFactory.ChangeUserInformationCommand();
        var user = _userFactory.CreateUser();
        
        _userRepository
            .GetByIdAsync(_context.UserId, Arg.Any<CancellationToken>())
            .Returns(user);
        
        // Act
        await Act(command);
        
        // Assert
        await _userRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        
        await _userRepository
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
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
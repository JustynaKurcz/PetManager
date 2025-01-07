using PetManager.Application.Pets.Commands.CreatePet;
using PetManager.Application.Shared.Context;
using PetManager.Core.HealthRecords.Repositories;
using PetManager.Core.Pets.Repositories;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;
using PetManager.Tests.Unit.Pets.Factories;

namespace PetManager.Tests.Unit.Pets.Handlers.Commands.CreatePet;

public sealed class CreatePetCommandHandlerTests
{
    private async Task<CreatePetResponse> Act(CreatePetCommand command)
        => await _handler.Handle(command, CancellationToken.None);

    [Fact]
    public async Task
        given_invalid_user_id_when_create_pet_then_should_throw_user_not_found_exception()
    {
        // Arrange
        var command = _petFactory.CreatePetCommand();
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
    }

    // [Fact]
    // public async Task Given_ValidData_When_CreatePet_Then_ShouldCreatePet()
    // {
    //     // Arrange
    //     var command = _petFactory.CreatePetCommand();
    //     var userId = Guid.NewGuid();
    //     _context.UserId.Returns(userId);
    //
    //     _userRepository
    //         .ExistsAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>())
    //         .Returns(true);
    //
    //     _petRepository
    //         .AddAsync(Arg.Any<Pet>(), Arg.Any<CancellationToken>())
    //         .Returns(Task.CompletedTask);
    //
    //     // Act
    //     var response = await Act(command);
    //
    //     // Assert
    //     response.Should().NotBeNull();
    //     response.Should().BeOfType<CreatePetResponse>();
    //     response.PetId.Should().NotBe(Guid.Empty);
    //
    //     await _userRepository
    //         .Received(1)
    //         .ExistsAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>());
    //
    //     await _petRepository
    //         .Received(1)
    //         .AddAsync(Arg.Any<Pet>(), Arg.Any<CancellationToken>());
    // }


    private readonly IRequestHandler<CreatePetCommand, CreatePetResponse> _handler;
    
    private readonly IUserRepository _userRepository;
    private readonly IPetRepository _petRepository;
    private readonly IHealthRecordRepository _healthRecordRepository;
    private readonly IContext _context;
    private readonly PetTestFactory _petFactory = new();

    public CreatePetCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _petRepository = Substitute.For<IPetRepository>();
        _healthRecordRepository = Substitute.For<IHealthRecordRepository>();
        _context = Substitute.For<IContext>();

        _handler = new CreatePetCommandHandler(
            _context,
            _userRepository, 
            _petRepository,
            _healthRecordRepository
            );
    }
}
using NSubstitute.ReturnsExtensions;
using PetManager.Application.Pets.Commands.CreatePet;
using PetManager.Core.HealthRecords.Repositories;
using PetManager.Core.Pets.Enums;
using PetManager.Core.Pets.Repositories;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;

namespace PetManager.Tests.Unit.Handlers;

public sealed class CreatePetCommandHandlerTests
{
    [Fact]
    public async Task
        given_create_pet_command_when_create_pet_and_user_does_not_exist_then_should_throw_user_not_found_exception()
    {
        // Arrange
        var command = CreateCreatePetCommand();
        _userRepository
            .GetByIdAsync(command.UserId, Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UserNotFoundException>();
        exception.Message.ShouldBe($"User with id {command.UserId} was not found.");

        await _userRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    private CreatePetCommand CreateCreatePetCommand() => new("TestName", Species.Dog, "TestBreed", Gender.Male,
        DateTimeOffset.UtcNow, Guid.NewGuid());

    private readonly IUserRepository _userRepository;
    private readonly IPetRepository _petRepository;
    private readonly IHealthRepository _healthRepository;

    private readonly IRequestHandler<CreatePetCommand, CreatePetResponse> _handler;

    public CreatePetCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _petRepository = Substitute.For<IPetRepository>();
        _healthRepository = Substitute.For<IHealthRepository>();

        _handler = new CreatePetCommandHandler(_userRepository, _petRepository, _healthRepository);
    }
}
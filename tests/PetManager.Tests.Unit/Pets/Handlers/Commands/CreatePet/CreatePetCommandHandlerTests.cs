using NSubstitute.ReturnsExtensions;
using PetManager.Application.Pets.Commands.CreatePet;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Repositories;
using PetManager.Core.Pets.Entities;
using PetManager.Core.Pets.Enums;
using PetManager.Core.Pets.Repositories;
using PetManager.Core.Users.Entities;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;

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
        var command = CreateCreatePetCommand();
        _userRepository
            .GetByIdAsync(command.UserId, Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UserNotFoundException>();
        exception.Message.ShouldBe($"User with id {command.UserId} was not found.");

        await _userRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task given_valid_data_when_create_pet_then_should_create_pet()
    {
        // Arrange
        var command = CreateCreatePetCommand();
        var user = User.Create("TestEmail", "TestPassword", Role.Create("User"));
        var pet = Pet.Create(command.Name, command.Species, command.Breed, command.Gender, command.BirthDate,
            command.UserId);
        var healthRecord = HealthRecord.Create(pet.PetId);
        _userRepository
            .GetByIdAsync(command.UserId, Arg.Any<CancellationToken>())
            .Returns(user);

        _petRepository
            .AddAsync(pet, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _healthRecordRepository
            .AddAsync(healthRecord, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        // Act
        var response = await Act(command);

        // Assert
        response.ShouldNotBeNull();
        response.ShouldBeOfType<CreatePetResponse>();
        response.PetId.ShouldNotBe(Guid.Empty);

        await _userRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());

        await _petRepository
            .Received(1)
            .AddAsync(Arg.Any<Pet>(), Arg.Any<CancellationToken>());

        await _healthRecordRepository
            .Received(1)
            .AddAsync(Arg.Any<HealthRecord>(), Arg.Any<CancellationToken>());
    }

    private CreatePetCommand CreateCreatePetCommand() => new("TestName", Species.Dog, "TestBreed", Gender.Male,
        DateTimeOffset.UtcNow, Guid.NewGuid());

    private readonly IUserRepository _userRepository;
    private readonly IPetRepository _petRepository;
    private readonly IHealthRecordRepository _healthRecordRepository;

    private readonly IRequestHandler<CreatePetCommand, CreatePetResponse> _handler;

    public CreatePetCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _petRepository = Substitute.For<IPetRepository>();
        _healthRecordRepository = Substitute.For<IHealthRecordRepository>();

        _handler = new CreatePetCommandHandler(_userRepository, _petRepository, _healthRecordRepository);
    }
}
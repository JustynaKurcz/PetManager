using PetManager.Application.Context;
using PetManager.Application.Pets.Commands.CreatePet;
using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Repositories;
using PetManager.Core.Pets.Entities;
using PetManager.Core.Pets.Repositories;
using PetManager.Core.Users.Exceptions;
using PetManager.Core.Users.Repositories;
using PetManager.Tests.Unit.HealthRecords.Factories;
using PetManager.Tests.Unit.Pets.Factories;
using PetManager.Tests.Unit.Users.Factories;

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

    [Fact]
    public async Task given_valid_data_when_create_pet_then_should_create_pet()
    {
        // Arrange
        var command = _petFactory.CreatePetCommand();
        var user = _userFactory.CreateUser();
        var pet = _petFactory.CreatePet();
        var healthRecord = _healthRecordFactory.CreateHealthRecord();

        _userRepository
            .GetByIdAsync(_context.UserId, Arg.Any<CancellationToken>())
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

    private readonly IContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IPetRepository _petRepository;
    private readonly IHealthRecordRepository _healthRecordRepository;

    private readonly IRequestHandler<CreatePetCommand, CreatePetResponse> _handler;

    private readonly PetTestFactory _petFactory = new();
    private readonly UserTestFactory _userFactory = new();
    private readonly HealthRecordTestFactory _healthRecordFactory = new();

    public CreatePetCommandHandlerTests()
    {
        _context = Substitute.For<IContext>();
        _userRepository = Substitute.For<IUserRepository>();
        _petRepository = Substitute.For<IPetRepository>();
        _healthRecordRepository = Substitute.For<IHealthRecordRepository>();

        _handler = new CreatePetCommandHandler(_context, _userRepository, _petRepository, _healthRecordRepository);
    }
}
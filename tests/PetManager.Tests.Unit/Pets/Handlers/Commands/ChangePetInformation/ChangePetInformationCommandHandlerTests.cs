using PetManager.Application.Pets.Commands.ChangePetInformation;
using PetManager.Application.Shared.Context;
using PetManager.Core.Pets.Entities;
using PetManager.Core.Pets.Exceptions;
using PetManager.Core.Pets.Repositories;
using PetManager.Tests.Unit.Pets.Factories;

namespace PetManager.Tests.Unit.Pets.Handlers.Commands.ChangePetInformation;

public sealed class ChangePetInformationCommandHandlerTests
{
    private async Task Act(ChangePetInformationCommand command)
        => await _handler.Handle(command, CancellationToken.None);

    [Fact]
    public async Task given_invalid_pet_id_when_change_pet_information_then_should_throw_pet_not_found_exception()
    {
        // Arrange
        var command = _petFactory.ChangePetInformationCommand();
        _petRepository
            .GetByIdAsync(Arg.Any<Expression<Func<Pet, bool>>>(), Arg.Any<CancellationToken>(), Arg.Any<bool>())
            .ReturnsNull();

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<PetNotFoundException>();
        exception.Message.ShouldBe($"Pet with id {command.PetId} was not found.");

        await _petRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Expression<Func<Pet, bool>>>(), Arg.Any<CancellationToken>(), Arg.Any<bool>());
    }

    [Fact]
    public async Task given_valid_data_when_change_pet_information_then_should_change_pet_information()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _context.UserId.Returns(userId);

        var command = _petFactory.ChangePetInformationCommand();
        var pet = _petFactory.CreatePet(userId);

        _petRepository
            .GetByIdAsync(Arg.Any<Expression<Func<Pet, bool>>>(), Arg.Any<CancellationToken>(), Arg.Any<bool>())
            .Returns(pet);

        // Act
        await Act(command);

        // Assert
        await _petRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Expression<Func<Pet, bool>>>(), Arg.Any<CancellationToken>(), Arg.Any<bool>());

        await _petRepository
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    private readonly IContext _context;
    private readonly IPetRepository _petRepository;
    private readonly IRequestHandler<ChangePetInformationCommand> _handler;
    private readonly PetTestFactory _petFactory = new();

    public ChangePetInformationCommandHandlerTests()
    {
        _context = Substitute.For<IContext>();
        _petRepository = Substitute.For<IPetRepository>();

        _handler = new ChangePetInformationCommandHandler(_context, _petRepository);
    }
}
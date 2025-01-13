using PetManager.Application.Pets.Commands.DeletePet;
using PetManager.Core.Pets.Entities;
using PetManager.Core.Pets.Exceptions;
using PetManager.Core.Pets.Repositories;
using PetManager.Tests.Unit.Pets.Factories;

namespace PetManager.Tests.Unit.Pets.Handlers.Commands.DeletePet;

public sealed class DeletePetCommandHandlerTests
{
    private async Task Act(DeletePetCommand command)
        => await _handler.Handle(command, CancellationToken.None);

    [Fact]
    public async Task given_invalid_pet_id_when_delete_pet_then_should_throw_pet_not_found_exception()
    {
        // Arrange
        var command = _petFactory.DeletePetCommand();
        _petRepository
            .GetAsync(Arg.Any<Expression<Func<Pet, bool>>>(), Arg.Any<CancellationToken>(), Arg.Any<bool>())
            .ReturnsNull();

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<PetNotFoundException>();
        exception.Message.ShouldBe($"Pet with id {command.PetId} was not found.");

        await _petRepository
            .Received(1)
            .GetAsync(Arg.Any<Expression<Func<Pet, bool>>>(), Arg.Any<CancellationToken>(), Arg.Any<bool>());

        await _petRepository
            .DidNotReceive()
            .DeleteAsync(Arg.Any<Pet>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task given_valid_pet_id_when_delete_pet_then_should_delete_pet()
    {
        // Arrange
        var command = _petFactory.DeletePetCommand();
        var pet = _petFactory.CreatePet();
        _petRepository
            .GetAsync(Arg.Any<Expression<Func<Pet, bool>>>(), Arg.Any<CancellationToken>(), Arg.Any<bool>())
            .Returns(pet);

        // Act
        await Act(command);

        // Assert
        await _petRepository
            .Received(1)
            .GetAsync(Arg.Any<Expression<Func<Pet, bool>>>(), Arg.Any<CancellationToken>(), Arg.Any<bool>());

        await _petRepository
            .Received(1)
            .DeleteAsync(Arg.Any<Pet>(), Arg.Any<CancellationToken>());
    }

    private readonly IPetRepository _petRepository;
    private readonly IRequestHandler<DeletePetCommand> _handler;
    private readonly PetTestFactory _petFactory = new();

    public DeletePetCommandHandlerTests()
    {
        _petRepository = Substitute.For<IPetRepository>();
        _handler = new DeletePetCommandHandler(_petRepository);
    }
}
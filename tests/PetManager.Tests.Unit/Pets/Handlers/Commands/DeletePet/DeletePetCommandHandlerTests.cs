using PetManager.Application.Pets.Commands.DeletePet;
using PetManager.Core.Pets.Entities;
using PetManager.Core.Pets.Enums;
using PetManager.Core.Pets.Exceptions;
using PetManager.Core.Pets.Repositories;

namespace PetManager.Tests.Unit.Pets.Handlers.Commands.DeletePet;

public sealed class DeletePetCommandHandlerTests
{
    private async Task Act(DeletePetCommand command)
        => await _handler.Handle(command, CancellationToken.None);

    [Fact]
    public async Task given_invalid_pet_id_when_delete_pet_then_should_throw_pet_not_found_exception()
    {
        // Arrange
        var command = DeletePetCommand();
        _petRepository
            .GetByIdAsync(command.PetId, Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<PetNotFoundException>();
        exception.Message.ShouldBe($"Pet with id {command.PetId} was not found.");

        await _petRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task given_valid_pet_id_when_delete_pet_then_should_delete_pet()
    {
        // Arrange
        var command = DeletePetCommand();
        var pet = Pet.Create("TestName", Species.Dog, "TestBreed", Gender.Female, DateTime.Now, Guid.NewGuid());
        _petRepository
            .GetByIdAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns(pet);

        // Act
        await Act(command);

        // Assert
        await _petRepository
            .Received(1)
            .DeleteAsync(Arg.Any<Pet>(), Arg.Any<CancellationToken>());
    }

    private DeletePetCommand DeletePetCommand()
        => new(Guid.NewGuid());
    
    private readonly IPetRepository _petRepository;
    private readonly IRequestHandler<DeletePetCommand> _handler;
    
    public DeletePetCommandHandlerTests()
    {
        _petRepository = Substitute.For<IPetRepository>();
        _handler = new DeletePetCommandHandler(_petRepository);
    }
}
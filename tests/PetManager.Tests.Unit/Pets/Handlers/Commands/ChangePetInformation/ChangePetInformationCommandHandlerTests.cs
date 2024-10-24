using PetManager.Application.Pets.Commands.ChangePetInformation;
using PetManager.Core.Pets.Entities;
using PetManager.Core.Pets.Enums;
using PetManager.Core.Pets.Exceptions;
using PetManager.Core.Pets.Repositories;

namespace PetManager.Tests.Unit.Pets.Handlers.Commands.ChangePetInformation;

public sealed class ChangePetInformationCommandHandlerTests
{
    private async Task Act(ChangePetInformationCommand command)
        => await _handler.Handle(command, CancellationToken.None);


    [Fact]
    public async Task given_invalid_pet_id_when_change_pet_information_then_should_throw_pet_not_found_exception()
    {
        // Arrange
        var command = ChangePetInformationCommand();
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
    public async Task given_valid_data_when_change_pet_information_then_should_change_pet_information()
    {
        // Arrange
        var command = ChangePetInformationCommand();
        var pet = Pet.Create("TestName", Species.Cat, "TestBreed", Gender.Female, DateTime.Now, Guid.NewGuid());

        _petRepository
            .GetByIdAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns(pet);

        // Act
        await Act(command);

        // Assert
        await _petRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    private ChangePetInformationCommand ChangePetInformationCommand()
        => new("TestName", Species.Cat, "TestBreed", Gender.Female, DateTime.Now);

    private readonly IPetRepository _petRepository;
    private readonly IRequestHandler<ChangePetInformationCommand> _handler;

    public ChangePetInformationCommandHandlerTests()
    {
        _petRepository = Substitute.For<IPetRepository>();

        _handler = new ChangePetInformationCommandHandler(_petRepository);
    }
}
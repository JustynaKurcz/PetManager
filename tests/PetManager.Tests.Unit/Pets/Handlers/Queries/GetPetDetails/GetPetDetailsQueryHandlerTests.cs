using PetManager.Application.Pets.Queries.GetPetDetails;
using PetManager.Application.Pets.Queries.GetPetDetails.DTO;
using PetManager.Core.Pets.Exceptions;
using PetManager.Core.Pets.Repositories;
using PetManager.Infrastructure.EF.Pets.Queries.GetPetDetails;
using PetManager.Tests.Unit.Pets.Factories;

namespace PetManager.Tests.Unit.Pets.Handlers.Queries.GetPetDetails;

public sealed class GetPetDetailsQueryHandlerTests
{
    private async Task<PetDetailsDto> Act(GetPetDetailsQuery query)
        => await _handler.Handle(query, CancellationToken.None);

    [Fact]
    public async Task given_invalid_pet_id_when_get_pet_details_then_should_throw_pet_not_found_exception()
    {
        // Arrange
        var query = _petFactory.GetPetDetailsQuery();
        _petRepository
            .GetByIdAsync(query.PetId, Arg.Any<CancellationToken>(), Arg.Any<bool>())
            .ReturnsNull();

        // Act
        var exception = await Record.ExceptionAsync(() => Act(query));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<PetNotFoundException>();
        exception.Message.ShouldBe($"Pet with id {query.PetId} was not found.");

        await _petRepository
            .Received(1)
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>(), Arg.Any<bool>());
    }

    [Fact]
    public async Task given_valid_pet_id_when_get_pet_details_then_should_return_pet_details()
    {
        // Arrange
        var query = _petFactory.GetPetDetailsQuery();
        var pet = _petFactory.CreatePet();

        _petRepository
            .GetByIdAsync(query.PetId, Arg.Any<CancellationToken>(), Arg.Any<bool>())
            .Returns(pet);

        // Act
        var response = await Act(query);

        // Assert
        response.ShouldNotBeNull();
        response.ShouldBeOfType<PetDetailsDto>();
    }
    private readonly IPetRepository _petRepository;

    private readonly IRequestHandler<GetPetDetailsQuery, PetDetailsDto> _handler;
    
    private readonly PetTestFactory _petFactory = new();

    public GetPetDetailsQueryHandlerTests()
    {
        _petRepository = Substitute.For<IPetRepository>();

        _handler = new GetPetDetailsQueryHandler(_petRepository);
    }
}
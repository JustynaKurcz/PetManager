using PetManager.Application.Common.Context;
using PetManager.Application.Common.Pagination;
using PetManager.Application.Pets.Queries.BrowsePets;
using PetManager.Application.Pets.Queries.BrowsePets.DTO;
using PetManager.Core.Pets.Repositories;
using PetManager.Infrastructure.EF.Pets.Queries.BrowsePets;
using PetManager.Tests.Unit.Pets.Factories;

namespace PetManager.Tests.Unit.Pets.Handlers.Queries.BrowsePets;

public sealed class BrowsePetsQueryHandlerTests
{
    private async Task<PaginationResult<PetDto>> Act(BrowsePetsQuery query)
        => await _handler.Handle(query, CancellationToken.None);

    [Fact]
    public async Task given_valid_query_when_browse_pets_then_should_return_pets()
    {
        // Arrange
        var query = new BrowsePetsQuery();
        var userId = Guid.NewGuid();
        var pets = await _petFactory.CreatePets();

        _context.UserId.Returns(userId);
        _petRepository.BrowseAsync(userId, Arg.Any<CancellationToken>())
            .Returns(pets);

        // Act
        var result = await Act(query);

        // Assert
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(pets.Count());
    }

    [Fact]
    public async Task given_valid_query_when_empty_pets_then_should_return_empty_list()
    {
        // Arrange
        var query = new BrowsePetsQuery();
        var userId = Guid.NewGuid();

        _context.UserId.Returns(userId);
        _petRepository.BrowseAsync(userId, Arg.Any<CancellationToken>())
            .Returns([]);

        // Act
        var result = await Act(query);

        // Assert
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(0);
    }

    private readonly IPetRepository _petRepository;
    private readonly IContext _context;
    private readonly PetTestFactory _petFactory = new();
    
    
    private readonly IRequestHandler<BrowsePetsQuery, PaginationResult<PetDto>> _handler;

    public BrowsePetsQueryHandlerTests()
    {
        _petRepository = Substitute.For<IPetRepository>();
        _context = Substitute.For<IContext>();
        
        _handler = new BrowsePetsQueryHandler(_context, _petRepository);
    }
}
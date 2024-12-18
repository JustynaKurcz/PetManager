using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PetManager.Application.Pagination;
using PetManager.Application.Pets.Queries.BrowsePets.DTO;
using PetManager.Infrastructure.EF.DbContext;
using PetManager.Tests.Integration.Common;
using PetManager.Tests.Integration.Users;

namespace PetManager.Tests.Integration.Pets.Endpoints.Queries.BrowsePets;

public class BrowsePetsEndpointTests : IClassFixture<PetManagerTestFactory>, IAsyncLifetime
{
    private readonly IServiceScope _scope;
    private readonly UserTestFactory _userFactory = new();
    private readonly PetTestFactory _petFactory = new();
    private readonly HttpClient _client;
    private readonly PetManagerDbContext _dbContext;

    public BrowsePetsEndpointTests()
    {
        var factory = new PetManagerTestFactory();
        _scope = factory.Services.CreateScope();
        _client = factory.CreateClient();
        _dbContext = _scope.ServiceProvider.GetRequiredService<PetManagerDbContext>();
    }

    [Fact]
    public async Task GetAll_WithQueryParameters_ReturnsPaginatedList()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await _dbContext.Users.AddAsync(user);

        var petCount = 10;
        var pets = _petFactory.CreatePets(user.UserId, petCount);
        await _dbContext.Pets.AddRangeAsync(pets);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("api/v1/pets?PageNumber=2&PageSize=5");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    public async Task InitializeAsync()
    {
        await _dbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.DisposeAsync();
    }
}
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using PetManager.Application.Auth;
using PetManager.Application.Pets.Commands.CreatePet;
using PetManager.Infrastructure.EF.DbContext;
using PetManager.Tests.Integration.Common;
using PetManager.Tests.Integration.Users;

namespace PetManager.Tests.Integration.Pets.Endpoints.Commands.CreatePet;

public class CreatePetEndpointTests: IClassFixture<PetManagerTestFactory>, IAsyncLifetime
{
    private readonly IServiceScope _scope;
    private readonly UserTestFactory _userFactory = new();
    private readonly PetTestFactory _petFactory = new();
    private readonly HttpClient _client;
    private readonly PetManagerDbContext _dbContext;
    private readonly IAuthManager _authManager;

    public CreatePetEndpointTests()
    {
        var factory = new PetManagerTestFactory();
        _scope = factory.Services.CreateScope();
        _client = factory.CreateClient();
        _dbContext = _scope.ServiceProvider.GetRequiredService<PetManagerDbContext>();
        _authManager = _scope.ServiceProvider.GetRequiredService<IAuthManager>();
    }
    
    [Fact]
    public async Task create_pet_without_being_authorized_should_return_unauthorized_status_code()
    {
        // Arrange
        var command = _petFactory.CreatePetCommand();
        
        // Act
        var response = await _client.PostAsJsonAsync("api/v1/pets", command);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreatePet_WithValidCommand_ReturnsSuccessStatusCode()
    {
        // Arrange
        var command = _petFactory.CreatePetCommand();
        var user = _userFactory.CreateUser();
        
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        
        Authenticate(user.UserId, user.Role.ToString());
        
        // Act
        var response = await _client.PostAsJsonAsync("api/v1/pets", command);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        await response.Content.ReadFromJsonAsync<CreatePetResponse>().ShouldNotBeNull();

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
    
    private void Authenticate(Guid userId, string role)
     {
         var token = _authManager.GenerateToken(userId, role);
         _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
     }
}
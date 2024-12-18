using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using PetManager.Application.Auth;
using PetManager.Infrastructure.EF.DbContext;
using PetManager.Tests.Integration.Common;
using PetManager.Tests.Integration.Users;

namespace PetManager.Tests.Integration.Pets.Endpoints.Commands.DeletePet;

public class DeletePetEndpointTests : IClassFixture<PetManagerTestFactory>, IAsyncLifetime
{
    private readonly IServiceScope _scope;
    private readonly UserTestFactory _userFactory = new();
    private readonly PetTestFactory _petFactory = new();
    private readonly HttpClient _client;
    private readonly PetManagerDbContext _dbContext;
    private readonly IAuthManager _authManager;

    public DeletePetEndpointTests()
    {
        var factory = new PetManagerTestFactory();
        _scope = factory.Services.CreateScope();
        _client = factory.CreateClient();
        _dbContext = _scope.ServiceProvider.GetRequiredService<PetManagerDbContext>();
        _authManager = _scope.ServiceProvider.GetRequiredService<IAuthManager>();
    }
    
    [Fact]
    public async Task delete_pet_without_being_authorized_should_return_unauthorized_status_code()
    {
        // Arrange
        var petId = Guid.NewGuid();
        
        // Act
        var response = await _client.DeleteAsync($"api/v1/pets/{petId}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task delete_pet_given_invalid_pet_id_should_return_not_found_status_code()
    {
        // Arrange
        var petId = Guid.NewGuid();
        var user = _userFactory.CreateUser();
        
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        
        Authenticate(user.UserId, user.Role.ToString());

        // Act
        var response = await _client.DeleteAsync($"api/v1/pets/{petId}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task delete_pet_given_valid_pet_id_should_return_no_content_status_code()
    {
        // Arrange
        var user = _userFactory.CreateUser();
        await _dbContext.Users.AddAsync(user);
        
        Authenticate(user.UserId, user.Role.ToString());
        
        var pet = _petFactory.CreatePet(user.UserId);
        await _dbContext.Pets.AddAsync(pet);
        await _dbContext.SaveChangesAsync();
        

        // Act
        var response = await _client.DeleteAsync($"api/v1/pets/{pet.PetId}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
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
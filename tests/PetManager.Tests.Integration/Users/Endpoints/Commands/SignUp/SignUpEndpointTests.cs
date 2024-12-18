using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using PetManager.Application.Security;
using PetManager.Application.Users.Commands.SignUp;
using PetManager.Infrastructure.EF.DbContext;

namespace PetManager.Tests.Integration.Users.Endpoints.Commands.SignUp;

public class SignUpEndpointTests : IClassFixture<PetManagerTestFactory>, IAsyncLifetime
{
    private readonly IServiceScope _scope;
    private readonly UserTestFactory _userFactory = new();
    private readonly HttpClient _client;
    private readonly PetManagerDbContext _dbContext;
    private readonly IPasswordManager _passwordManager;

    public SignUpEndpointTests()
    {
        var factory = new PetManagerTestFactory();
        _scope = factory.Services.CreateScope();
        _client = factory.CreateClient();
        _dbContext = _scope.ServiceProvider.GetRequiredService<PetManagerDbContext>();
        _passwordManager = _scope.ServiceProvider.GetRequiredService<IPasswordManager>();
    }

    [Fact]
    public async Task RegisterUser_ForValidModel_ReturnsOk()
    {
        // Arrange
        var command = _userFactory.CreateSignUpCommand();

        // Act
        var response = await _client.PostAsJsonAsync("api/v1/users/sign-up", command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        await response.Content.ReadFromJsonAsync<SignUpResponse>().ShouldNotBeNull();
    }

    [Fact]
    public async Task RegisterUser_ForDuplicateEmail_ReturnsBadRequest()
    {
        // Arrange
        var command = _userFactory.CreateSignUpCommand();
        var hashedPassword = _passwordManager.HashPassword(command.Password);
        var user = _userFactory.CreateUser(command.Email.ToLowerInvariant(), hashedPassword);
        
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        // Act 
        var response = await _client.PostAsJsonAsync("api/v1/users/sign-up", command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
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
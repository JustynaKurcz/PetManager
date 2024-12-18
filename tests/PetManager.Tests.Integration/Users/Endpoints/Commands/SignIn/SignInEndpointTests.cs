using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PetManager.Application.Security;
using PetManager.Application.Users.Commands.SignIn;
using PetManager.Infrastructure.EF.DbContext;

namespace PetManager.Tests.Integration.Users.Endpoints.Commands.SignIn;

public class SignInEndpointTests : IClassFixture<PetManagerTestFactory>, IAsyncLifetime
{
    private readonly IServiceScope _scope;
    private readonly UserTestFactory _userFactory = new();
    private readonly HttpClient _client;
    private readonly PetManagerDbContext _dbContext;
    private readonly IPasswordManager _passwordManager;

    public SignInEndpointTests()
    {
        var factory = new PetManagerTestFactory();
        _scope = factory.Services.CreateScope();
        _client = factory.CreateClient();
        _dbContext = _scope.ServiceProvider.GetRequiredService<PetManagerDbContext>();
        _passwordManager = _scope.ServiceProvider.GetRequiredService<IPasswordManager>();
    }

    [Fact]
    public async Task SignIn_ForValidCredentials_ReturnsOkWithToken()
    {
        // Arrange
        var command = _userFactory.CreateSignUpCommand();
        var hashedPassword = _passwordManager.HashPassword(command.Password);
        var user = _userFactory.CreateUser(command.Email.ToLowerInvariant(), hashedPassword);
        
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        
        var signInCommand = _userFactory.CreateSignInCommand(command.Email, command.Password);

        // Act
        var response = await _client.PostAsJsonAsync("api/v1/users/sign-in", signInCommand);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        await response.Content.ReadFromJsonAsync<SignInResponse>().ShouldNotBeNull();
    }

    [Fact]
    public async Task SignIn_ForInvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var signInCommand = _userFactory.CreateSignInCommand();

        // Act
        var response = await _client.PostAsJsonAsync("api/v1/users/sign-in", signInCommand);

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
using System.Net.Http.Headers;
using PetManager.Application.Common.Security.Auth;
using PetManager.Application.Common.Security.Passwords;
using PetManager.Infrastructure.EF.DbContext;
using PetManager.Tests.Integration.Users.Factories;

namespace PetManager.Tests.Integration.Configuration;

[Collection("IntegrationTests")]
public abstract class IntegrationTestBase : IClassFixture<PetManagerTestFactory>, IAsyncLifetime
{
    private readonly IServiceScope _scope;
    protected readonly HttpClient _client;
    private readonly PetManagerDbContext _dbContext;
    protected readonly IPasswordManager _passwordManager;
    private readonly IAuthManager _authManager;
    private readonly UserTestFactory _userFactory = new();

    protected IntegrationTestBase()
    {
        var factory = new PetManagerTestFactory();
        _scope = factory.Services.CreateScope();
        _client = factory.CreateClient();
        _dbContext = _scope.ServiceProvider.GetRequiredService<PetManagerDbContext>();
        _passwordManager = _scope.ServiceProvider.GetRequiredService<IPasswordManager>();
        _authManager = _scope.ServiceProvider.GetRequiredService<IAuthManager>();
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

    protected async Task AddAsync<T>(T entity) where T : class
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    protected async Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class
    {
        await _dbContext.Set<T>().AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
    }

    protected async Task Authenticate(Guid userId, string role)
    {
        var token = await _authManager.GenerateToken(userId, role);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
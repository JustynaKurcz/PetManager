using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using PetManager.Infrastructure.EF.DbContext;

namespace PetManager.Tests.Integration.Configuration
{
    public class PetManagerTestFactory : WebApplicationFactory<Api.Program>
    {
        private const string AppSettings = "appsettings.test.json";
        private const string ConnectionString = "PetManagerDbTest";
        private string _dbName;

        public PetManagerTestFactory()
        {
            _dbName = $"PetManagerDbTest_{Guid.NewGuid()}";
        }

        private readonly IConfiguration _configuration = new ConfigurationBuilder()
            .AddJsonFile(AppSettings)
            .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services
                    .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<PetManagerDbContext>));

                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                var connectionString = _configuration.GetConnectionString(ConnectionString);

                services.AddDbContext<PetManagerDbContext>(options =>
                {
                    options.UseNpgsql(connectionString?.Replace("PetManager_test", _dbName))
                        .EnableSensitiveDataLogging(false)
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
            });
        }
    }
}
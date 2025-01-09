using PetManager.Core.HealthRecords.Repositories;
using PetManager.Core.Pets.Repositories;
using PetManager.Core.Users.Repositories;
using PetManager.Infrastructure.EF.HealthRecords.Repositories;
using PetManager.Infrastructure.EF.Pets.Repositories;
using PetManager.Infrastructure.EF.Users.Repositories;

namespace PetManager.Infrastructure.EF;

internal static class DataAccess
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IPetRepository, PetRepository>()
            .AddScoped<IHealthRecordRepository, HealthRecordRepository>()
            .AddScoped<IVaccinationRepository, VaccinationRepository>();

        return services;
    }
}
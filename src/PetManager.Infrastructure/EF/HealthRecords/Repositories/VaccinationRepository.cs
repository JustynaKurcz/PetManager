using PetManager.Core.HealthRecords.Entities;
using PetManager.Core.HealthRecords.Repositories;
using PetManager.Infrastructure.EF.DbContext;

namespace PetManager.Infrastructure.EF.HealthRecords.Repositories;

internal class VaccinationRepository(PetManagerDbContext dbContext) : IVaccinationRepository
{
    private readonly DbSet<Vaccination> _vaccinations = dbContext.Vaccinations;

    public async Task<IEnumerable<Vaccination>> GetScheduledVaccinationsAsync(CancellationToken cancellationToken)
    {
        var today = DateTimeOffset.UtcNow;
        var nextWeek = today.AddDays(7);

        return await _vaccinations
            .Include(v => v.HealthRecord)
            .ThenInclude(hr => hr.Pet)
            .ThenInclude(p => p.User)
            .Where(v => v.NextVaccinationDate >= today && v.NextVaccinationDate <= nextWeek && !v.IsNotificationSent)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateVaccinationAsync(Vaccination vaccination, CancellationToken cancellationToken)
        => await Task.FromResult(_vaccinations.Update(vaccination));

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await dbContext.SaveChangesAsync(cancellationToken);
}
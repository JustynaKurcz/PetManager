using PetManager.Application.Common.Emails;
using PetManager.Infrastructure.Common.QuartzJobs.Base.Options;
using Quartz;

namespace PetManager.Infrastructure.Common.QuartzJobs.Base;

public abstract class BaseReminderJob<TEntity>(
    IServiceScopeFactory scopeFactory,
    IEmailService emailService,
    ReminderOptions options
) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CheckAndSendReminders();
    }
    
    protected abstract Task<IEnumerable<TEntity>> GetScheduledItems(IServiceScope scope);
    protected abstract Task UpdateItem(TEntity item, IServiceScope scope);
    protected abstract Task SendReminderEmail(TEntity item, CancellationToken cancellationToken);
    private async Task CheckAndSendReminders()
    {
        using var scope = scopeFactory.CreateScope();
        var scheduledItems = await GetScheduledItems(scope);

        foreach (var item in scheduledItems)
        {
            await SendReminderEmail(item, CancellationToken.None);
            await UpdateItem(item, scope);
        }
    }
}
using System.Reflection;
using PetManager.Infrastructure.Common.QuartzJobs.Base.Options;
using Quartz;

namespace PetManager.Infrastructure.Common.QuartzJobs.Configuration;

internal static class QuartzJobsExtensions
{
    private const string DailyAtNoonCronExpression = "0 0 9 ? * * *";
    private const string SectionName = "ReminderSettings";

    public static IServiceCollection AddQuartzJobs(this IServiceCollection services, IConfiguration configuration)
    {
        var reminderOptions = configuration.GetSection(SectionName).Get<ReminderOptions>();
        services.AddSingleton(reminderOptions);

        services.AddQuartz(RegisterJobs);

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        return services;
    }

    private static void RegisterJobs(IServiceCollectionQuartzConfigurator configurator)
    {
        var jobTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => !t.IsInterface && !t.IsAbstract && typeof(IJob).IsAssignableFrom(t))
            .ToList();

        foreach (var jobType in jobTypes)
        {
            var method = typeof(QuartzJobsExtensions)
                .GetMethod(nameof(AddJob), BindingFlags.NonPublic | BindingFlags.Static)
                ?.MakeGenericMethod(jobType);

            method?.Invoke(null, [configurator]);
        }
    }

    private static void AddJob<T>(IServiceCollectionQuartzConfigurator configurator) where T : IJob
    {
        var jobName = typeof(T).Name;

        configurator.AddJob<T>(j => j
            .WithIdentity(jobName)
            .DisallowConcurrentExecution()
            .Build()
        );

        configurator.AddTrigger(t => t
            .ForJob(jobName)
            .WithIdentity($"{jobName}-trigger")
            .WithCronSchedule(DailyAtNoonCronExpression)
        );
    }
}
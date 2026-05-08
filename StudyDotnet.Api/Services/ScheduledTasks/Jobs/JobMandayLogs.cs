using Quartz;

namespace StudyDotnet.Services.ScheduledTasks.Jobs;

[DisallowConcurrentExecution]
public sealed class JobMandayLogs : IJob
{
    private readonly ILogger<JobMandayLogs> _logger;

    public JobMandayLogs(ILogger<JobMandayLogs> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Study job: pull attendance logs for manday calculation at {RunAt}.", DateTimeOffset.Now);
        return Task.CompletedTask;
    }
}

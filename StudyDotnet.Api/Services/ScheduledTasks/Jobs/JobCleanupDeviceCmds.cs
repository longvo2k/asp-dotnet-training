using Quartz;

namespace StudyDotnet.Services.ScheduledTasks.Jobs;

[DisallowConcurrentExecution]
public sealed class JobCleanupDeviceCmds : IJob
{
    private readonly ILogger<JobCleanupDeviceCmds> _logger;

    public JobCleanupDeviceCmds(ILogger<JobCleanupDeviceCmds> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Study job: clean old processed device commands at {RunAt}.", DateTimeOffset.Now);
        return Task.CompletedTask;
    }
}

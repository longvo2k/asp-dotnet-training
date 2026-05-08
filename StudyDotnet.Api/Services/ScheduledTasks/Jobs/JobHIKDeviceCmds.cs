using Quartz;

namespace StudyDotnet.Services.ScheduledTasks.Jobs;

[DisallowConcurrentExecution]
public sealed class JobHIKDeviceCmds : IJob
{
    private readonly ILogger<JobHIKDeviceCmds> _logger;

    public JobHIKDeviceCmds(ILogger<JobHIKDeviceCmds> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Study job: send queued commands to HIK devices at {RunAt}.", DateTimeOffset.Now);
        return Task.CompletedTask;
    }
}

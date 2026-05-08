using Quartz;

namespace StudyDotnet.Services.ScheduledTasks.Jobs;

[DisallowConcurrentExecution]
public sealed class JobPullEventHIKDevice : IJob
{
    private readonly ILogger<JobPullEventHIKDevice> _logger;

    public JobPullEventHIKDevice(ILogger<JobPullEventHIKDevice> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Study job: sync access events from HIK devices at {RunAt}.", DateTimeOffset.Now);
        return Task.CompletedTask;
    }
}

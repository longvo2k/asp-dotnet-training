using Quartz;

namespace StudyDotnet.Services.ScheduledTasks.Jobs;

[DisallowConcurrentExecution]
public sealed class JobPullUserHIKDevice : IJob
{
    private readonly ILogger<JobPullUserHIKDevice> _logger;

    public JobPullUserHIKDevice(ILogger<JobPullUserHIKDevice> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Study job: sync users from HIK devices at {RunAt}.", DateTimeOffset.Now);
        return Task.CompletedTask;
    }
}

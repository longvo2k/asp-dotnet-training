using StudyDotnet.Domain.Entities;

namespace StudyDotnet.Domain.Learning;

public sealed class DeviceAccessLevel
{
    public Guid DeviceId { get; set; }
    public Device? Device { get; set; }
    public Guid AccessLevelId { get; set; }
    public AccessLevel? AccessLevel { get; set; }
}

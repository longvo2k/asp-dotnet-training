namespace StudyDotnet.Domain.Learning;

public sealed class AccessLevel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<UserAccessLevel> UserAccessLevels { get; set; } = new();
    public List<DeviceAccessLevel> DeviceAccessLevels { get; set; } = new();
}

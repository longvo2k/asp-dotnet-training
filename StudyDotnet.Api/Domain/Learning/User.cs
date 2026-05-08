namespace StudyDotnet.Domain.Learning;

public sealed class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public List<UserAccessLevel> UserAccessLevels { get; set; } = new();
}

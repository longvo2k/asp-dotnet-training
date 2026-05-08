namespace StudyDotnet.Domain.Learning;

public sealed class UserAccessLevel
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid AccessLevelId { get; set; }
    public AccessLevel? AccessLevel { get; set; }
}

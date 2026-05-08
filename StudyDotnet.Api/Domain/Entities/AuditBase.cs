namespace StudyDotnet.Domain.Entities;

public abstract class AuditBase<TKey> : IEntity<TKey>, IAudit
{
    public TKey Id { get; protected set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = "seed";
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}

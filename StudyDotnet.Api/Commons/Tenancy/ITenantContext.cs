namespace StudyDotnet.Commons.Tenancy;

public interface ITenantContext
{
    string? TenantId { get; }
    bool HasTenant { get; }
    void SetTenant(string tenantId);
}

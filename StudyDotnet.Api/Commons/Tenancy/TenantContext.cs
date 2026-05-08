namespace StudyDotnet.Commons.Tenancy;

public sealed class TenantContext : ITenantContext
{
    public string? TenantId { get; private set; }
    public bool HasTenant => !string.IsNullOrWhiteSpace(TenantId);

    public void SetTenant(string tenantId)
    {
        TenantId = tenantId.Trim().ToLowerInvariant();
    }
}

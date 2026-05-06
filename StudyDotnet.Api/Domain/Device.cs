namespace StudyDotnet.Api.Domain;

public enum DeviceSupplier
{
    ZKTeco,
    HIKvision
}

public sealed class Device : AuditBase<Guid>
{
    private Device()
    {
    }

    public Device(Guid id, string tenantId, Guid companyId, string name, DeviceSupplier supplier, bool isOnline)
    {
        Id = id;
        TenantId = tenantId;
        CompanyId = companyId;
        Name = name;
        Supplier = supplier;
        IsOnline = isOnline;
    }

    public string TenantId { get; set; } = string.Empty;
    public Guid CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DeviceSupplier Supplier { get; set; }
    public bool IsOnline { get; set; }
    public Company? Company { get; set; }
}

namespace StudyDotnet.Domain.Entities;

public sealed class Company : AuditBase<Guid>
{
    private Company()
    {
    }

    public Company(Guid id, string tenantId, string name, string taxCode)
    {
        Id = id;
        TenantId = tenantId;
        Name = name;
        TaxCode = taxCode;
    }

    public string TenantId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string TaxCode { get; set; } = string.Empty;
    public List<Device> Devices { get; set; } = new();
}

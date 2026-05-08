using Microsoft.EntityFrameworkCore;
using StudyDotnet.Domain.Entities;

namespace StudyDotnet.Data.EF;

public sealed class StudyDbContext : DbContext
{
    public StudyDbContext(DbContextOptions<StudyDbContext> options)
        : base(options)
    {
    }

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Device> Devices => Set<Device>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(builder =>
        {
            builder.ToTable("Companies");
            builder.HasKey(company => company.Id);
            builder.Property(company => company.TenantId).HasMaxLength(64).IsRequired();
            builder.Property(company => company.Name).HasMaxLength(200).IsRequired();
            builder.Property(company => company.TaxCode).HasMaxLength(50).IsRequired();
            builder.HasIndex(company => new { company.TenantId, company.Name });
        });

        modelBuilder.Entity<Device>(builder =>
        {
            builder.ToTable("Devices");
            builder.HasKey(device => device.Id);
            builder.Property(device => device.TenantId).HasMaxLength(64).IsRequired();
            builder.Property(device => device.Name).HasMaxLength(200).IsRequired();
            builder.Property(device => device.Supplier).HasConversion<string>().HasMaxLength(50);
            builder.HasIndex(device => new { device.TenantId, device.Supplier });
            builder
                .HasOne(device => device.Company)
                .WithMany(company => company.Devices)
                .HasForeignKey(device => device.CompanyId);
        });

        SeedData(modelBuilder);
    }

    public Task<int> SaveChangesAsync(string userName, CancellationToken cancellationToken = default)
    {
        FillAuditFields(userName);
        return base.SaveChangesAsync(cancellationToken);
    }

    private void FillAuditFields(string userName)
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<IAudit>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.CreatedBy = userName;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
                entry.Entity.UpdatedBy = userName;
            }
        }
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var companyAId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var companyBId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var seedTime = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Company>().HasData(
            new
            {
                Id = companyAId,
                TenantId = "demo",
                Name = "XiD Demo",
                TaxCode = "XID-001",
                CreatedAt = seedTime,
                CreatedBy = "seed",
                UpdatedAt = (DateTime?)null,
                UpdatedBy = (string?)null
            },
            new
            {
                Id = companyBId,
                TenantId = "lab",
                Name = "Study Lab",
                TaxCode = "LAB-002",
                CreatedAt = seedTime,
                CreatedBy = "seed",
                UpdatedAt = (DateTime?)null,
                UpdatedBy = (string?)null
            });

        modelBuilder.Entity<Device>().HasData(
            new
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                TenantId = "demo",
                CompanyId = companyAId,
                Name = "Front Door ZK",
                Supplier = DeviceSupplier.ZKTeco,
                IsOnline = true,
                CreatedAt = seedTime,
                CreatedBy = "seed",
                UpdatedAt = (DateTime?)null,
                UpdatedBy = (string?)null
            },
            new
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                TenantId = "demo",
                CompanyId = companyAId,
                Name = "Warehouse HIK",
                Supplier = DeviceSupplier.HIKvision,
                IsOnline = false,
                CreatedAt = seedTime,
                CreatedBy = "seed",
                UpdatedAt = (DateTime?)null,
                UpdatedBy = (string?)null
            },
            new
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                TenantId = "lab",
                CompanyId = companyBId,
                Name = "Office HIK",
                Supplier = DeviceSupplier.HIKvision,
                IsOnline = true,
                CreatedAt = seedTime,
                CreatedBy = "seed",
                UpdatedAt = (DateTime?)null,
                UpdatedBy = (string?)null
            });
    }
}

using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<DemoItem> DemoItems { get; set; }
    public DbSet<Voucher> Vouchers { get; set; }
    public DbSet<ProcessedBlobRecord> ProcessedBlobRecords { get; set; }
    public DbSet<ApplicationType> ApplicationTypes { get; set; }
    public DbSet<ApplicationTypeField> ApplicationTypeFields { get; set; }
    public DbSet<CsvColumnMapping> CsvColumnMappings { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DemoItem>(e =>
        {
            e.ToTable("DemoItems");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.Property(x => x.Description).HasMaxLength(2000);
        });

        modelBuilder.Entity<Voucher>(e =>
        {
            e.ToTable("Vouchers");
            e.HasKey(x => x.Id);
            e.Property(x => x.Summary).IsRequired().HasMaxLength(500);
            e.Property(x => x.DebitAccount).HasMaxLength(100);
            e.Property(x => x.CreditAccount).HasMaxLength(100);
            e.Property(x => x.SourceBlobPath).HasMaxLength(500);
        });

        modelBuilder.Entity<ProcessedBlobRecord>(e =>
        {
            e.ToTable("ProcessedBlobRecords");
            e.HasKey(x => x.Id);
            e.Property(x => x.ContainerName).IsRequired().HasMaxLength(200);
            e.Property(x => x.BlobName).IsRequired().HasMaxLength(500);
            e.Property(x => x.BlobETag).HasMaxLength(100);
            e.Property(x => x.Status).IsRequired().HasMaxLength(20);
            e.Property(x => x.ErrorMessage).HasMaxLength(2000);
            e.HasIndex(x => new { x.ContainerName, x.BlobName }).IsUnique();
        });

        modelBuilder.Entity<ApplicationType>(e =>
        {
            e.ToTable("ApplicationTypes");
            e.HasKey(x => x.Id);
            e.Property(x => x.Code).IsRequired().HasMaxLength(50);
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.Property(x => x.Description).HasMaxLength(500);
            e.HasIndex(x => x.Code).IsUnique();
        });

        modelBuilder.Entity<ApplicationTypeField>(e =>
        {
            e.ToTable("ApplicationTypeFields");
            e.HasKey(x => x.Id);
            e.Property(x => x.FieldCode).IsRequired().HasMaxLength(100);
            e.Property(x => x.FieldName).IsRequired().HasMaxLength(200);
            e.Property(x => x.DataType).IsRequired().HasMaxLength(50);
            e.HasIndex(x => new { x.ApplicationTypeId, x.FieldCode }).IsUnique();
        });

        modelBuilder.Entity<CsvColumnMapping>(e =>
        {
            e.ToTable("CsvColumnMappings");
            e.HasKey(x => x.Id);
            e.Property(x => x.CsvColumnName).HasMaxLength(200);
            e.Property(x => x.TargetFieldCode).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Department>(e =>
        {
            e.ToTable("Departments");
            e.HasKey(x => x.Id);
            e.Property(x => x.Code).IsRequired().HasMaxLength(50);
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.HasIndex(x => x.Code).IsUnique();
        });

        modelBuilder.Entity<UserRole>(e =>
        {
            e.ToTable("UserRoles");
            e.HasKey(x => x.Id);
            e.Property(x => x.EntraObjectId).IsRequired().HasMaxLength(64);
            e.Property(x => x.Upn).IsRequired().HasMaxLength(256);
            e.Property(x => x.RoleCode).IsRequired().HasMaxLength(50);
            e.HasIndex(x => x.EntraObjectId);
            e.HasOne(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}

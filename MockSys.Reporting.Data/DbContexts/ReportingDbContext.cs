using Microsoft.EntityFrameworkCore;
using MockSys.Reporting.Data.Entities;

namespace MockSys.Reporting.Data.DbContexts;

public class ReportingDbContext(DbContextOptions<ReportingDbContext> options) : DbContext(options)
{
    public DbSet<SyncLog> SyncLogs { get; set; }
    public DbSet<SyncTransaction> SyncTransactions { get; set; }
    public DbSet<MetricResult> MetricResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SyncLog>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<SyncTransaction>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<MetricResult>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => new { e.MetricName, e.Date })
              .HasDatabaseName("ix_metric_name_date");

            entity.Property(m => m.Result)
              .HasColumnType("jsonb")
              .IsRequired();
        });
    }
}

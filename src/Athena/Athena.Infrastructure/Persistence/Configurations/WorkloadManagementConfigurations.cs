using Athena.Domain.Block.Registry.Entities;
using Athena.Domain.Block.Registry.ValueObjects;
using Athena.Domain.Workload.Management.Entities;
using Athena.Domain.Workload.Registry.Entities;
using Athena.Domain.Workload.Registry.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athena.Infrastructure.Persistence.Configurations;

public sealed class WorkloadLifecycleConfiguration : IEntityTypeConfiguration<WorkloadLifecycle>
{
    public void Configure(EntityTypeBuilder<WorkloadLifecycle> builder)
    {
        builder.ToTable("workload_lifecycles");
        builder.Property<int>("workload_database_id").HasColumnName("workload_database_id").ValueGeneratedNever();
        builder.HasKey("workload_database_id").HasName("pk_workload_lifecycles");
        builder.Property(x => x.Id).HasConversion(id => id.Value, value => WorkloadId.Create(value)).HasColumnName("workload_id").IsRequired();
        builder.HasOne<Workload>().WithOne().HasPrincipalKey<Workload>("id", nameof(Workload.Id))
            .HasForeignKey<WorkloadLifecycle>("workload_database_id", nameof(WorkloadLifecycle.Id)).OnDelete(DeleteBehavior.Restrict);
        builder.Property(x => x.DesiredState).HasConversion<int>().HasColumnName("desired_state").IsRequired();
        builder.Property(x => x.OperationalStatus).HasConversion<int>().HasColumnName("operational_status").IsRequired();
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted").IsRequired();
    }
}

public sealed class WorkloadOperationConfiguration : IEntityTypeConfiguration<WorkloadOperation>
{
    public void Configure(EntityTypeBuilder<WorkloadOperation> builder)
    {
        builder.ToTable("workload_operations");
        builder.HasKey(x => x.Id).HasName("pk_workload_operations");
        builder.Property(x => x.Id).HasColumnName("operation_id");
        builder.Property<int>("workload_database_id").HasColumnName("workload_database_id").IsRequired();
        builder.Property(x => x.WorkloadId).HasConversion(id => id.Value, value => WorkloadId.Create(value)).HasColumnName("workload_id").IsRequired();
        builder.HasOne<Workload>().WithMany().HasPrincipalKey("id", nameof(Workload.Id))
            .HasForeignKey("workload_database_id", nameof(WorkloadOperation.WorkloadId)).OnDelete(DeleteBehavior.Restrict);
        builder.Property(x => x.Type).HasConversion<int>().HasColumnName("operation_type").IsRequired();
        builder.Property(x => x.Status).HasConversion<int>().HasColumnName("operation_status").IsRequired();
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(x => x.StartedAtUtc).HasColumnName("started_at_utc");
        builder.Property(x => x.CompletedAtUtc).HasColumnName("completed_at_utc");
        builder.Property(x => x.ErrorMessage).HasColumnName("error_message").HasMaxLength(4000);
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted").IsRequired();
        builder.HasIndex("workload_database_id", nameof(WorkloadOperation.CreatedAtUtc)).HasDatabaseName("ix_workload_operations_workload_created");
    }
}

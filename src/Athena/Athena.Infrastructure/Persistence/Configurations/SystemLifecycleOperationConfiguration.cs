using Athena.Domain.Orchestration.Management.Entities;
using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Registry.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RegisteredSystem = Athena.Domain.System.Registry.Entities.System;

namespace Athena.Infrastructure.Persistence.Configurations;

public class SystemLifecycleOperationConfiguration : IEntityTypeConfiguration<SystemLifecycleOperation>
{
    public void Configure(EntityTypeBuilder<SystemLifecycleOperation> builder)
    {
        builder.ToTable("system_lifecycle_operations");
        builder.HasKey(x => x.Id).HasName("pk_system_lifecycle_operations");
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
        builder.Property<int>("system_database_id").HasColumnName("system_database_id").IsRequired();
        builder.Property(x => x.SystemId).HasConversion(id => id.Value, value => SystemId.Create(value)).HasColumnName("system_id").IsRequired();
        builder.HasOne<RegisteredSystem>().WithMany().HasForeignKey("system_database_id", nameof(SystemLifecycleOperation.SystemId))
               .HasPrincipalKey("id", nameof(RegisteredSystem.Id)).OnDelete(DeleteBehavior.Restrict);
        builder.Property(x => x.OrchestrationLifecycleOperationId).HasColumnName("orchestration_lifecycle_operation_id");
        builder.HasOne<OrchestrationLifecycleOperation>().WithMany().HasForeignKey(nameof(SystemLifecycleOperation.OrchestrationLifecycleOperationId))
               .OnDelete(DeleteBehavior.Restrict);
        builder.Property(x => x.Type).HasConversion<int>().HasColumnName("type").IsRequired();
        builder.Property(x => x.Status).HasConversion<int>().HasColumnName("status").IsRequired();
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(x => x.StartedAtUtc).HasColumnName("started_at_utc");
        builder.Property(x => x.CompletedAtUtc).HasColumnName("completed_at_utc");
        builder.Property(x => x.ErrorMessage).HasColumnName("error_message");
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted").IsRequired();
        builder.HasIndex("system_database_id", nameof(SystemLifecycleOperation.CreatedAtUtc)).HasDatabaseName("ix_system_lifecycle_operations_system_created");
        builder.HasIndex(nameof(SystemLifecycleOperation.OrchestrationLifecycleOperationId)).HasDatabaseName("ix_system_lifecycle_operations_parent_operation");
    }
}
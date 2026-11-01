using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Management.Enums;
using Athena.Domain.System.Registry.Entities;
using Athena.Domain.System.Registry.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athena.Infrastructure.Persistence.Configurations;

public sealed class SystemInstanceOperationConfiguration : IEntityTypeConfiguration<SystemInstanceOperation>
{
    public void Configure(EntityTypeBuilder<SystemInstanceOperation> builder)
    {
        builder.ToTable("system_instance_operations");
        builder.HasKey(operation => operation.Id).HasName("pk_system_instance_operations");
        builder.Property(operation => operation.Id).HasColumnName("operation_id").IsRequired();
        builder.Property(operation => operation.SystemInstanceId)
            .HasConversion(id => id.Value, value => SystemInstanceId.Create(value))
            .HasColumnName("system_instance_id")
            .IsRequired();
        builder.Property<int>("system_instance_database_id").HasColumnName("system_instance_database_id").IsRequired();
        builder.HasOne<SystemInstance>()
            .WithMany()
            .HasForeignKey("system_instance_database_id", nameof(SystemInstanceOperation.SystemInstanceId))
            .HasPrincipalKey("id", nameof(SystemInstance.Id))
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(operation => operation.Type).HasConversion<int>().HasColumnName("operation_type").IsRequired();
        builder.Property(operation => operation.Status).HasConversion<int>().HasColumnName("operation_status").IsRequired();
        builder.Property(operation => operation.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(operation => operation.StartedAtUtc).HasColumnName("started_at_utc");
        builder.Property(operation => operation.CompletedAtUtc).HasColumnName("completed_at_utc");
        builder.Property(operation => operation.ErrorMessage).HasColumnName("error_message").HasMaxLength(4000);
        builder.HasIndex("system_instance_database_id", nameof(SystemInstanceOperation.SystemInstanceId), nameof(SystemInstanceOperation.CreatedAtUtc))
            .HasDatabaseName("ix_system_instance_operations_instance_created");
    }
}

using Athena.Domain.System.Registry.Entities;
using Athena.Domain.System.Registry.Enums;
using Athena.Domain.System.Registry.ValueObjects;
using Athena.Domain.Workload.Registry.Entities;
using Athena.Domain.Workload.Registry.ValueObjects;
using RegisteredSystem = Athena.Domain.System.Registry.Entities.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athena.Infrastructure.Persistence.Configurations;

public sealed class SystemInstanceConfiguration : IEntityTypeConfiguration<SystemInstance>
{
    public void Configure(EntityTypeBuilder<SystemInstance> builder)
    {
        builder.ToTable("system_instances");
        // Technical primary key.
        builder.Property<int>("id").UseIdentityByDefaultColumn();
        builder.HasKey("id").HasName("pk_system_instances");
        // Domain identifier.
        builder.Property(instance => instance.Id).HasConversion(id => id.Value, value => SystemInstanceId.Create(value)).HasColumnName("system_instance_id").IsRequired();
        builder.HasIndex(instance => instance.Id).IsUnique().HasDatabaseName("ux_system_instances_system_instance_id");
        builder.HasAlternateKey("id", nameof(SystemInstance.Id)).HasName("ak_system_instances_id_system_instance_id");
        // System relationship.
        builder.Property(instance => instance.SystemId).HasConversion(id => id.Value, value => SystemId.Create(value)).HasColumnName("system_id").IsRequired();
        builder.Property<int>("system_database_id").HasColumnName("system_database_id").IsRequired();
        builder.HasOne<RegisteredSystem>().WithMany().HasPrincipalKey("id", nameof(RegisteredSystem.Id)).HasForeignKey("system_database_id", nameof(SystemInstance.SystemId)).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("system_database_id", nameof(SystemInstance.SystemId)).HasDatabaseName("ix_system_instances_system_database_id_system_id");
        // Workload relationship.
        builder.Property(instance => instance.WorkloadId).HasConversion(id => id.Value, value => WorkloadId.Create(value)).HasColumnName("workload_id").IsRequired();
        builder.Property<int>("workload_database_id").HasColumnName("workload_database_id").IsRequired();
        builder.HasOne<Workload>().WithMany().HasPrincipalKey("id", nameof(Workload.Id)).HasForeignKey("workload_database_id", nameof(SystemInstance.WorkloadId)).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("workload_database_id", nameof(SystemInstance.WorkloadId)).HasDatabaseName("ix_system_instances_workload_database_id_workload_id");
        // Name: unique within the same Workload.
        builder.Property(instance => instance.Name).HasConversion(name => name.Value, value => SystemInstanceName.Create(value)).HasColumnName("name").HasMaxLength(SystemInstanceName.MaxLength).IsRequired();
        builder.HasIndex("workload_database_id", nameof(SystemInstance.WorkloadId), nameof(SystemInstance.Name)).IsUnique().HasDatabaseName("ux_system_instances_workload_name");
        // Registration status.
        builder.Property(instance => instance.RegistrationStatus).HasConversion<int>().HasColumnName("registration_status").IsRequired();
        // Availability status.
        builder.Property(instance => instance.AvailabilityStatus).HasConversion<int>().HasColumnName("availability_status").HasDefaultValue(SystemInstanceAvailabilityStatus.Unknown).IsRequired();
        // Audit fields.
        builder.Property(instance => instance.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(instance => instance.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(instance => instance.IsDeleted).HasColumnName("is_deleted").IsRequired();
    }
}
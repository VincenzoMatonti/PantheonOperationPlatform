using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Athena.Domain.SystemRegistry.ValueObjects;
using RuntimeInstance = Athena.Infrastructure.Persistence.Runtime.SystemRuntimeInstance;
using RegisteredSystem = Athena.Domain.SystemRegistry.Entities.System;

namespace Athena.Infrastructure.Persistence.Configurations;

public class SystemRuntimeInstanceConfiguration : IEntityTypeConfiguration<RuntimeInstance>
{
    public void Configure(EntityTypeBuilder<RuntimeInstance> builder)
    {
        builder.ToTable("system_runtime_instances");
        builder.HasKey(x => x.Id).HasName("pk_system_runtime_instances");
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
        builder.Property<int>("system_database_id").HasColumnName("system_database_id").IsRequired();
        builder.Property(x => x.SystemId).HasConversion(id => id.Value, value => SystemId.Create(value)).HasColumnName("system_id").IsRequired();
        builder.HasOne<RegisteredSystem>().WithMany().HasForeignKey("system_database_id", nameof(RuntimeInstance.SystemId))
            .HasPrincipalKey("id", nameof(RegisteredSystem.Id)).OnDelete(DeleteBehavior.Restrict);
        builder.Property(x => x.RuntimeProvider).HasColumnName("runtime_provider").HasMaxLength(50).IsRequired();
        builder.Property(x => x.RuntimeResourceId).HasColumnName("runtime_resource_id").HasMaxLength(255).IsRequired();
        builder.Property(x => x.RuntimeResourceName).HasColumnName("runtime_resource_name").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Environment).HasColumnName("environment").HasMaxLength(100).IsRequired();
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc").IsRequired();
        builder.HasIndex("system_database_id", nameof(RuntimeInstance.Environment)).HasDatabaseName("ix_system_runtime_instances_system_environment");
        builder.HasIndex(nameof(RuntimeInstance.RuntimeProvider), nameof(RuntimeInstance.Environment), nameof(RuntimeInstance.RuntimeResourceId))
            .IsUnique().HasDatabaseName("ux_system_runtime_instances_resource");
    }
}
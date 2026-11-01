using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Management.Enums;
using Athena.Domain.System.Registry.Entities;
using Athena.Domain.System.Registry.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athena.Infrastructure.Persistence.Configurations;

public sealed class SystemInstanceLifecycleConfiguration : IEntityTypeConfiguration<SystemInstanceLifecycle>
{
    public void Configure(EntityTypeBuilder<SystemInstanceLifecycle> builder)
    {
        builder.ToTable("system_instance_lifecycles");
        builder.Property<int>("system_instance_database_id").HasColumnName("system_instance_database_id").ValueGeneratedNever();
        builder.HasKey("system_instance_database_id").HasName("pk_system_instance_lifecycles");
        builder.Property(lifecycle => lifecycle.Id)
            .HasConversion(id => id.Value, value => SystemInstanceId.Create(value))
            .HasColumnName("system_instance_id")
            .IsRequired();
        builder.HasOne<SystemInstance>()
            .WithOne()
            .HasPrincipalKey<SystemInstance>("id", nameof(SystemInstance.Id))
            .HasForeignKey<SystemInstanceLifecycle>("system_instance_database_id", nameof(SystemInstanceLifecycle.Id))
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(lifecycle => lifecycle.DesiredState).HasConversion<int>().HasColumnName("desired_state").IsRequired();
        builder.Property(lifecycle => lifecycle.OperationalStatus).HasConversion<int>().HasColumnName("operational_status").IsRequired();
        builder.Property(lifecycle => lifecycle.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(lifecycle => lifecycle.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(lifecycle => lifecycle.ObservedAtUtc).HasColumnName("observed_at_utc");
        builder.Property(lifecycle => lifecycle.IsDeleted).HasColumnName("is_deleted").IsRequired();
    }
}

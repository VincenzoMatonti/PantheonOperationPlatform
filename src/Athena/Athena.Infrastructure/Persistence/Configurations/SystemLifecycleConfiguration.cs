using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Registry.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RegisteredSystem = Athena.Domain.System.Registry.Entities.System;

namespace Athena.Infrastructure.Persistence.Configurations;

public class SystemLifecycleConfiguration : IEntityTypeConfiguration<SystemLifecycle>
{
    public void Configure(EntityTypeBuilder<SystemLifecycle> builder)
    {
        builder.ToTable("system_lifecycles");
        builder.Property<int>("system_database_id").HasColumnName("system_database_id").ValueGeneratedNever();
        builder.HasKey("system_database_id").HasName("pk_system_lifecycles");
        builder.Property(x => x.SystemId).HasConversion(id => id.Value, value => SystemId.Create(value)).HasColumnName("system_id").IsRequired();
        builder.HasOne<RegisteredSystem>().WithOne().HasPrincipalKey<RegisteredSystem>("id", nameof(RegisteredSystem.Id))
               .HasForeignKey<SystemLifecycle>("system_database_id", nameof(SystemLifecycle.SystemId)).OnDelete(DeleteBehavior.Restrict);
        builder.Property(x => x.DesiredState).HasConversion<int>().HasColumnName("desired_state").IsRequired();
        builder.Property(x => x.OperationalStatus).HasConversion<int>().HasColumnName("operational_status").IsRequired();
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted").IsRequired();
    }
}

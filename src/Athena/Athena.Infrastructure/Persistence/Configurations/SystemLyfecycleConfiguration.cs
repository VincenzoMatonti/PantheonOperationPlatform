using Athena.Domain.SystemManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athena.Infrastructure.Persistence.Configurations;

public class SystemLifecycleConfiguration : IEntityTypeConfiguration<SystemLifecycle>
{
    public void Configure(EntityTypeBuilder<SystemLifecycle> builder)
    {
        builder.ToTable("system_lifecycles");

        // PK e FK numerica verso systems.id.
        // La proprietà è shadow: non viene aggiunta al dominio.
        builder.Property<int>("system_database_id")
            .HasColumnName("system_id")
            .ValueGeneratedNever();

        builder.HasKey("system_database_id");

        builder.HasOne<Domain.SystemRegistry.Entities.System>()
            .WithOne()
            .HasForeignKey<SystemLifecycle>("system_database_id")
            .OnDelete(DeleteBehavior.Restrict);

        // Il GUID resta nel dominio, ma non è la FK relazionale.
        builder.Ignore(lifecycle => lifecycle.SystemId);

        builder.Property(lifecycle => lifecycle.DesiredState)
            .HasConversion<int>()
            .HasColumnName("desired_state")
            .IsRequired();

        builder.Property(lifecycle => lifecycle.OperationalStatus)
            .HasConversion<int>()
            .HasColumnName("operational_status")
            .IsRequired();

        builder.Property(lifecycle => lifecycle.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(lifecycle => lifecycle.UpdatedAtUtc)
            .HasColumnName("updated_at_utc");

        builder.Property(lifecycle => lifecycle.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired();
    }
}

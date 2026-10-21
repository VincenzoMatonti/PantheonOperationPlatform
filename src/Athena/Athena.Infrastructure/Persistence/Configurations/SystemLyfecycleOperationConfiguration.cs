using Athena.Domain.SystemManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athena.Infrastructure.Persistence.Configurations;

public class SystemLifecycleOperationConfiguration : IEntityTypeConfiguration<SystemLifecycleOperation>
{
    public void Configure(EntityTypeBuilder<SystemLifecycleOperation> builder)
    {
        builder.ToTable("system_lifecycle_operations");
        builder.HasKey(operation => operation.Id);
        builder.Property(operation => operation.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        // FK numerica verso systems.id, gestita da EF Core.
        builder.Property<int>("system_database_id")
            .HasColumnName("system_id")
            .IsRequired();

        builder.HasOne<Domain.SystemRegistry.Entities.System>()
            .WithMany()
            .HasForeignKey("system_database_id")
            .OnDelete(DeleteBehavior.Restrict);

        // Il GUID del sistema rimane un concetto del dominio.
        builder.Ignore(operation => operation.SystemId);

        builder.Property(operation => operation.Type)
            .HasConversion<int>()
            .HasColumnName("type")
            .IsRequired();

        builder.Property(operation => operation.Status)
            .HasConversion<int>()
            .HasColumnName("status")
            .IsRequired();

        builder.Property(operation => operation.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(operation => operation.UpdatedAtUtc)
            .HasColumnName("updated_at_utc");

        builder.Property(operation => operation.StartedAtUtc)
            .HasColumnName("started_at_utc");

        builder.Property(operation => operation.CompletedAtUtc)
            .HasColumnName("completed_at_utc");

        builder.Property(operation => operation.ErrorMessage)
            .HasColumnName("error_message");

        builder.Property(operation => operation.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired();

        builder.HasIndex("system_database_id", nameof(SystemLifecycleOperation.CreatedAtUtc))
            .HasDatabaseName("ix_system_lifecycle_operations_system_created");
    }
}

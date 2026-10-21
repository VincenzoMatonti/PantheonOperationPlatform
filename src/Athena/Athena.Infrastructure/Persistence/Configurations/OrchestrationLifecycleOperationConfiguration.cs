using Athena.Domain.OrchestrationManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athena.Infrastructure.Persistence.Configurations;

public class OrchestrationLifecycleOperationConfiguration : IEntityTypeConfiguration<OrchestrationLifecycleOperation>
{
    public void Configure(EntityTypeBuilder<OrchestrationLifecycleOperation> builder)
    {
        builder.ToTable("orchestration_lifecycle_operations");
        builder.HasKey(operation => operation.Id);
        builder.Property(operation => operation.Id).HasColumnName("id").ValueGeneratedNever();

        // FK numerica verso orchestrations.id.
        builder.Property<int>("orchestration_database_id")
            .HasColumnName("orchestration_id")
            .IsRequired();

        builder.HasOne<Domain.OrchestrationRegistry.Entities.Orchestration>()
            .WithMany()
            .HasForeignKey("orchestration_database_id")
            .OnDelete(DeleteBehavior.Restrict);

        // L'identificativo del dominio rimane un concetto del Domain.
        builder.Ignore(operation => operation.OrchestrationId);
        builder.Property(operation => operation.Type).HasConversion<int>().HasColumnName("type").IsRequired();
        builder.Property(operation => operation.Status).HasConversion<int>().HasColumnName("status").IsRequired();
        builder.Property(operation => operation.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(operation => operation.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(operation => operation.StartedAtUtc).HasColumnName("started_at_utc");
        builder.Property(operation => operation.CompletedAtUtc).HasColumnName("completed_at_utc");
        builder.Property(operation => operation.ErrorMessage).HasColumnName("error_message");
        builder.Property(operation => operation.IsDeleted).HasColumnName("is_deleted").IsRequired();
        builder.HasIndex("orchestration_database_id", nameof(OrchestrationLifecycleOperation.CreatedAtUtc))
            .HasDatabaseName("ix_orchestration_lifecycle_operations_orchestration_created");
    }
}
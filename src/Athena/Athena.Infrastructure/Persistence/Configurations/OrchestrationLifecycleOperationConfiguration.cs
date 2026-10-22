using Athena.Domain.OrchestrationManagement.Entities;
using Athena.Domain.OrchestrationRegistry.Entities;
using Athena.Domain.OrchestrationRegistry.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athena.Infrastructure.Persistence.Configurations;

public class OrchestrationLifecycleOperationConfiguration : IEntityTypeConfiguration<OrchestrationLifecycleOperation>
{
    public void Configure(EntityTypeBuilder<OrchestrationLifecycleOperation> builder)
    {
        builder.ToTable("orchestration_lifecycle_operations");
        builder.HasKey(x => x.Id).HasName("pk_orchestration_lifecycle_operations");
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
        builder.Property<int>("orchestration_database_id").HasColumnName("orchestration_database_id").IsRequired();
        builder.Property(x => x.OrchestrationId).HasConversion(id => id.Value, value => OrchestrationId.Create(value)).HasColumnName("orchestration_id").IsRequired();
        builder.HasOne<Orchestration>().WithMany().HasForeignKey("orchestration_database_id", nameof(OrchestrationLifecycleOperation.OrchestrationId))
               .HasPrincipalKey("id", nameof(Orchestration.Id)).OnDelete(DeleteBehavior.Restrict);
        builder.Property(x => x.Type).HasConversion<int>().HasColumnName("type").IsRequired();
        builder.Property(x => x.Status).HasConversion<int>().HasColumnName("status").IsRequired();
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(x => x.StartedAtUtc).HasColumnName("started_at_utc");
        builder.Property(x => x.CompletedAtUtc).HasColumnName("completed_at_utc");
        builder.Property(x => x.ErrorMessage).HasColumnName("error_message");
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted").IsRequired();
        builder.HasIndex("orchestration_database_id", nameof(OrchestrationLifecycleOperation.CreatedAtUtc))
               .HasDatabaseName("ix_orchestration_lifecycle_operations_orchestration_created");
    }
}
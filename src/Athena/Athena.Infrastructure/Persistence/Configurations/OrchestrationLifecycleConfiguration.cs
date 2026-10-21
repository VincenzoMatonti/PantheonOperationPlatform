using Athena.Domain.OrchestrationManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athena.Infrastructure.Persistence.Configurations;

public class OrchestrationLifecycleConfiguration : IEntityTypeConfiguration<OrchestrationLifecycle>
{
    public void Configure(EntityTypeBuilder<OrchestrationLifecycle> builder)
    {
        builder.ToTable("orchestration_lifecycles");

        // Chiave numerica interna e FK verso orchestrations.id.
        // La proprietà è shadow: non viene aggiunta al dominio.
        builder.Property<int>("orchestration_database_id").HasColumnName("orchestration_id").ValueGeneratedNever();
        builder.HasKey("orchestration_database_id");
        builder.HasOne<Domain.OrchestrationRegistry.Entities.Orchestration>()
           .WithOne().HasForeignKey<OrchestrationLifecycle>("orchestration_database_id").OnDelete(DeleteBehavior.Restrict);

        // L'identificativo di dominio non è la FK relazionale.
        builder.Ignore(lifecycle => lifecycle.Id);
        builder.Property(lifecycle => lifecycle.DesiredState).HasConversion<int>().HasColumnName("desired_state").IsRequired();
        builder.Property(lifecycle => lifecycle.OperationalStatus).HasConversion<int>().HasColumnName("operational_status").IsRequired();
        builder.Property(lifecycle => lifecycle.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(lifecycle => lifecycle.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(lifecycle => lifecycle.IsDeleted).HasColumnName("is_deleted").IsRequired();
    }
}
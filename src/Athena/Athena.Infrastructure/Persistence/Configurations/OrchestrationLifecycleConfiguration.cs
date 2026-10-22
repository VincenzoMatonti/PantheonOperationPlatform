using Athena.Domain.OrchestrationManagement.Entities;
using Athena.Domain.OrchestrationRegistry.Entities;
using Athena.Domain.OrchestrationRegistry.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athena.Infrastructure.Persistence.Configurations;

public class OrchestrationLifecycleConfiguration : IEntityTypeConfiguration<OrchestrationLifecycle>
{
    public void Configure(EntityTypeBuilder<OrchestrationLifecycle> builder)
    {
        builder.ToTable("orchestration_lifecycles");
        builder.Property<int>("orchestration_database_id").HasColumnName("orchestration_database_id").ValueGeneratedNever();
        builder.HasKey("orchestration_database_id").HasName("pk_orchestration_lifecycles");
        builder.Property(x => x.Id).HasConversion(id => id.Value, value => OrchestrationId.Create(value)).HasColumnName("orchestration_id").IsRequired();
        builder.HasOne<Orchestration>().WithOne().HasPrincipalKey<Orchestration>("id", nameof(Orchestration.Id))
               .HasForeignKey<OrchestrationLifecycle>("orchestration_database_id", nameof(OrchestrationLifecycle.Id)).OnDelete(DeleteBehavior.Restrict);
        builder.Property(x => x.DesiredState).HasConversion<int>().HasColumnName("desired_state").IsRequired();
        builder.Property(x => x.OperationalStatus).HasConversion<int>().HasColumnName("operational_status").IsRequired();
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted").IsRequired();
    }
}

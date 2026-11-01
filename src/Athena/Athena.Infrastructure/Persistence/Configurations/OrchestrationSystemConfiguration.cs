using Athena.Domain.Orchestration.Management.Entities;
using Athena.Domain.Orchestration.Management.ValueObjects;
using Athena.Domain.Orchestration.Registry.Entities;
using Athena.Domain.Orchestration.Registry.ValueObjects;
using Athena.Domain.System.Registry.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RegisteredSystemInstance = Athena.Domain.System.Registry.Entities.SystemInstance;

namespace Athena.Infrastructure.Persistence.Configurations;

public sealed class OrchestrationSystemInstanceConfiguration : IEntityTypeConfiguration<OrchestrationSystemInstance>
{
    public void Configure(EntityTypeBuilder<OrchestrationSystemInstance> builder)
    {
        builder.ToTable("orchestration_system_instances");

        // PK tecnica numerica: resta esclusivamente nel modello EF.
        builder.Property<int>("id").HasColumnName("id").UseIdentityByDefaultColumn();
        builder.HasKey("id").HasName("pk_orchestration_system_instances");

        // Identità GUID dell'associazione.
        builder.Property(x => x.Id).HasConversion(id => id.Value, value => OrchestrationSystemInstanceId.Create(value)).HasColumnName("association_id").IsRequired();
        builder.HasIndex(x => x.Id).IsUnique().HasDatabaseName("ux_orchestration_system_instances_association_id");

        // Orchestrazione: FK numerica + GUID.
        builder.Property<int>("orchestration_database_id").HasColumnName("orchestration_database_id").IsRequired();
        builder.Property(x => x.OrchestrationId).HasConversion(id => id.Value, value => OrchestrationId.Create(value)).HasColumnName("orchestration_id").IsRequired();
        builder.HasOne<Orchestration>()
            .WithMany()
            .HasForeignKey("orchestration_database_id", nameof(OrchestrationSystemInstance.OrchestrationId))
            .HasPrincipalKey("id", nameof(Orchestration.Id)).OnDelete(DeleteBehavior.Restrict);

        // SystemInstance: FK tecnica + identificativo di dominio.
        builder.Property<int>("system_instance_database_id").HasColumnName("system_instance_database_id").IsRequired();
        builder.Property(x => x.SystemInstanceId).HasConversion(id => id.Value, value => SystemInstanceId.Create(value)).HasColumnName("system_instance_id").IsRequired();
        builder.HasOne<RegisteredSystemInstance>()
            .WithMany()
            .HasForeignKey("system_instance_database_id", nameof(OrchestrationSystemInstance.SystemInstanceId))
            .HasPrincipalKey("id", nameof(RegisteredSystemInstance.Id)).OnDelete(DeleteBehavior.Restrict);
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted").IsRequired();

        // Ricerca delle associazioni di un'orchestrazione o di un sistema.
        builder.HasIndex("orchestration_database_id", "system_instance_database_id").HasDatabaseName("ix_orchestration_system_instances_database_pair");
        builder.HasIndex(nameof(OrchestrationSystemInstance.OrchestrationId), nameof(OrchestrationSystemInstance.SystemInstanceId)).HasDatabaseName("ix_orchestration_system_instances_guid_pair");
    }
}
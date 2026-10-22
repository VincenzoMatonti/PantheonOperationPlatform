using Athena.Domain.OrchestrationManagement.Entities;
using Athena.Domain.OrchestrationManagement.ValueObjects;
using Athena.Domain.OrchestrationRegistry.Entities;
using Athena.Domain.OrchestrationRegistry.ValueObjects;
using Athena.Domain.SystemRegistry.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RegisteredSystem = Athena.Domain.SystemRegistry.Entities.System;

namespace Athena.Infrastructure.Persistence.Configurations;

public sealed class OrchestrationSystemConfiguration : IEntityTypeConfiguration<OrchestrationSystem>
{
    public void Configure(EntityTypeBuilder<OrchestrationSystem> builder)
    {
        builder.ToTable("orchestration_systems");

        // PK tecnica numerica: resta esclusivamente nel modello EF.
        builder.Property<int>("id").HasColumnName("id").UseIdentityByDefaultColumn();
        builder.HasKey("id").HasName("pk_orchestration_systems");

        // Identità GUID dell'associazione.
        builder.Property(x => x.Id).HasConversion(id => id.Value, value => OrchestrationSystemId.Create(value)).HasColumnName("association_id").IsRequired();
        builder.HasIndex(x => x.Id).IsUnique().HasDatabaseName("ux_orchestration_systems_association_id");

        // Orchestrazione: FK numerica + GUID.
        builder.Property<int>("orchestration_database_id").HasColumnName("orchestration_database_id").IsRequired();
        builder.Property(x => x.OrchestrationId).HasConversion(id => id.Value, value => OrchestrationId.Create(value)).HasColumnName("orchestration_id").IsRequired();
        builder.HasOne<Orchestration>()
            .WithMany()
            .HasForeignKey("orchestration_database_id", nameof(OrchestrationSystem.OrchestrationId))
            .HasPrincipalKey("id", nameof(Orchestration.Id)).OnDelete(DeleteBehavior.Restrict);

        // Sistema: FK numerica + GUID.
        builder.Property<int>("system_database_id").HasColumnName("system_database_id").IsRequired();
        builder.Property(x => x.SystemId).HasConversion(id => id.Value, value => SystemId.Create(value)).HasColumnName("system_id").IsRequired();
        builder.HasOne<RegisteredSystem>()
            .WithMany()
            .HasForeignKey("system_database_id", nameof(OrchestrationSystem.SystemId))
            .HasPrincipalKey("id", nameof(RegisteredSystem.Id)).OnDelete(DeleteBehavior.Restrict);
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted").IsRequired();

        // Ricerca delle associazioni di un'orchestrazione o di un sistema.
        builder.HasIndex("orchestration_database_id", "system_database_id").HasDatabaseName("ix_orchestration_systems_database_pair");
        builder.HasIndex(nameof(OrchestrationSystem.OrchestrationId), nameof(OrchestrationSystem.SystemId)).HasDatabaseName("ix_orchestration_systems_guid_pair");
    }
}
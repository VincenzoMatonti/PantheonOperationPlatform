using Athena.Domain.Orchestration.Registry.Entities;
using Athena.Domain.Orchestration.Registry.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athena.Infrastructure.Persistence.Configurations;

public class OrchestrationConfiguration : IEntityTypeConfiguration<Orchestration>
{
    public void Configure(EntityTypeBuilder<Orchestration> builder)
    {
        builder.ToTable("orchestrations");

        // Chiave primaria numerica interna, generata dal database.
        builder.Property<int>("id").UseIdentityByDefaultColumn();
        builder.HasKey("id").HasName("pk_orchestrations");

        // Identificativo di dominio GUID.
        builder.Property(orchestration => orchestration.Id).HasConversion(id => id.Value, value => OrchestrationId.Create(value))
               .HasColumnName("orchestration_id").IsRequired();

        // Il GUID deve essere univoco indipendentemente dalla PK numerica.
        builder.HasIndex(orchestration => orchestration.Id).IsUnique().HasDatabaseName("ux_orchestrations_orchestration_id");

        // Coppia alternativa per le FK composite.
        builder.HasAlternateKey("id", nameof(Orchestration.Id)).HasName("ak_orchestrations_id_orchestration_id");
        builder.Property(orchestration => orchestration.Name).HasConversion(name => name.Value, value => OrchestrationName.Create(value))
               .HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.HasIndex(orchestration => orchestration.Name).IsUnique().HasDatabaseName("ux_orchestrations_name");
        builder.Property(orchestration => orchestration.RegistrationStatus).HasConversion<int>().HasColumnName("registration_status").IsRequired();
        builder.Property(orchestration => orchestration.AvailabilityStatus).HasConversion<int>().HasColumnName("availability_status").IsRequired();
        builder.Property(orchestration => orchestration.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(orchestration => orchestration.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(orchestration => orchestration.IsDeleted).HasColumnName("is_deleted").IsRequired();
    }
}

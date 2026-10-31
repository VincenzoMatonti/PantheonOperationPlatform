using Athena.Domain.System.Registry.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RegisteredSystem = Athena.Domain.System.Registry.Entities.System;

namespace Athena.Infrastructure.Persistence.Configurations;

public class SystemConfiguration : IEntityTypeConfiguration<RegisteredSystem>
{
    public void Configure(EntityTypeBuilder<RegisteredSystem> builder)
    {
        builder.ToTable("systems");

        // Chiave primaria numerica interna, generata dal database.
        builder.Property<int>("id").UseIdentityByDefaultColumn();
        builder.HasKey("id").HasName("pk_systems");

        // Identificativo di dominio GUID.
        builder.Property(system => system.Id).HasConversion(id => id.Value, value => SystemId.Create(value)).HasColumnName("system_id").IsRequired();

        // Il GUID deve essere univoco indipendentemente dalla PK numerica.
        builder.HasIndex(system => system.Id).IsUnique().HasDatabaseName("ux_systems_system_id");
        // Coppia alternativa per le FK composite:
        // la PK numerica e il GUID devono appartenere alla stessa riga.
        builder.HasAlternateKey("id", nameof(RegisteredSystem.Id)).HasName("ak_systems_id_system_id");
        builder.Property(system => system.Name).HasConversion(name => name.Value, value => SystemName.Create(value))
               .HasColumnName("name").HasMaxLength(SystemName.MaxLength).IsRequired();
        builder.HasIndex(system => system.Name).IsUnique().HasDatabaseName("ux_systems_name");
        builder.Property(system => system.RegistrationStatus).HasConversion<int>().HasColumnName("registration_status").IsRequired();
        builder.Property(system => system.AvailabilityStatus).HasConversion<int>().HasColumnName("availability_status").IsRequired();
        builder.Property(system => system.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(system => system.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(system => system.IsDeleted).HasColumnName("is_deleted").IsRequired();
        ConfigureEndpoints(builder);
        ConfigureCapabilities(builder);
    }

    private static void ConfigureEndpoints(EntityTypeBuilder<RegisteredSystem> builder)
    {
        builder.OwnsMany(system => system.Endpoints, endpoint =>
        {
            endpoint.ToTable("system_endpoints");
            endpoint.WithOwner().HasForeignKey("system_id");
            endpoint.Property<int>("id").UseIdentityByDefaultColumn();
            endpoint.HasKey("id");
            endpoint.Property(value => value.Name).HasColumnName("name").IsRequired();
            endpoint.Property(value => value.BaseAddress).HasConversion(address => address.ToString(), value => new Uri(value, UriKind.Absolute)).HasColumnName("base_address").IsRequired();
            endpoint.HasIndex("system_id", nameof(SystemEndpoint.Name)).IsUnique();
        });

        builder.Navigation(system => system.Endpoints).UsePropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureCapabilities(EntityTypeBuilder<RegisteredSystem> builder)
    {
        builder.OwnsMany(system => system.Capabilities, capability =>
        {
            capability.ToTable("system_capabilities");
            capability.WithOwner().HasForeignKey("system_id");
            capability.Property<int>("id").UseIdentityByDefaultColumn();
            capability.HasKey("id");
            capability.Property(value => value.Name).HasColumnName("name").IsRequired();
            capability.Property(value => value.Description).HasColumnName("description").IsRequired();
            capability.HasIndex("system_id", nameof(SystemCapability.Name)).IsUnique();
        });

        builder.Navigation(system => system.Capabilities).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

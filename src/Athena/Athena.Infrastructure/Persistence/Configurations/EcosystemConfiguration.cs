using Athena.Domain.Company.Registry.Entities;
using Athena.Domain.Company.Registry.ValueObjects;
using Athena.Domain.Ecosystem.Registry.Entities;
using Athena.Domain.Ecosystem.Registry.Enums;
using Athena.Domain.Ecosystem.Registry.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athena.Infrastructure.Persistence.Configurations;

public sealed class EcosystemConfiguration : IEntityTypeConfiguration<Ecosystem>
{
    public void Configure(EntityTypeBuilder<Ecosystem> builder)
    {
        builder.ToTable("ecosystems");
        // Internal database identifier.
        builder.Property<int>("id").UseIdentityByDefaultColumn();
        builder.HasKey("id").HasName("pk_ecosystems");
        // Domain identifier.
        builder.Property(ecosystem => ecosystem.Id).HasConversion(id => id.Value, value => EcosystemId.Create(value)).HasColumnName("ecosystem_id").IsRequired();
        builder.HasIndex(ecosystem => ecosystem.Id).IsUnique().HasDatabaseName("ux_ecosystems_ecosystem_id");
        builder.HasAlternateKey("id", nameof(Ecosystem.Id)).HasName("ak_ecosystems_id_ecosystem_id");
        // Mandatory Company association.
        builder.Property(ecosystem => ecosystem.CompanyId).HasConversion(id => id.Value, value => CompanyId.Create(value)).HasColumnName("company_id").IsRequired();
        builder.Property<int>("company_database_id").HasColumnName("company_database_id").IsRequired();
        builder.HasOne<Company>().WithMany().HasPrincipalKey("id", nameof(Company.Id)).HasForeignKey("company_database_id", nameof(Ecosystem.CompanyId)).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("company_database_id", nameof(Ecosystem.CompanyId)).HasDatabaseName("ix_ecosystems_company_database_id_company_id");
        // Ecosystem identity.
        builder.Property(ecosystem => ecosystem.Name).HasConversion(name => name.Value, value => EcosystemName.Create(value)).HasColumnName("name").HasMaxLength(EcosystemName.MaxLength).IsRequired();
        builder.HasIndex(ecosystem => ecosystem.Name).IsUnique().HasDatabaseName("ux_ecosystems_name");
        // Registration status and timestamps.
        builder.Property(ecosystem => ecosystem.RegistrationStatus).HasConversion<int>().HasColumnName("registration_status").IsRequired();
        builder.Property(ecosystem => ecosystem.AvailabilityStatus).HasConversion<int>().HasColumnName("availability_status").HasDefaultValue(EcosystemAvailabilityStatus.Unknown).IsRequired();
        builder.Property(ecosystem => ecosystem.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(ecosystem => ecosystem.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(ecosystem => ecosystem.IsDeleted).HasColumnName("is_deleted").IsRequired();
    }
}
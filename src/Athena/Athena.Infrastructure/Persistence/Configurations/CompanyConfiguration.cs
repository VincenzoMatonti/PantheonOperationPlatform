
using Athena.Domain.Company.Registry.Entities;
using Athena.Domain.Company.Registry.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athena.Infrastructure.Persistence.Configurations;

public sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("companies");
        // Internal database identifier.
        builder.Property<int>("id").UseIdentityByDefaultColumn();
        builder.HasKey("id").HasName("pk_companies");
        // Domain identifier.
        builder.Property(company => company.Id).HasConversion(id => id.Value, value => CompanyId.Create(value)).HasColumnName("company_id").IsRequired();
        builder.HasIndex(company => company.Id).IsUnique().HasDatabaseName("ux_companies_company_id");
        builder.HasAlternateKey("id", nameof(Company.Id)).HasName("ak_companies_id_company_id");
        // Optional Group association.
        builder.Property(company => company.GroupId).HasConversion(id => id == null ? (Guid?)null : id.Value, value => value.HasValue ? GroupId.Create(value.Value) : null).HasColumnName("group_id");
        builder.Property<int?>("group_database_id").HasColumnName("group_database_id");
        builder.HasOne<Group>().WithMany().HasPrincipalKey("id", nameof(Group.Id)).HasForeignKey("group_database_id", nameof(Company.GroupId)).OnDelete(DeleteBehavior.Restrict);
        // Company identity and legal information.
        builder.Property(company => company.Name).HasConversion(name => name.Value, value => CompanyName.Create(value)).HasColumnName("name").HasMaxLength(CompanyName.MaxLength).IsRequired();
        builder.HasIndex(company => company.Name).IsUnique().HasDatabaseName("ux_companies_name");
        builder.Property(company => company.LegalName).HasConversion(name => name.Value, value => LegalName.Create(value)).HasColumnName("legal_name").HasMaxLength(LegalName.MaxLength).IsRequired();
        builder.Property(company => company.CountryCode).HasConversion(code => code.Value, value => CountryCode.Create(value)).HasColumnName("country_code").HasMaxLength(2).IsRequired();
        // Optional fiscal information.
        builder.Property(company => company.TaxCode).HasConversion(code => code!.Value, value => TaxCode.Create(value)).HasColumnName("tax_code").HasMaxLength(TaxCode.MaxLength);
        builder.Property(company => company.VatNumber).HasConversion(number => number!.Value, value => VatNumber.Create(value)).HasColumnName("vat_number").HasMaxLength(VatNumber.MaxLength);
        builder.Property(company => company.RegistrationNumber).HasConversion(number => number!.Value, value => RegistrationNumber.Create(value)).HasColumnName("registration_number").HasMaxLength(RegistrationNumber.MaxLength);
        builder.Property(company => company.LegalForm).HasConversion(form => form!.Value, value => LegalForm.Create(value)).HasColumnName("legal_form").HasMaxLength(LegalForm.MaxLength);
        // Optional contact information.
        builder.Property(company => company.Email).HasConversion(email => email!.Value, value => EmailAddress.Create(value)).HasColumnName("email").HasMaxLength(EmailAddress.MaxLength);
        builder.Property(company => company.Pec).HasConversion(email => email!.Value, value => PecAddress.Create(value)).HasColumnName("pec").HasMaxLength(PecAddress.MaxLength);
        builder.Property(company => company.PhoneNumber).HasConversion(phone => phone!.Value, value => PhoneNumber.Create(value)).HasColumnName("phone_number").HasMaxLength(PhoneNumber.MaxLength);
        builder.Property(company => company.Website).HasConversion(website => website!.Value, value => WebsiteUrl.Create(value)).HasColumnName("website").HasMaxLength(WebsiteUrl.MaxLength);
        // Registered address: owned value object stored in companies.
        builder.OwnsOne(company => company.RegisteredAddress, address =>
        {
            address.Property(value => value.Street).HasColumnName("registered_address_street").HasMaxLength(PostalAddress.MaxStreetLength);
            address.Property(value => value.City).HasColumnName("registered_address_city").HasMaxLength(PostalAddress.MaxCityLength);
            address.Property(value => value.PostalCode).HasColumnName("registered_address_postal_code").HasMaxLength(PostalAddress.MaxPostalCodeLength);
            address.Property(value => value.Region).HasColumnName("registered_address_region").HasMaxLength(PostalAddress.MaxRegionLength);
            address.Property(value => value.CountryCode).HasConversion(code => code.Value, value => CountryCode.Create(value)).HasColumnName("registered_address_country_code").HasMaxLength(2).IsRequired(false);
        });
        builder.Navigation(company => company.RegisteredAddress).IsRequired(false);
        // Registration status and timestamps.
        builder.Property(company => company.RegistrationStatus).HasConversion<int>().HasColumnName("registration_status").IsRequired();
        builder.Property(company => company.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(company => company.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(company => company.IsDeleted).HasColumnName("is_deleted").IsRequired();
        // Fiscal identifiers are unique within their country.
        builder.HasIndex(company => new { company.CountryCode, company.TaxCode }).IsUnique().HasDatabaseName("ux_companies_country_tax_code");
        builder.HasIndex(company => new { company.CountryCode, company.VatNumber }).IsUnique().HasDatabaseName("ux_companies_country_vat_number");
    }
}

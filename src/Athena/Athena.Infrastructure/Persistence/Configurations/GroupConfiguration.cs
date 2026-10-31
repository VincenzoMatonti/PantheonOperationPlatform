using Athena.Domain.Company.Registry.Entities;
using Athena.Domain.Company.Registry.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athena.Infrastructure.Persistence.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("groups");
        // Chiave primaria numerica interna, generata dal database.
        builder.Property<int>("id").UseIdentityByDefaultColumn();
        builder.HasKey("id").HasName("pk_groups");
        // Identificativo di dominio GUID.
        builder.Property(group => group.Id).HasConversion(id => id.Value, value => GroupId.Create(value)).HasColumnName("group_id").IsRequired();
        // GUID univoco indipendentemente dalla PK numerica.
        builder.HasIndex(group => group.Id).IsUnique().HasDatabaseName("ux_groups_group_id");
        // Chiave alternativa per le relazioni composite.
        builder.HasAlternateKey("id", nameof(Group.Id)).HasName("ak_groups_id_group_id");
        builder.Property(group => group.Name).HasConversion(name => name.Value, value => GroupName.Create(value)).HasColumnName("name").HasMaxLength(GroupName.MaxLength).IsRequired();
        builder.HasIndex(group => group.Name).IsUnique().HasDatabaseName("ux_groups_name");
        builder.Property(group => group.RegistrationStatus).HasConversion<int>().HasColumnName("registration_status").IsRequired();
        builder.Property(group => group.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(group => group.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(group => group.IsDeleted).HasColumnName("is_deleted").IsRequired();
    }
}

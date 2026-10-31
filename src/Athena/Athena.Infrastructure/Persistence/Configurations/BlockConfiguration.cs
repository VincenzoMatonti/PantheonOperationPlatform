using Athena.Domain.Block.Registry.Entities;
using Athena.Domain.Block.Registry.Enums;
using Athena.Domain.Block.Registry.ValueObjects;
using Athena.Domain.Ecosystem.Registry.Entities;
using Athena.Domain.Ecosystem.Registry.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athena.Infrastructure.Persistence.Configurations;

public sealed class BlockConfiguration : IEntityTypeConfiguration<Block>
{
    public void Configure(EntityTypeBuilder<Block> builder)
    {
        builder.ToTable("blocks");
        // Internal database identity
        builder.Property<int>("id").UseIdentityByDefaultColumn();
        builder.HasKey("id").HasName("pk_blocks");
        // Domain identity
        builder.Property(block => block.Id).HasConversion(id => id.Value, value => BlockId.Create(value)).HasColumnName("block_id").IsRequired();
        builder.HasIndex(block => block.Id).IsUnique().HasDatabaseName("ux_blocks_block_id");
        builder.HasAlternateKey("id", nameof(Block.Id)).HasName("ak_blocks_id_block_id");
        // Parent Ecosystem
        builder.Property(block => block.EcosystemId).HasConversion(id => id.Value, value => EcosystemId.Create(value)).HasColumnName("ecosystem_id").IsRequired();
        builder.Property<int>("ecosystem_database_id").HasColumnName("ecosystem_database_id").IsRequired();
        builder.HasOne<Ecosystem>().WithMany().HasPrincipalKey("id", nameof(Ecosystem.Id)).HasForeignKey("ecosystem_database_id", nameof(Block.EcosystemId)).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("ecosystem_database_id", nameof(Block.EcosystemId)).HasDatabaseName("ix_blocks_ecosystem_database_id_ecosystem_id");
        // Name
        builder.Property(block => block.Name).HasConversion(name => name.Value, value => BlockName.Create(value)).HasColumnName("name").HasMaxLength(BlockName.MaxLength).IsRequired();
        builder.HasIndex(block => block.Name).IsUnique().HasDatabaseName("ux_blocks_name");
        // Registration status
        builder.Property(block => block.RegistrationStatus).HasConversion<int>().HasColumnName("registration_status").IsRequired();
        // Availability status
        builder.Property(block => block.AvailabilityStatus).HasConversion<int>().HasColumnName("availability_status").HasDefaultValue(BlockAvailabilityStatus.Unknown).IsRequired();
        // Audit
        builder.Property(block => block.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(block => block.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(block => block.IsDeleted).HasColumnName("is_deleted").IsRequired();
    }
}
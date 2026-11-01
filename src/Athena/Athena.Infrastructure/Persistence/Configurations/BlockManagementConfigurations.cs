using Athena.Domain.Block.Management.Entities;
using Athena.Domain.Block.Registry.Entities;
using Athena.Domain.Block.Registry.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athena.Infrastructure.Persistence.Configurations;

public sealed class BlockLifecycleConfiguration : IEntityTypeConfiguration<BlockLifecycle>
{
    public void Configure(EntityTypeBuilder<BlockLifecycle> builder)
    {
        builder.ToTable("block_lifecycles");
        builder.Property<int>("block_database_id").HasColumnName("block_database_id").ValueGeneratedNever();
        builder.HasKey("block_database_id").HasName("pk_block_lifecycles");
        builder.Property(x => x.Id).HasConversion(id => id.Value, value => BlockId.Create(value)).HasColumnName("block_id").IsRequired();
        builder.HasOne<Block>().WithOne().HasPrincipalKey<Block>("id", nameof(Block.Id))
            .HasForeignKey<BlockLifecycle>("block_database_id", nameof(BlockLifecycle.Id)).OnDelete(DeleteBehavior.Restrict);
        builder.Property(x => x.DesiredState).HasConversion<int>().HasColumnName("desired_state").IsRequired();
        builder.Property(x => x.OperationalStatus).HasConversion<int>().HasColumnName("operational_status").IsRequired();
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted").IsRequired();
    }
}

public sealed class BlockOperationConfiguration : IEntityTypeConfiguration<BlockOperation>
{
    public void Configure(EntityTypeBuilder<BlockOperation> builder)
    {
        builder.ToTable("block_operations");
        builder.HasKey(x => x.Id).HasName("pk_block_operations");
        builder.Property(x => x.Id).HasColumnName("operation_id");
        builder.Property<int>("block_database_id").HasColumnName("block_database_id").IsRequired();
        builder.Property(x => x.BlockId).HasConversion(id => id.Value, value => BlockId.Create(value)).HasColumnName("block_id").IsRequired();
        builder.HasOne<Block>().WithMany().HasPrincipalKey("id", nameof(Block.Id))
            .HasForeignKey("block_database_id", nameof(BlockOperation.BlockId)).OnDelete(DeleteBehavior.Restrict);
        builder.Property(x => x.Type).HasConversion<int>().HasColumnName("operation_type").IsRequired();
        builder.Property(x => x.Status).HasConversion<int>().HasColumnName("operation_status").IsRequired();
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(x => x.StartedAtUtc).HasColumnName("started_at_utc");
        builder.Property(x => x.CompletedAtUtc).HasColumnName("completed_at_utc");
        builder.Property(x => x.ErrorMessage).HasColumnName("error_message").HasMaxLength(4000);
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted").IsRequired();
        builder.HasIndex("block_database_id", nameof(BlockOperation.CreatedAtUtc)).HasDatabaseName("ix_block_operations_block_created");
    }
}

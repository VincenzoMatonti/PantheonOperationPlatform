using Athena.Domain.Block.Registry.Entities;
using Athena.Domain.Block.Registry.ValueObjects;
using Athena.Domain.Workload.Registry.Entities;
using Athena.Domain.Workload.Registry.Enums;
using Athena.Domain.Workload.Registry.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athena.Infrastructure.Persistence.Configurations;

public sealed class WorkloadConfiguration : IEntityTypeConfiguration<Workload>
{
    public void Configure(EntityTypeBuilder<Workload> builder)
    {
        builder.ToTable("workloads");
        // Technical primary key.
        builder.Property<int>("id").UseIdentityByDefaultColumn();
        builder.HasKey("id").HasName("pk_workloads");
        // Domain identifier.
        builder.Property(workload => workload.Id).HasConversion(id => id.Value, value => WorkloadId.Create(value)).HasColumnName("workload_id").IsRequired();
        builder.HasIndex(workload => workload.Id).IsUnique().HasDatabaseName("ux_workloads_workload_id");
        builder.HasAlternateKey("id", nameof(Workload.Id)).HasName("ak_workloads_id_workload_id");
        // Block relationship.
        builder.Property(workload => workload.BlockId).HasConversion(id => id.Value, value => BlockId.Create(value)).HasColumnName("block_id").IsRequired();
        builder.Property<int>("block_database_id").HasColumnName("block_database_id").IsRequired();
        builder.HasOne<Block>().WithMany().HasPrincipalKey("id", nameof(Block.Id)).HasForeignKey("block_database_id", nameof(Workload.BlockId)).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("block_database_id", nameof(Workload.BlockId)).HasDatabaseName("ix_workloads_block_database_id_block_id");
        // Name.
        builder.Property(workload => workload.Name).HasConversion(name => name.Value, value => WorkloadName.Create(value)).HasColumnName("name").HasMaxLength(WorkloadName.MaxLength).IsRequired();
        builder.HasIndex("block_database_id", nameof(Workload.BlockId), nameof(Workload.Name)).IsUnique().HasDatabaseName("ux_workloads_block_name");
        // Registration status.
        builder.Property(workload => workload.RegistrationStatus).HasConversion<int>().HasColumnName("registration_status").IsRequired();
        // Availability status.
        builder.Property(workload => workload.AvailabilityStatus).HasConversion<int>().HasColumnName("availability_status").HasDefaultValue(WorkloadAvailabilityStatus.Unknown).IsRequired();
        // Audit fields.
        builder.Property(workload => workload.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(workload => workload.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(workload => workload.IsDeleted).HasColumnName("is_deleted").IsRequired();
    }
}
using Athena.Domain.Ecosystem.Management.Entities;
using Athena.Domain.Ecosystem.Registry.Entities;
using Athena.Domain.Ecosystem.Registry.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athena.Infrastructure.Persistence.Configurations;

public sealed class EcosystemLifecycleConfiguration : IEntityTypeConfiguration<EcosystemLifecycle>
{
    public void Configure(EntityTypeBuilder<EcosystemLifecycle> builder)
    {
        builder.ToTable("ecosystem_lifecycles");
        builder.Property<int>("ecosystem_database_id").HasColumnName("ecosystem_database_id").ValueGeneratedNever();
        builder.HasKey("ecosystem_database_id").HasName("pk_ecosystem_lifecycles");
        builder.Property(x => x.Id).HasConversion(id => id.Value, value => EcosystemId.Create(value)).HasColumnName("ecosystem_id").IsRequired();
        builder.HasOne<Ecosystem>().WithOne().HasPrincipalKey<Ecosystem>("id", nameof(Ecosystem.Id))
            .HasForeignKey<EcosystemLifecycle>("ecosystem_database_id", nameof(EcosystemLifecycle.Id)).OnDelete(DeleteBehavior.Restrict);
        builder.Property(x => x.DesiredState).HasConversion<int>().HasColumnName("desired_state").IsRequired();
        builder.Property(x => x.OperationalStatus).HasConversion<int>().HasColumnName("operational_status").IsRequired();
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted").IsRequired();
    }
}

public sealed class EcosystemOperationConfiguration : IEntityTypeConfiguration<EcosystemOperation>
{
    public void Configure(EntityTypeBuilder<EcosystemOperation> builder)
    {
        builder.ToTable("ecosystem_operations");
        builder.HasKey(x => x.Id).HasName("pk_ecosystem_operations");
        builder.Property(x => x.Id).HasColumnName("operation_id");
        builder.Property<int>("ecosystem_database_id").HasColumnName("ecosystem_database_id").IsRequired();
        builder.Property(x => x.EcosystemId).HasConversion(id => id.Value, value => EcosystemId.Create(value)).HasColumnName("ecosystem_id").IsRequired();
        builder.HasOne<Ecosystem>().WithMany().HasPrincipalKey("id", nameof(Ecosystem.Id))
            .HasForeignKey("ecosystem_database_id", nameof(EcosystemOperation.EcosystemId)).OnDelete(DeleteBehavior.Restrict);
        builder.Property(x => x.Type).HasConversion<int>().HasColumnName("operation_type").IsRequired();
        builder.Property(x => x.Status).HasConversion<int>().HasColumnName("operation_status").IsRequired();
        builder.Property(x => x.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(x => x.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(x => x.StartedAtUtc).HasColumnName("started_at_utc");
        builder.Property(x => x.CompletedAtUtc).HasColumnName("completed_at_utc");
        builder.Property(x => x.ErrorMessage).HasColumnName("error_message").HasMaxLength(4000);
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted").IsRequired();
        builder.HasIndex("ecosystem_database_id", nameof(EcosystemOperation.CreatedAtUtc)).HasDatabaseName("ix_ecosystem_operations_ecosystem_created");
    }
}

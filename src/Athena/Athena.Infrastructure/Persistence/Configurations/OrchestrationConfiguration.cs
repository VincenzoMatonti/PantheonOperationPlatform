using Athena.Domain.OrchestrationRegistry.Entities;
using Athena.Domain.OrchestrationRegistry.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athena.Infrastructure.Persistence.Configurations;

public class OrchestrationConfiguration : IEntityTypeConfiguration<Orchestration>
{
    public void Configure(EntityTypeBuilder<Orchestration> builder)
    {
        builder.ToTable("orchestrations");
        builder.Property<int>("id").UseIdentityByDefaultColumn();
        builder.HasKey("id");
        builder.Property(orchestration => orchestration.Id)
            .HasConversion(id => id.Value, value => OrchestrationId.Create(value))
            .HasColumnName("orchestration_id")
            .IsRequired();

        builder.HasIndex(orchestration => orchestration.Id)
            .IsUnique();

        builder.Property(orchestration => orchestration.Name)
            .HasConversion(name => name.Value, value => OrchestrationName.Create(value))
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(orchestration => orchestration.Name).IsUnique();
        builder.Property(orchestration => orchestration.RegistrationStatus)
            .HasConversion<int>()
            .HasColumnName("registration_status")
            .IsRequired();

        builder.Property(orchestration => orchestration.AvailabilityStatus)
            .HasConversion<int>()
            .HasColumnName("availability_status")
            .IsRequired();

        builder.Property(orchestration => orchestration.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(orchestration => orchestration.UpdatedAtUtc).HasColumnName("updated_at_utc");
        builder.Property(orchestration => orchestration.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired();
    }
}
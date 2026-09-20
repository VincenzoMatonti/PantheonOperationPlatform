using Hermes.Domain.Operations.Entities;
using Hermes.Domain.Packages.Entities;
using Hermes.Domain.Packages.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.Infrastructure.Persistence.Configurations.Packages;

public class PackageConfiguration : IEntityTypeConfiguration<Package>
{
    public void Configure(EntityTypeBuilder<Package> builder)
    {
        builder.ToTable("packages");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.OperationId).IsRequired();
        builder.Property(x => x.Type).HasConversion(value => value.Value, value => PackageType.Create(value)).IsRequired();
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();
        builder.Property(x => x.ContentType).HasConversion(value => value.Value, value => ContentType.Create(value)).IsRequired();
        builder.Property(x => x.Version).HasConversion(value => value.Value, value => PackageVersion.Create(value)).IsRequired();
        builder.Property(x => x.Sequence).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.HasOne<Operation>().WithMany().HasForeignKey(x => x.OperationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => new { x.OperationId, x.Sequence }).IsUnique();
        builder.HasIndex(x => x.OperationId);
        builder.HasIndex(x => x.Status);
    }
}
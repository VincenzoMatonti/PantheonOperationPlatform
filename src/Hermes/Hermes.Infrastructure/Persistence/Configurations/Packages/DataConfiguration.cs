using Hermes.Domain.Packages.Entities;
using Hermes.Domain.Packages.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.Infrastructure.Persistence.Configurations.Packages;

public class DataConfiguration : IEntityTypeConfiguration<Data>
{
    public void Configure(EntityTypeBuilder<Data> builder)
    {
        builder.ToTable("data");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.PackageId).IsRequired();
        builder.Property(x => x.Type).HasConversion(value => value.Value, value => DataType.Create(value)).IsRequired();
        builder.Property(x => x.ContentType).HasConversion(value => value.Value, value => ContentType.Create(value)).IsRequired();
        builder.Property(x => x.Content).IsRequired();
        builder.Property(x => x.Sequence).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.HasOne<Package>().WithMany().HasForeignKey(x => x.PackageId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => new { x.PackageId, x.Sequence }).IsUnique();
        builder.HasIndex(x => x.PackageId);
    }
}
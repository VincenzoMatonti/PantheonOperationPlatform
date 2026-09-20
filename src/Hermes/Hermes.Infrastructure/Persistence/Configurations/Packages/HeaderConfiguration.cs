using Hermes.Domain.Packages.Entities;
using Hermes.Domain.Packages.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.Infrastructure.Persistence.Configurations.Packages;

public class HeaderConfiguration : IEntityTypeConfiguration<Header>
{
    public void Configure(EntityTypeBuilder<Header> builder)
    {
        builder.ToTable("headers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.PackageId).IsRequired();
        builder.Property(x => x.Key).HasConversion(value => value.Value, value => HeaderKey.Create(value)).IsRequired();
        builder.Property(x => x.Value).HasConversion(value => value.Value, value => HeaderValue.Create(value)).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.HasOne<Package>().WithMany().HasForeignKey(x => x.PackageId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => new { x.PackageId, x.Key }).IsUnique();
        builder.HasIndex(x => x.PackageId);
    }
}
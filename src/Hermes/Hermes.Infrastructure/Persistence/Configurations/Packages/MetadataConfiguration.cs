using Hermes.Domain.Packages.Entities;
using Hermes.Domain.Packages.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.Infrastructure.Persistence.Configurations.Packages;

public class MetadataConfiguration : IEntityTypeConfiguration<Metadata>
{
    public void Configure(EntityTypeBuilder<Metadata> builder)
    {
        builder.ToTable("metadata");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.PackageId).IsRequired();
        builder.Property(x => x.DataId);
        builder.Property(x => x.Key).HasConversion(value => value.Value, value => MetadataKey.Create(value)).IsRequired();
        builder.Property(x => x.Value).HasConversion(value => value.Value, value => MetadataValue.Create(value)).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.HasOne<Package>().WithMany().HasForeignKey(x => x.PackageId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Data>().WithMany().HasForeignKey(x => x.DataId).OnDelete(DeleteBehavior.Restrict);
        
        // Metadata appartenenti al Package:
        // stessa Key non duplicabile nello stesso Package.
        builder.HasIndex(x => new { x.PackageId, x.Key }).HasFilter("\"DataId\" IS NULL").IsUnique();
        
        // Metadata appartenenti al Data:
        // stessa Key non duplicabile nello stesso Data.
        builder.HasIndex(x => new { x.DataId, x.Key }).HasFilter("\"DataId\" IS NOT NULL").IsUnique();
        
        builder.HasIndex(x => x.PackageId);
        builder.HasIndex(x => x.DataId);
    }
}
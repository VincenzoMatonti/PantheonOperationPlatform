using Hermes.Domain.Endpoints.Entities;
using Hermes.Domain.Operations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.Infrastructure.Persistence.Configurations.Endpoints;

public class EndpointOperationConfiguration : IEntityTypeConfiguration<EndpointOperation>
{
    public void Configure(EntityTypeBuilder<EndpointOperation> builder)
    {
        builder.ToTable("endpoint_operations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.EndpointId).IsRequired();
        builder.Property(x => x.OperationTypeId).IsRequired();
        builder.Property(x => x.IsEnabled).IsRequired();
        builder.Property(x => x.IsDeleted).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.HasOne<Endpoint>().WithMany().HasForeignKey(x => x.EndpointId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<OperationType>().WithMany().HasForeignKey(x => x.OperationTypeId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => new { x.EndpointId, x.OperationTypeId }).IsUnique();
    }
}
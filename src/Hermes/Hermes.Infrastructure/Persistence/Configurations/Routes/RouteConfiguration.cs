using Hermes.Domain.Clients.Entities;
using Hermes.Domain.Endpoints.Entities;
using Hermes.Domain.Operations.Entities;
using Hermes.Domain.Routes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.Infrastructure.Persistence.Configurations;

public class RouteConfiguration : IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> builder)
    {
        builder.ToTable("routes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.ClientId).IsRequired();
        builder.Property(x => x.OperationTypeId).IsRequired();
        builder.Property(x => x.EndpointId).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.IsDeleted).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.HasOne<Client>().WithMany().HasForeignKey(x => x.ClientId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<OperationType>().WithMany().HasForeignKey(x => x.OperationTypeId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Endpoint>().WithMany().HasForeignKey(x => x.EndpointId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => new { x.ClientId, x.OperationTypeId, x.EndpointId }).IsUnique();
    }
}
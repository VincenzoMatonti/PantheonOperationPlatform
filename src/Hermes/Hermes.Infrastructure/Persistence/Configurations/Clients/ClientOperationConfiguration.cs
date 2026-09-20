using Hermes.Domain.Clients.Entities;
using Hermes.Domain.Operations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.Infrastructure.Persistence.Configurations.Clients;

public class ClientOperationConfiguration : IEntityTypeConfiguration<ClientOperation>
{
    public void Configure(EntityTypeBuilder<ClientOperation> builder)
    {
        builder.ToTable("client_operations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.ClientId).IsRequired();
        builder.Property(x => x.OperationTypeId).IsRequired();
        builder.Property(x => x.IsEnabled).IsRequired();
        builder.Property(x => x.IsDeleted).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.HasOne<Client>().WithMany().HasForeignKey(x => x.ClientId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<OperationType>().WithMany().HasForeignKey(x => x.OperationTypeId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => new { x.ClientId, x.OperationTypeId }).IsUnique();
    }
}
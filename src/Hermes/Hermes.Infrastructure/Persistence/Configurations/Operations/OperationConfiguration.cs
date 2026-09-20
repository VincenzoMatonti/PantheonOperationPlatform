using Hermes.Domain.Executions.Entities;
using Hermes.Domain.Operations.Entities;
using Hermes.Domain.Operations.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.Infrastructure.Persistence.Configurations.Operations;

public class OperationConfiguration : IEntityTypeConfiguration<Operation>
{
    public void Configure(EntityTypeBuilder<Operation> builder)
    {
        builder.ToTable("operations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.ExecutionId).IsRequired();
        builder.Property(x => x.CorrelationId).HasConversion(value => value.Value, value => CorrelationId.From(value)).IsRequired();
        builder.Property(x => x.ExternalId).HasConversion(value => value.Value, value => ExternalOperationId.Create(value)).IsRequired();
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();
        builder.Property(x => x.IsDeleted).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.HasOne<Execution>().WithMany().HasForeignKey(x => x.ExecutionId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.ExternalId).IsUnique();
        builder.HasIndex(x => x.CorrelationId);
        builder.HasIndex(x => x.ExecutionId).IsUnique();
        builder.HasIndex(x => x.Status);
    }
}
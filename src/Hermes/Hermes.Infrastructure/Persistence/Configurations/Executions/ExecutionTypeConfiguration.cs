using Hermes.Domain.Executions.Entities;
using Hermes.Domain.Executions.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.Infrastructure.Persistence.Configurations.Executions;

public class ExecutionTypeConfiguration
    : IEntityTypeConfiguration<ExecutionType>
{
    public void Configure(EntityTypeBuilder<ExecutionType> builder)
    {
        builder.ToTable("execution_types");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Code).HasConversion(value => value.Value, value => ExecutionTypeCode.Create(value)).IsRequired();
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
    }
}
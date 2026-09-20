using Hermes.Domain.Executions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.Infrastructure.Persistence.Configurations.Executions;

public class ExecutionConfiguration : IEntityTypeConfiguration<Execution>
{
    public void Configure(EntityTypeBuilder<Execution> builder)
    {
        builder.ToTable("executions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.StartedAt);
        builder.Property(x => x.CompletedAt);
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.HasIndex(x => x.Status);
    }
}
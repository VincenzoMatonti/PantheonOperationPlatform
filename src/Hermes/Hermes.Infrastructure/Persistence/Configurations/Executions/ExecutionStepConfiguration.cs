using Hermes.Domain.Executions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.Infrastructure.Persistence.Configurations.Executions;

public class ExecutionStepConfiguration : IEntityTypeConfiguration<ExecutionStep>
{
    public void Configure(EntityTypeBuilder<ExecutionStep> builder)
    {
        builder.ToTable("execution_steps");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.ExecutionId).IsRequired();
        builder.Property(x => x.Sequence).IsRequired();
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.StartedAt);
        builder.Property(x => x.CompletedAt);
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.HasOne<Execution>().WithMany().HasForeignKey(x => x.ExecutionId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => new { x.ExecutionId, x.Sequence }).IsUnique();
        builder.HasIndex(x => x.ExecutionId);
        builder.HasIndex(x => x.Status);
    }
}
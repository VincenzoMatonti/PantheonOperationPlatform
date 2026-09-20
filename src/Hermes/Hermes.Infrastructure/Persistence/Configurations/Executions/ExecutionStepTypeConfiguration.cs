using Hermes.Domain.Executions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.Infrastructure.Persistence.Configurations.Executions;

public class ExecutionStepTypeConfiguration
    : IEntityTypeConfiguration<ExecutionStepType>
{
    public void Configure(EntityTypeBuilder<ExecutionStepType> builder)
    {
        builder.ToTable("execution_step_types");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.ExecutionStepId).IsRequired();
        builder.Property(x => x.ExecutionTypeId).IsRequired();
        builder.Property(x => x.IsEnabled).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.HasOne<ExecutionStep>().WithMany().HasForeignKey(x => x.ExecutionStepId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<ExecutionType>().WithMany().HasForeignKey(x => x.ExecutionTypeId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => new { x.ExecutionStepId, x.ExecutionTypeId }).IsUnique();
        builder.HasIndex(x => x.ExecutionStepId);
        builder.HasIndex(x => x.ExecutionTypeId);
    }
}
using Hermes.Domain.Executions.Entities;
using Hermes.Domain.Routes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.Infrastructure.Persistence.Configurations.Executions;

public class ExecutionStepRouteConfiguration
    : IEntityTypeConfiguration<ExecutionStepRoute>
{
    public void Configure(EntityTypeBuilder<ExecutionStepRoute> builder)
    {
        builder.ToTable("execution_step_routes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.ExecutionStepId).IsRequired();
        builder.Property(x => x.RouteId).IsRequired();
        builder.Property(x => x.IsEnabled).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.HasOne<ExecutionStep>().WithMany().HasForeignKey(x => x.ExecutionStepId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Route>().WithMany().HasForeignKey(x => x.RouteId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => new { x.ExecutionStepId, x.RouteId }).IsUnique();
        builder.HasIndex(x => x.ExecutionStepId);
        builder.HasIndex(x => x.RouteId);
    }
}
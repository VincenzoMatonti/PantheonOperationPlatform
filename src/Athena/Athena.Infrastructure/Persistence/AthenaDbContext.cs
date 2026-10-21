using Microsoft.EntityFrameworkCore;
using Athena.Domain.SystemManagement.Entities;
using RegisteredSystem = Athena.Domain.SystemRegistry.Entities.System;
using RegisteredOrchestration = Athena.Domain.OrchestrationRegistry.Entities.Orchestration;
using Athena.Domain.OrchestrationManagement.Entities;

namespace Athena.Infrastructure.Persistence;

public sealed class AthenaDbContext(DbContextOptions<AthenaDbContext> options) : DbContext(options)
{
    public DbSet<RegisteredSystem> Systems => Set<RegisteredSystem>();
    public DbSet<SystemLifecycle> SystemLifecycles => Set<SystemLifecycle>();
    public DbSet<SystemLifecycleOperation> SystemLifecycleOperations => Set<SystemLifecycleOperation>();
    public DbSet<RegisteredOrchestration> Orchestrations => Set<RegisteredOrchestration>();
    public DbSet<OrchestrationLifecycle> OrchestrationLifecycles => Set<OrchestrationLifecycle>();
    public DbSet<OrchestrationLifecycleOperation> OrchestrationLifecycleOperations => Set<OrchestrationLifecycleOperation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AthenaDbContext).Assembly);
    }
}
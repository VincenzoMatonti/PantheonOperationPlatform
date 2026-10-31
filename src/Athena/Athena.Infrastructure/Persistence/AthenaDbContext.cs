using Microsoft.EntityFrameworkCore;
using Athena.Domain.SystemManagement.Entities;
using RegisteredSystem = Athena.Domain.SystemRegistry.Entities.System;
using RegisteredOrchestration = Athena.Domain.OrchestrationRegistry.Entities.Orchestration;
using Athena.Domain.OrchestrationManagement.Entities;
using Athena.Infrastructure.Persistence.Runtime;

namespace Athena.Infrastructure.Persistence;

public class AthenaDbContext(DbContextOptions<AthenaDbContext> options) : DbContext(options)
{
    public DbSet<RegisteredSystem> Systems => Set<RegisteredSystem>();
    public DbSet<SystemLifecycle> SystemLifecycles => Set<SystemLifecycle>();
    public DbSet<SystemLifecycleOperation> SystemLifecycleOperations => Set<SystemLifecycleOperation>();
    public DbSet<RegisteredOrchestration> Orchestrations => Set<RegisteredOrchestration>();
    public DbSet<OrchestrationLifecycle> OrchestrationLifecycles => Set<OrchestrationLifecycle>();
    public DbSet<OrchestrationLifecycleOperation> OrchestrationLifecycleOperations => Set<OrchestrationLifecycleOperation>();
    public DbSet<OrchestrationSystem> OrchestrationSystems => Set<OrchestrationSystem>();
    public DbSet<SystemRuntimeInstance> SystemRuntimeInstances => Set<SystemRuntimeInstance>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Dichiarare prima i tipi delle chiavi tecniche shadow.
        modelBuilder.Entity<RegisteredSystem>().Property<int>("id");
        modelBuilder.Entity<RegisteredOrchestration>().Property<int>("id");

        // Applicare tutte le configurazioni.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AthenaDbContext).Assembly);
    }
}
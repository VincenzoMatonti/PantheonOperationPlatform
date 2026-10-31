using Microsoft.EntityFrameworkCore;
using Athena.Domain.Company.Registry.Entities;
using Athena.Domain.System.Management.Entities;
using Athena.Infrastructure.Persistence.Runtime;
using Athena.Domain.Orchestration.Management.Entities;
using RegisteredSystem = Athena.Domain.System.Registry.Entities.System;
using RegisteredOrchestration = Athena.Domain.Orchestration.Registry.Entities.Orchestration;
using Athena.Domain.Ecosystem.Registry.Entities;
using Athena.Domain.Block.Registry.Entities;




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
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Ecosystem> Ecosystems => Set<Ecosystem>();
    public DbSet<Block> Blocks => Set<Block>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Dichiarare prima i tipi delle chiavi tecniche shadow.
        modelBuilder.Entity<RegisteredSystem>().Property<int>("id");
        modelBuilder.Entity<RegisteredOrchestration>().Property<int>("id");
        modelBuilder.Entity<Group>().Property<int>("id");
        modelBuilder.Entity<Company>().Property<int>("id");
        modelBuilder.Entity<Ecosystem>().Property<int>("id");
        modelBuilder.Entity<Ecosystem>().Property<int>("company_database_id");
        modelBuilder.Entity<Block>().Property<int>("id");
        modelBuilder.Entity<Block>().Property<int>("ecosystem_database_id");

        // Applicare tutte le configurazioni.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AthenaDbContext).Assembly);
    }
}
using Microsoft.EntityFrameworkCore;
using Athena.Domain.Company.Entities;
using Athena.Domain.Orchestration.Management.Entities;
using RegisteredSystem = Athena.Domain.System.Registry.Entities.System;
using RegisteredOrchestration = Athena.Domain.Orchestration.Registry.Entities.Orchestration;
using RegisteredSystemInstance = Athena.Domain.System.Registry.Entities.SystemInstance;
using SystemInstanceLifecycle = Athena.Domain.System.Management.Entities.SystemInstanceLifecycle;
using SystemInstanceOperation = Athena.Domain.System.Management.Entities.SystemInstanceOperation;
using WorkloadLifecycle = Athena.Domain.Workload.Management.Entities.WorkloadLifecycle;
using WorkloadOperation = Athena.Domain.Workload.Management.Entities.WorkloadOperation;
using BlockLifecycle = Athena.Domain.Block.Management.Entities.BlockLifecycle;
using BlockOperation = Athena.Domain.Block.Management.Entities.BlockOperation;
using EcosystemLifecycle = Athena.Domain.Ecosystem.Management.Entities.EcosystemLifecycle;
using EcosystemOperation = Athena.Domain.Ecosystem.Management.Entities.EcosystemOperation;
using Athena.Domain.Ecosystem.Registry.Entities;
using Athena.Domain.Block.Registry.Entities;
using Athena.Domain.Workload.Registry.Entities;




namespace Athena.Infrastructure.Persistence;

public class AthenaDbContext(DbContextOptions<AthenaDbContext> options) : DbContext(options)
{
    public DbSet<RegisteredSystem> Systems => Set<RegisteredSystem>();
    public DbSet<RegisteredOrchestration> Orchestrations => Set<RegisteredOrchestration>();
    public DbSet<OrchestrationLifecycle> OrchestrationLifecycles => Set<OrchestrationLifecycle>();
    public DbSet<OrchestrationLifecycleOperation> OrchestrationLifecycleOperations => Set<OrchestrationLifecycleOperation>();
    public DbSet<OrchestrationSystemInstance> OrchestrationSystemInstances => Set<OrchestrationSystemInstance>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Ecosystem> Ecosystems => Set<Ecosystem>();
    public DbSet<Block> Blocks => Set<Block>();
    public DbSet<Workload> Workloads => Set<Workload>();
    public DbSet<RegisteredSystemInstance> SystemInstances => Set<RegisteredSystemInstance>();
    public DbSet<SystemInstanceLifecycle> SystemInstanceLifecycles => Set<SystemInstanceLifecycle>();
    public DbSet<SystemInstanceOperation> SystemInstanceOperations => Set<SystemInstanceOperation>();
    public DbSet<WorkloadLifecycle> WorkloadLifecycles => Set<WorkloadLifecycle>();
    public DbSet<WorkloadOperation> WorkloadOperations => Set<WorkloadOperation>();
    public DbSet<BlockLifecycle> BlockLifecycles => Set<BlockLifecycle>();
    public DbSet<BlockOperation> BlockOperations => Set<BlockOperation>();
    public DbSet<EcosystemLifecycle> EcosystemLifecycles => Set<EcosystemLifecycle>();
    public DbSet<EcosystemOperation> EcosystemOperations => Set<EcosystemOperation>();

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
        modelBuilder.Entity<Workload>().Property<int>("id");
        modelBuilder.Entity<Workload>().Property<int>("block_database_id");
        modelBuilder.Entity<RegisteredSystemInstance>().Property<int>("id");
        modelBuilder.Entity<RegisteredSystemInstance>().Property<int>("system_database_id");
        modelBuilder.Entity<RegisteredSystemInstance>().Property<int>("workload_database_id");

        // Applicare tutte le configurazioni.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AthenaDbContext).Assembly);
    }
}
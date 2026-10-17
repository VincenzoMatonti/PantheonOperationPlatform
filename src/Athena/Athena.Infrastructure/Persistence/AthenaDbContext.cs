using Microsoft.EntityFrameworkCore;
using Athena.Domain.SystemManagement.Entities;
using RegisteredSystem = Athena.Domain.SystemRegistry.Entities.System;

namespace Athena.Infrastructure.Persistence;

public sealed class AthenaDbContext(DbContextOptions<AthenaDbContext> options) : DbContext(options)
{
    public DbSet<RegisteredSystem> Systems => Set<RegisteredSystem>();
    public DbSet<SystemLifecycle> SystemLifecycles => Set<SystemLifecycle>();
    public DbSet<SystemLifecycleOperation> SystemLifecycleOperations => Set<SystemLifecycleOperation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AthenaDbContext).Assembly);
    }
}
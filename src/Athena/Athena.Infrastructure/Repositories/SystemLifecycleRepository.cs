using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Management.Enums;
using Athena.Domain.System.Management.Repositories;
using Athena.Domain.System.Registry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.Infrastructure.Repositories;

public class SystemLifecycleRepository(AthenaDbContext dbContext) : ISystemLifecycleRepository
{
    // Retrieval
    public async Task<SystemLifecycle?> GetBySystemIdAsync(SystemId systemId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(systemId);
        var systemDatabaseId = await GetSystemDatabaseIdAsync(systemId, cancellationToken);
        if (systemDatabaseId is null) return null;
        return await dbContext.SystemLifecycles.SingleOrDefaultAsync(lifecycle =>
        EF.Property<int>(lifecycle, "system_database_id") == systemDatabaseId.Value, cancellationToken);
    }

    public async Task<IReadOnlyList<SystemLifecycle>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SystemLifecycles.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SystemLifecycle>> GetByDesiredStateAsync(
        SystemDesiredState desiredState, CancellationToken cancellationToken = default)
    {
        return await dbContext.SystemLifecycles.Where(lifecycle => lifecycle.DesiredState == desiredState).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SystemLifecycle>> GetByOperationalStatusAsync(SystemOperationalStatus operationalStatus, CancellationToken cancellationToken = default)
    {
        return await dbContext.SystemLifecycles.Where(lifecycle => lifecycle.OperationalStatus == operationalStatus).ToListAsync(cancellationToken);
    }

    // Soft deletion
    public async Task<IReadOnlyList<SystemLifecycle>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default)
    {
        return await dbContext.SystemLifecycles.Where(lifecycle => lifecycle.IsDeleted == isDeleted).ToListAsync(cancellationToken);
    }

    // Existence checks
    public async Task<bool> ExistsBySystemIdAsync(SystemId systemId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(systemId);
        var systemDatabaseId = await GetSystemDatabaseIdAsync(systemId, cancellationToken);
        if (systemDatabaseId is null) return false;
        return await dbContext.SystemLifecycles.AnyAsync(lifecycle =>
        EF.Property<int>(lifecycle, "system_database_id") == systemDatabaseId.Value, cancellationToken);
    }

    // Persistence
    public async Task AddAsync(SystemLifecycle lifecycle, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(lifecycle);
        var systemDatabaseId = await GetSystemDatabaseIdAsync(lifecycle.SystemId, cancellationToken) ??
        throw new InvalidOperationException($"System '{lifecycle.SystemId.Value}' does not exist.");
        dbContext.Entry(lifecycle).Property<int>("system_database_id").CurrentValue = systemDatabaseId;
        await dbContext.SystemLifecycles.AddAsync(lifecycle, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(SystemLifecycle lifecycle, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(lifecycle);
        var systemDatabaseId = await GetSystemDatabaseIdAsync(lifecycle.SystemId, cancellationToken) ??
        throw new InvalidOperationException($"System '{lifecycle.SystemId.Value}' does not exist.");
        var entry = dbContext.Entry(lifecycle);
        if (entry.State == EntityState.Detached) dbContext.SystemLifecycles.Update(lifecycle);
        entry.Property<int>("system_database_id").CurrentValue = systemDatabaseId;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    // Internal helpers
    private async Task<int?> GetSystemDatabaseIdAsync(SystemId systemId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(systemId);
        return await dbContext.Systems
            .Where(system => system.Id == systemId)
            .Select(system => (int?)EF.Property<int>(system, "id"))
            .SingleOrDefaultAsync(cancellationToken);
    }
}

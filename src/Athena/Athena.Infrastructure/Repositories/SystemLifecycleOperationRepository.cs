
using Athena.Domain.SystemManagement.Entities;
using Athena.Domain.SystemManagement.Enums;
using Athena.Domain.SystemManagement.Repositories;
using Athena.Domain.SystemRegistry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.Infrastructure.Repositories;

public class SystemLifecycleOperationRepository(AthenaDbContext dbContext) : ISystemLifecycleOperationRepository
{
    // Retrieval
    public async Task<SystemLifecycleOperation?> GetByIdAsync(Guid operationId, CancellationToken cancellationToken = default)
    {
        return await dbContext.SystemLifecycleOperations.SingleOrDefaultAsync(operation => operation.Id == operationId, cancellationToken);
    }

    public async Task<IReadOnlyList<SystemLifecycleOperation>> GetBySystemIdAsync(
        SystemId systemId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(systemId);
        var systemDatabaseId = await GetSystemDatabaseIdAsync(systemId, cancellationToken);
        if (systemDatabaseId is null) return [];
        return await dbContext.SystemLifecycleOperations.Where(operation =>
            EF.Property<int>(operation, "system_database_id") == systemDatabaseId.Value).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SystemLifecycleOperation>> GetByStatusAsync(
        SystemLifecycleOperationStatus status,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.SystemLifecycleOperations
            .Where(operation => operation.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SystemLifecycleOperation>> GetBySystemIdAndStatusAsync(
            SystemId systemId, SystemLifecycleOperationStatus status,
            CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(systemId);
        var systemDatabaseId = await GetSystemDatabaseIdAsync(systemId, cancellationToken);
        if (systemDatabaseId is null) return [];
        return await dbContext.SystemLifecycleOperations.Where(operation =>
        EF.Property<int>(operation, "system_database_id") == systemDatabaseId.Value && operation.Status == status).ToListAsync(cancellationToken);
    }

    // Soft deletion
    public async Task<IReadOnlyList<SystemLifecycleOperation>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default)
    {
        return await dbContext.SystemLifecycleOperations.Where(operation => operation.IsDeleted == isDeleted).ToListAsync(cancellationToken);
    }

    // Existence checks
    public async Task<bool> ExistsByIdAsync(Guid operationId, CancellationToken cancellationToken = default)
    {
        return await dbContext.SystemLifecycleOperations.AnyAsync(operation => operation.Id == operationId, cancellationToken);
    }

    // Persistence
    public async Task AddAsync(SystemLifecycleOperation operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);
        var systemDatabaseId = await GetSystemDatabaseIdAsync(operation.SystemId, cancellationToken) ??
        throw new InvalidOperationException($"System '{operation.SystemId.Value}' does not exist.");
        dbContext.Entry(operation).Property<int>("system_database_id").CurrentValue = systemDatabaseId;
        await dbContext.SystemLifecycleOperations.AddAsync(operation, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(SystemLifecycleOperation operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);
        var systemDatabaseId = await GetSystemDatabaseIdAsync(operation.SystemId, cancellationToken) ??
        throw new InvalidOperationException($"System '{operation.SystemId.Value}' does not exist.");
        var entry = dbContext.Entry(operation);
        if (entry.State == EntityState.Detached) dbContext.SystemLifecycleOperations.Update(operation);
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

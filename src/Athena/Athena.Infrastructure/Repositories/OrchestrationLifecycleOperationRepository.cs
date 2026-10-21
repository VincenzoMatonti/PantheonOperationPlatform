using Athena.Domain.OrchestrationManagement.Entities;
using Athena.Domain.OrchestrationManagement.Enums;
using Athena.Domain.OrchestrationManagement.Repositories;
using Athena.Domain.OrchestrationRegistry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.Infrastructure.Repositories;

public class OrchestrationLifecycleOperationRepository(AthenaDbContext context) : IOrchestrationLifecycleOperationRepository
{
    // Retrieval
    public async Task<OrchestrationLifecycleOperation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.OrchestrationLifecycleOperations.SingleOrDefaultAsync(operation => operation.Id == id && !operation.IsDeleted, cancellationToken);
    }

    public async Task<List<OrchestrationLifecycleOperation>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.OrchestrationLifecycleOperations.Where(operation => !operation.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<OrchestrationLifecycleOperation>> GetByOrchestrationIdAsync(
        OrchestrationId orchestrationId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(orchestrationId);
        var databaseId = await GetOrchestrationDatabaseIdAsync(orchestrationId, cancellationToken);
        if (databaseId is null) return [];
        return await context.OrchestrationLifecycleOperations.Where(operation =>
                EF.Property<int>(operation, "orchestration_database_id") == databaseId.Value && !operation.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<OrchestrationLifecycleOperation>> GetByTypeAsync(OrchestrationLifecycleOperationType type, CancellationToken cancellationToken = default)
    {
        return await context.OrchestrationLifecycleOperations.Where(operation => !operation.IsDeleted && operation.Type == type).ToListAsync(cancellationToken);
    }

    public async Task<List<OrchestrationLifecycleOperation>> GetByStatusAsync(
        OrchestrationLifecycleOperationStatus status, CancellationToken cancellationToken = default)
    {
        return await context.OrchestrationLifecycleOperations.Where(operation => !operation.IsDeleted && operation.Status == status).ToListAsync(cancellationToken);
    }

    public async Task<List<OrchestrationLifecycleOperation>> GetByOrchestrationIdAndStatusAsync(
            OrchestrationId orchestrationId, OrchestrationLifecycleOperationStatus status, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(orchestrationId);
        var databaseId = await GetOrchestrationDatabaseIdAsync(orchestrationId, cancellationToken);
        if (databaseId is null) return [];
        return await context.OrchestrationLifecycleOperations.Where(operation =>
            EF.Property<int>(operation, "orchestration_database_id") == databaseId.Value &&
                operation.Status == status && !operation.IsDeleted).ToListAsync(cancellationToken);
    }

    public Task<List<OrchestrationLifecycleOperation>> GetPendingAsync(CancellationToken cancellationToken = default) =>
        GetByStatusAsync(OrchestrationLifecycleOperationStatus.Pending, cancellationToken);

    public Task<List<OrchestrationLifecycleOperation>> GetRunningAsync(CancellationToken cancellationToken = default) =>
        GetByStatusAsync(OrchestrationLifecycleOperationStatus.Running, cancellationToken);

    public Task<List<OrchestrationLifecycleOperation>> GetFailedAsync(CancellationToken cancellationToken = default) =>
        GetByStatusAsync(OrchestrationLifecycleOperationStatus.Failed, cancellationToken);

    public async Task<List<OrchestrationLifecycleOperation>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        return await context.OrchestrationLifecycleOperations.Where(operation => operation.IsDeleted).ToListAsync(cancellationToken);
    }

    // Persistence
    public async Task AddAsync(OrchestrationLifecycleOperation operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);
        var databaseId = await GetOrchestrationDatabaseIdAsync(operation.OrchestrationId, cancellationToken) ??
        throw new InvalidOperationException($"Orchestration '{operation.OrchestrationId.Value}' does not exist.");
        var entry = context.Entry(operation);
        entry.Property<int>("orchestration_database_id").CurrentValue = databaseId;
        await context.OrchestrationLifecycleOperations.AddAsync(operation, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(OrchestrationLifecycleOperation operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);
        var databaseId = await GetOrchestrationDatabaseIdAsync(operation.OrchestrationId, cancellationToken) ??
        throw new InvalidOperationException($"Orchestration '{operation.OrchestrationId.Value}' does not exist.");
        var entry = context.Entry(operation);
        if (entry.State == EntityState.Detached)
        {
            context.OrchestrationLifecycleOperations.Update(operation);
            entry = context.Entry(operation);
        }
        entry.Property<int>("orchestration_database_id").CurrentValue = databaseId;
        await context.SaveChangesAsync(cancellationToken);
    }

    // Internal helpers
    private async Task<int?> GetOrchestrationDatabaseIdAsync(OrchestrationId orchestrationId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(orchestrationId);
        return await context.Orchestrations.Where(orchestration => orchestration.Id == orchestrationId && !orchestration.IsDeleted)
            .Select(orchestration => (int?)EF.Property<int>(orchestration, "id")).SingleOrDefaultAsync(cancellationToken);
    }
}

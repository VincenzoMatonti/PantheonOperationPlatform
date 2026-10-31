using Athena.Domain.Orchestration.Management.Entities;
using Athena.Domain.Orchestration.Management.Enums;
using Athena.Domain.Orchestration.Management.Repositories;
using Athena.Domain.Orchestration.Registry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.Infrastructure.Repositories;

public class OrchestrationLifecycleRepository(AthenaDbContext context) : IOrchestrationLifecycleRepository
{
    // Retrieval
    public async Task<OrchestrationLifecycle?> GetByIdAsync(OrchestrationId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        var orchestrationDatabaseId = await GetOrchestrationDatabaseIdAsync(id, cancellationToken);
        if (orchestrationDatabaseId is null) return null;
        return await context.OrchestrationLifecycles.SingleOrDefaultAsync(lifecycle =>
           EF.Property<int>(lifecycle, "orchestration_database_id") == orchestrationDatabaseId.Value && !lifecycle.IsDeleted, cancellationToken);
    }

    public async Task<List<OrchestrationLifecycle>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.OrchestrationLifecycles.Where(lifecycle => !lifecycle.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<OrchestrationLifecycle>> GetByDesiredStateAsync(OrchestrationDesiredState state, CancellationToken cancellationToken = default)
    {
        return await context.OrchestrationLifecycles.Where(lifecycle => !lifecycle.IsDeleted && lifecycle.DesiredState == state).ToListAsync(cancellationToken);
    }

    public async Task<List<OrchestrationLifecycle>> GetByOperationalStatusAsync(
        OrchestrationOperationalStatus status, CancellationToken cancellationToken = default)
    {
        return await context.OrchestrationLifecycles.Where(lifecycle =>
           !lifecycle.IsDeleted && lifecycle.OperationalStatus == status).ToListAsync(cancellationToken);
    }

    // Soft deletion
    public async Task<List<OrchestrationLifecycle>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        return await context.OrchestrationLifecycles.Where(lifecycle => lifecycle.IsDeleted).ToListAsync(cancellationToken);
    }

    // Persistence
    public async Task AddAsync(OrchestrationLifecycle lifecycle, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(lifecycle);
        var orchestrationDatabaseId = await GetOrchestrationDatabaseIdAsync(lifecycle.Id, cancellationToken) ??
        throw new InvalidOperationException($"Orchestration '{lifecycle.Id.Value}' does not exist.");
        var entry = context.Entry(lifecycle);
        entry.Property<int>("orchestration_database_id").CurrentValue = orchestrationDatabaseId;
        await context.OrchestrationLifecycles.AddAsync(lifecycle, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(OrchestrationLifecycle lifecycle, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(lifecycle);
        var orchestrationDatabaseId = await GetOrchestrationDatabaseIdAsync(lifecycle.Id, cancellationToken) ??
        throw new InvalidOperationException($"Orchestration '{lifecycle.Id.Value}' does not exist.");
        var entry = context.Entry(lifecycle);
        if (entry.State == EntityState.Detached)
        {
            context.OrchestrationLifecycles.Update(lifecycle);
            entry = context.Entry(lifecycle);
        }
        entry.Property<int>("orchestration_database_id").CurrentValue = orchestrationDatabaseId;
        await context.SaveChangesAsync(cancellationToken);
    }

    // Internal helpers
    private async Task<int?> GetOrchestrationDatabaseIdAsync(OrchestrationId orchestrationId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(orchestrationId);
        return await context.Orchestrations.Where(orchestration =>
           orchestration.Id == orchestrationId && !orchestration.IsDeleted).Select(orchestration =>
               (int?)EF.Property<int>(orchestration, "id")).SingleOrDefaultAsync(cancellationToken);
    }
}

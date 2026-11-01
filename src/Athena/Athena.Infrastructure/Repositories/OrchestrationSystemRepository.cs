using Athena.Domain.Orchestration.Management.Entities;
using Athena.Domain.Orchestration.Management.Repositories;
using Athena.Domain.Orchestration.Management.ValueObjects;
using Athena.Domain.Orchestration.Registry.ValueObjects;
using Athena.Domain.System.Registry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.Infrastructure.Repositories;

public class OrchestrationSystemInstanceRepository(AthenaDbContext context) : IOrchestrationSystemInstanceRepository
{
    public async Task<OrchestrationSystemInstance?> GetByIdAsync(
        OrchestrationSystemInstanceId id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = context.OrchestrationSystemInstances.AsQueryable();
        if (!includeDeleted) query = query.Where(association => !association.IsDeleted);
        return await query.FirstOrDefaultAsync(association => association.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<OrchestrationSystemInstance>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = context.OrchestrationSystemInstances.AsQueryable();
        if (!includeDeleted) query = query.Where(association => !association.IsDeleted);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<OrchestrationSystemInstance>> GetByOrchestrationIdAsync(
        OrchestrationId orchestrationId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(orchestrationId);
        var query = context.OrchestrationSystemInstances.AsQueryable();
        if (!includeDeleted) query = query.Where(association => !association.IsDeleted);
        return await query.Where(association => association.OrchestrationId == orchestrationId).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<OrchestrationSystemInstance>> GetBySystemInstanceIdAsync(
        SystemInstanceId systemId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(systemId);
        var query = context.OrchestrationSystemInstances.AsQueryable();
        if (!includeDeleted) query = query.Where(association => !association.IsDeleted);
        return await query.Where(association => association.SystemInstanceId == systemId).ToListAsync(cancellationToken);
    }

    public async Task<OrchestrationSystemInstance?> GetByOrchestrationAndSystemInstanceAsync(
        OrchestrationId orchestrationId, SystemInstanceId systemInstanceId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(orchestrationId);
        ArgumentNullException.ThrowIfNull(systemInstanceId);
        var query = context.OrchestrationSystemInstances.AsQueryable();
        if (!includeDeleted) query = query.Where(association => !association.IsDeleted);
        return await query.FirstOrDefaultAsync(
            association => association.OrchestrationId == orchestrationId && association.SystemInstanceId == systemInstanceId, cancellationToken);
    }

    public async Task<bool> ExistsAsync(OrchestrationSystemInstanceId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        return await context.OrchestrationSystemInstances.AnyAsync(association => association.Id == id && !association.IsDeleted, cancellationToken);
    }

    public async Task<bool> AssociationExistsAsync(
        OrchestrationId orchestrationId, SystemInstanceId systemId, bool includeDeleted = true, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(orchestrationId);
        ArgumentNullException.ThrowIfNull(systemId);
        var query = context.OrchestrationSystemInstances.AsQueryable();
        if (!includeDeleted) query = query.Where(association => !association.IsDeleted);
        return await query.AnyAsync(association => association.OrchestrationId == orchestrationId && association.SystemInstanceId == systemId, cancellationToken);
    }

    public async Task AddAsync(OrchestrationSystemInstance association, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(association);
        var orchestrationDatabaseId = await GetOrchestrationDatabaseIdAsync(association.OrchestrationId, cancellationToken) ??
            throw new InvalidOperationException($"Orchestration '{association.OrchestrationId.Value}' does not exist.");
        var systemInstanceDatabaseId = await GetSystemInstanceDatabaseIdAsync(association.SystemInstanceId, cancellationToken) ??
            throw new InvalidOperationException($"SystemInstance '{association.SystemInstanceId.Value}' does not exist.");
        await context.OrchestrationSystemInstances.AddAsync(association, cancellationToken);
        var entry = context.Entry(association);
        entry.Property<int>("orchestration_database_id").CurrentValue = orchestrationDatabaseId;
        entry.Property<int>("system_instance_database_id").CurrentValue = systemInstanceDatabaseId;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(OrchestrationSystemInstance association, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(association);
        var orchestrationDatabaseId = await GetOrchestrationDatabaseIdAsync(association.OrchestrationId, cancellationToken) ??
            throw new InvalidOperationException($"Orchestration '{association.OrchestrationId.Value}' does not exist.");

        var systemInstanceDatabaseId = await GetSystemInstanceDatabaseIdAsync(association.SystemInstanceId, cancellationToken) ??
            throw new InvalidOperationException($"SystemInstance '{association.SystemInstanceId.Value}' does not exist.");

        var entry = context.Entry(association);
        if (entry.State == EntityState.Detached)
        {
            context.OrchestrationSystemInstances.Update(association);
            entry = context.Entry(association);
        }

        entry.Property<int>("orchestration_database_id").CurrentValue = orchestrationDatabaseId;
        entry.Property<int>("system_instance_database_id").CurrentValue = systemInstanceDatabaseId;
        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task<int?> GetOrchestrationDatabaseIdAsync(OrchestrationId orchestrationId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(orchestrationId);
        return await context.Orchestrations
            .Where(orchestration => orchestration.Id == orchestrationId && !orchestration.IsDeleted)
            .Select(orchestration => (int?)EF.Property<int>(orchestration, "id")).SingleOrDefaultAsync(cancellationToken);
    }

    private async Task<int?> GetSystemInstanceDatabaseIdAsync(SystemInstanceId systemInstanceId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(systemInstanceId);
        return await context.SystemInstances
            .Where(instance => instance.Id == systemInstanceId && !instance.IsDeleted)
            .Select(instance => (int?)EF.Property<int>(instance, "id")).SingleOrDefaultAsync(cancellationToken);
    }
}

using Athena.Domain.OrchestrationManagement.Entities;
using Athena.Domain.OrchestrationManagement.Repositories;
using Athena.Domain.OrchestrationManagement.ValueObjects;
using Athena.Domain.OrchestrationRegistry.ValueObjects;
using Athena.Domain.SystemRegistry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.Infrastructure.Repositories;

public class OrchestrationSystemRepository(AthenaDbContext context) : IOrchestrationSystemRepository
{
    public async Task<OrchestrationSystem?> GetByIdAsync(
        OrchestrationSystemId id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = context.OrchestrationSystems.AsQueryable();
        if (!includeDeleted) query = query.Where(association => !association.IsDeleted);
        return await query.FirstOrDefaultAsync(association => association.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<OrchestrationSystem>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = context.OrchestrationSystems.AsQueryable();
        if (!includeDeleted) query = query.Where(association => !association.IsDeleted);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<OrchestrationSystem>> GetByOrchestrationIdAsync(
        OrchestrationId orchestrationId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(orchestrationId);
        var query = context.OrchestrationSystems.AsQueryable();
        if (!includeDeleted) query = query.Where(association => !association.IsDeleted);
        return await query.Where(association => association.OrchestrationId == orchestrationId).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<OrchestrationSystem>> GetBySystemIdAsync(
        SystemId systemId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(systemId);
        var query = context.OrchestrationSystems.AsQueryable();
        if (!includeDeleted) query = query.Where(association => !association.IsDeleted);
        return await query.Where(association => association.SystemId == systemId).ToListAsync(cancellationToken);
    }

    public async Task<OrchestrationSystem?> GetByOrchestrationAndSystemAsync(
        OrchestrationId orchestrationId, SystemId systemId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(orchestrationId);
        ArgumentNullException.ThrowIfNull(systemId);
        var query = context.OrchestrationSystems.AsQueryable();
        if (!includeDeleted) query = query.Where(association => !association.IsDeleted);
        return await query.FirstOrDefaultAsync(
            association => association.OrchestrationId == orchestrationId && association.SystemId == systemId, cancellationToken);
    }

    public async Task<bool> ExistsAsync(OrchestrationSystemId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        return await context.OrchestrationSystems.AnyAsync(association => association.Id == id && !association.IsDeleted, cancellationToken);
    }

    public async Task<bool> AssociationExistsAsync(
        OrchestrationId orchestrationId, SystemId systemId, bool includeDeleted = true, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(orchestrationId);
        ArgumentNullException.ThrowIfNull(systemId);
        var query = context.OrchestrationSystems.AsQueryable();
        if (!includeDeleted) query = query.Where(association => !association.IsDeleted);
        return await query.AnyAsync(association => association.OrchestrationId == orchestrationId && association.SystemId == systemId, cancellationToken);
    }

    public async Task AddAsync(OrchestrationSystem association, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(association);
        var orchestrationDatabaseId = await GetOrchestrationDatabaseIdAsync(association.OrchestrationId, cancellationToken) ??
            throw new InvalidOperationException($"Orchestration '{association.OrchestrationId.Value}' does not exist.");
        var systemDatabaseId = await GetSystemDatabaseIdAsync(association.SystemId, cancellationToken) ??
            throw new InvalidOperationException($"System '{association.SystemId.Value}' does not exist.");
        await context.OrchestrationSystems.AddAsync(association, cancellationToken);
        var entry = context.Entry(association);
        entry.Property<int>("orchestration_database_id").CurrentValue = orchestrationDatabaseId;
        entry.Property<int>("system_database_id").CurrentValue = systemDatabaseId;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(OrchestrationSystem association, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(association);
        var orchestrationDatabaseId = await GetOrchestrationDatabaseIdAsync(association.OrchestrationId, cancellationToken) ??
            throw new InvalidOperationException($"Orchestration '{association.OrchestrationId.Value}' does not exist.");

        var systemDatabaseId = await GetSystemDatabaseIdAsync(association.SystemId, cancellationToken) ??
            throw new InvalidOperationException($"System '{association.SystemId.Value}' does not exist.");

        var entry = context.Entry(association);
        if (entry.State == EntityState.Detached)
        {
            context.OrchestrationSystems.Update(association);
            entry = context.Entry(association);
        }

        entry.Property<int>("orchestration_database_id").CurrentValue = orchestrationDatabaseId;
        entry.Property<int>("system_database_id").CurrentValue = systemDatabaseId;
        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task<int?> GetOrchestrationDatabaseIdAsync(OrchestrationId orchestrationId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(orchestrationId);
        return await context.Orchestrations
            .Where(orchestration => orchestration.Id == orchestrationId && !orchestration.IsDeleted)
            .Select(orchestration => (int?)EF.Property<int>(orchestration, "id")).SingleOrDefaultAsync(cancellationToken);
    }

    private async Task<int?> GetSystemDatabaseIdAsync(SystemId systemId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(systemId);
        return await context.Systems
            .Where(system => system.Id == systemId && !system.IsDeleted)
            .Select(system => (int?)EF.Property<int>(system, "id")).SingleOrDefaultAsync(cancellationToken);
    }
}

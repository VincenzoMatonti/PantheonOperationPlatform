using Athena.Domain.OrchestrationManagement.Entities;
using Athena.Domain.OrchestrationManagement.Repositories;
using Athena.Domain.OrchestrationManagement.ValueObjects;
using Athena.Domain.OrchestrationRegistry.ValueObjects;
using Athena.Domain.SystemRegistry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.Infrastructure.Repositories;

public sealed class OrchestrationSystemRepository(AthenaDbContext context) : IOrchestrationSystemRepository
{
    public async Task<OrchestrationSystem?> GetByIdAsync(OrchestrationSystemId id, bool includeDeleted = false, CancellationToken cancellationToken = default)
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
        var query = context.OrchestrationSystems.AsQueryable();
        if (!includeDeleted) query = query.Where(association => !association.IsDeleted);
        return await query.Where(association => association.OrchestrationId == orchestrationId).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<OrchestrationSystem>> GetBySystemIdAsync(
        SystemId systemId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = context.OrchestrationSystems.AsQueryable();
        if (!includeDeleted) query = query.Where(association => !association.IsDeleted);
        return await query.Where(association => association.SystemId == systemId).ToListAsync(cancellationToken);
    }

    public async Task<OrchestrationSystem?> GetByOrchestrationAndSystemAsync(
        OrchestrationId orchestrationId, SystemId systemId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = context.OrchestrationSystems.AsQueryable();
        if (!includeDeleted) query = query.Where(association => !association.IsDeleted);
        return await query.FirstOrDefaultAsync(association => association.OrchestrationId == orchestrationId && association.SystemId == systemId, cancellationToken);
    }

    public async Task<bool> ExistsAsync(OrchestrationSystemId id, CancellationToken cancellationToken = default)
    {
        return await context.OrchestrationSystems.AnyAsync(association => association.Id == id && !association.IsDeleted, cancellationToken);
    }

    public async Task<bool> AssociationExistsAsync(
        OrchestrationId orchestrationId, SystemId systemId, bool includeDeleted = true, CancellationToken cancellationToken = default)
    {
        var query = context.OrchestrationSystems.AsQueryable();
        if (!includeDeleted) query = query.Where(association => !association.IsDeleted);
        return await query.AnyAsync(association => association.OrchestrationId == orchestrationId && association.SystemId == systemId, cancellationToken);
    }

    public async Task AddAsync(OrchestrationSystem association, CancellationToken cancellationToken = default)
    {
        await context.OrchestrationSystems.AddAsync(association, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(OrchestrationSystem association, CancellationToken cancellationToken = default)
    {
        context.OrchestrationSystems.Update(association); await context.SaveChangesAsync(cancellationToken);
    }

}

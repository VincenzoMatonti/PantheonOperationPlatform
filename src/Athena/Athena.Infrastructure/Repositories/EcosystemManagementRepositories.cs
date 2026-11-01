using Athena.Domain.Ecosystem.Management.Entities;
using Athena.Domain.Ecosystem.Management.Repositories;
using Athena.Domain.Ecosystem.Registry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.Infrastructure.Repositories;

public sealed class EcosystemLifecycleRepository(AthenaDbContext context) : IEcosystemLifecycleRepository
{
    public async Task<EcosystemLifecycle?> GetByIdAsync(EcosystemId id, CancellationToken cancellationToken = default)
    {
        var dbId = await FindDatabaseIdAsync(id, cancellationToken);
        if (dbId is null) return null;
        return await context.EcosystemLifecycles.SingleOrDefaultAsync(x => EF.Property<int>(x, "ecosystem_database_id") == dbId && !x.IsDeleted, cancellationToken);
    }
    public async Task<IReadOnlyList<EcosystemLifecycle>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await context.EcosystemLifecycles.Where(x => !x.IsDeleted).ToListAsync(cancellationToken);
    public async Task AddAsync(EcosystemLifecycle lifecycle, CancellationToken cancellationToken = default)
    {
        var dbId = await FindDatabaseIdAsync(lifecycle.Id, cancellationToken) ?? throw new InvalidOperationException($"Ecosystem '{lifecycle.Id.Value}' does not exist.");
        context.Entry(lifecycle).Property<int>("ecosystem_database_id").CurrentValue = dbId;
        await context.EcosystemLifecycles.AddAsync(lifecycle, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task UpdateAsync(EcosystemLifecycle lifecycle, CancellationToken cancellationToken = default)
    {
        var dbId = await FindDatabaseIdAsync(lifecycle.Id, cancellationToken) ?? throw new InvalidOperationException($"Ecosystem '{lifecycle.Id.Value}' does not exist.");
        var entry = context.Entry(lifecycle);
        if (entry.State == EntityState.Detached) { context.EcosystemLifecycles.Update(lifecycle); entry = context.Entry(lifecycle); }
        entry.Property<int>("ecosystem_database_id").CurrentValue = dbId;
        await context.SaveChangesAsync(cancellationToken);
    }
    private Task<int?> FindDatabaseIdAsync(EcosystemId id, CancellationToken token) =>
        context.Ecosystems.Where(x => x.Id == id).Select(x => (int?)EF.Property<int>(x, "id")).SingleOrDefaultAsync(token);
}

public sealed class EcosystemOperationRepository(AthenaDbContext context) : IEcosystemOperationRepository
{
    public async Task AddAsync(EcosystemOperation operation, CancellationToken cancellationToken = default)
    {
        var dbId = await context.Ecosystems.Where(x => x.Id == operation.EcosystemId)
            .Select(x => (int?)EF.Property<int>(x, "id")).SingleOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Ecosystem '{operation.EcosystemId.Value}' does not exist.");
        context.Entry(operation).Property<int>("ecosystem_database_id").CurrentValue = dbId;
        await context.EcosystemOperations.AddAsync(operation, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task UpdateAsync(EcosystemOperation operation, CancellationToken cancellationToken = default)
    {
        var dbId = await context.Ecosystems.Where(x => x.Id == operation.EcosystemId)
            .Select(x => (int?)EF.Property<int>(x, "id")).SingleOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Ecosystem '{operation.EcosystemId.Value}' does not exist.");
        var entry = context.Entry(operation);
        if (entry.State == EntityState.Detached) { context.EcosystemOperations.Update(operation); entry = context.Entry(operation); }
        entry.Property<int>("ecosystem_database_id").CurrentValue = dbId;
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task<IReadOnlyList<EcosystemOperation>> GetByEcosystemIdAsync(EcosystemId id, CancellationToken cancellationToken = default) =>
        await context.EcosystemOperations.AsNoTracking().Where(x => x.EcosystemId == id)
            .OrderByDescending(x => x.CreatedAtUtc).ToListAsync(cancellationToken);
}

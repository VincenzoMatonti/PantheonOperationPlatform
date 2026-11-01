using Athena.Domain.Workload.Management.Entities;
using Athena.Domain.Workload.Management.Repositories;
using Athena.Domain.Workload.Registry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.Infrastructure.Repositories;

public sealed class WorkloadLifecycleRepository(AthenaDbContext context) : IWorkloadLifecycleRepository
{
    public async Task<WorkloadLifecycle?> GetByIdAsync(WorkloadId id, CancellationToken cancellationToken = default)
    {
        var dbId = await FindDatabaseIdAsync(id, cancellationToken);
        if (dbId is null) return null;
        return await context.WorkloadLifecycles.SingleOrDefaultAsync(x => EF.Property<int>(x, "workload_database_id") == dbId, cancellationToken);
    }
    public async Task<IReadOnlyList<WorkloadLifecycle>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await context.WorkloadLifecycles.Where(x => !x.IsDeleted).ToListAsync(cancellationToken);
    public async Task AddAsync(WorkloadLifecycle lifecycle, CancellationToken cancellationToken = default)
    {
        var dbId = await FindDatabaseIdAsync(lifecycle.Id, cancellationToken) ?? throw new InvalidOperationException($"Workload '{lifecycle.Id.Value}' does not exist.");
        context.Entry(lifecycle).Property<int>("workload_database_id").CurrentValue = dbId;
        await context.WorkloadLifecycles.AddAsync(lifecycle, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task UpdateAsync(WorkloadLifecycle lifecycle, CancellationToken cancellationToken = default)
    {
        var dbId = await FindDatabaseIdAsync(lifecycle.Id, cancellationToken) ?? throw new InvalidOperationException($"Workload '{lifecycle.Id.Value}' does not exist.");
        var entry = context.Entry(lifecycle);
        if (entry.State == EntityState.Detached) { context.WorkloadLifecycles.Update(lifecycle); entry = context.Entry(lifecycle); }
        entry.Property<int>("workload_database_id").CurrentValue = dbId;
        await context.SaveChangesAsync(cancellationToken);
    }
    private Task<int?> FindDatabaseIdAsync(WorkloadId id, CancellationToken token) =>
        context.Workloads.Where(x => x.Id == id).Select(x => (int?)EF.Property<int>(x, "id")).SingleOrDefaultAsync(token);
}

public sealed class WorkloadOperationRepository(AthenaDbContext context) : IWorkloadOperationRepository
{
    public async Task AddAsync(WorkloadOperation operation, CancellationToken cancellationToken = default)
    {
        var dbId = await context.Workloads.Where(x => x.Id == operation.WorkloadId)
            .Select(x => (int?)EF.Property<int>(x, "id")).SingleOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Workload '{operation.WorkloadId.Value}' does not exist.");
        context.Entry(operation).Property<int>("workload_database_id").CurrentValue = dbId;
        await context.WorkloadOperations.AddAsync(operation, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task SaveIntentAsync(
        WorkloadOperation operation,
        WorkloadLifecycle lifecycle,
        CancellationToken cancellationToken = default)
    {
        if (operation.WorkloadId != lifecycle.Id)
            throw new ArgumentException("The lifecycle must belong to the operation's Workload.", nameof(lifecycle));

        var dbId = await context.Workloads.Where(x => x.Id == operation.WorkloadId)
            .Select(x => (int?)EF.Property<int>(x, "id")).SingleOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Workload '{operation.WorkloadId.Value}' does not exist.");
        context.Entry(operation).Property<int>("workload_database_id").CurrentValue = dbId;
        var lifecycleEntry = context.Entry(lifecycle);
        if (lifecycleEntry.State == EntityState.Detached)
        {
            context.WorkloadLifecycles.Update(lifecycle);
            lifecycleEntry = context.Entry(lifecycle);
        }
        lifecycleEntry.Property<int>("workload_database_id").CurrentValue = dbId;
        await context.WorkloadOperations.AddAsync(operation, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task UpdateAsync(WorkloadOperation operation, CancellationToken cancellationToken = default)
    {
        var dbId = await context.Workloads.Where(x => x.Id == operation.WorkloadId)
            .Select(x => (int?)EF.Property<int>(x, "id")).SingleOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Workload '{operation.WorkloadId.Value}' does not exist.");
        var entry = context.Entry(operation);
        if (entry.State == EntityState.Detached) { context.WorkloadOperations.Update(operation); entry = context.Entry(operation); }
        entry.Property<int>("workload_database_id").CurrentValue = dbId;
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task<IReadOnlyList<WorkloadOperation>> GetByWorkloadIdAsync(WorkloadId id, CancellationToken cancellationToken = default) =>
        await context.WorkloadOperations.AsNoTracking().Where(x => x.WorkloadId == id)
            .OrderByDescending(x => x.CreatedAtUtc).ToListAsync(cancellationToken);
}

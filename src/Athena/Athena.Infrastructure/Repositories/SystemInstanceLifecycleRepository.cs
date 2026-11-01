using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Management.Enums;
using Athena.Domain.System.Management.Repositories;
using Athena.Domain.System.Registry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.Infrastructure.Repositories;

public sealed class SystemInstanceLifecycleRepository(AthenaDbContext context) : ISystemInstanceLifecycleRepository
{
    public async Task<SystemInstanceLifecycle?> GetByIdAsync(SystemInstanceId id, CancellationToken cancellationToken = default)
    {
        var databaseId = await FindDatabaseIdAsync(id, cancellationToken);
        if (databaseId is null) return null;
        return await context.SystemInstanceLifecycles.SingleOrDefaultAsync(
            lifecycle => EF.Property<int>(lifecycle, "system_instance_database_id") == databaseId.Value && !lifecycle.IsDeleted,
            cancellationToken);
    }

    public async Task<IReadOnlyList<SystemInstanceLifecycle>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await context.SystemInstanceLifecycles.Where(lifecycle => !lifecycle.IsDeleted).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<SystemInstanceLifecycle>> GetByDesiredStateAsync(
        SystemInstanceDesiredState state, CancellationToken cancellationToken = default) =>
        await context.SystemInstanceLifecycles.Where(lifecycle => !lifecycle.IsDeleted && lifecycle.DesiredState == state).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<SystemInstanceLifecycle>> GetByOperationalStatusAsync(
        SystemInstanceOperationalStatus status, CancellationToken cancellationToken = default) =>
        await context.SystemInstanceLifecycles.Where(lifecycle => !lifecycle.IsDeleted && lifecycle.OperationalStatus == status).ToListAsync(cancellationToken);

    public async Task AddAsync(SystemInstanceLifecycle lifecycle, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(lifecycle);
        var databaseId = await FindDatabaseIdAsync(lifecycle.Id, cancellationToken)
            ?? throw new InvalidOperationException($"SystemInstance '{lifecycle.Id.Value}' does not exist.");
        context.Entry(lifecycle).Property<int>("system_instance_database_id").CurrentValue = databaseId;
        await context.SystemInstanceLifecycles.AddAsync(lifecycle, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(SystemInstanceLifecycle lifecycle, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(lifecycle);
        var databaseId = await FindDatabaseIdAsync(lifecycle.Id, cancellationToken)
            ?? throw new InvalidOperationException($"SystemInstance '{lifecycle.Id.Value}' does not exist.");
        var entry = context.Entry(lifecycle);
        if (entry.State == EntityState.Detached)
        {
            context.SystemInstanceLifecycles.Update(lifecycle);
            entry = context.Entry(lifecycle);
        }
        entry.Property<int>("system_instance_database_id").CurrentValue = databaseId;
        await context.SaveChangesAsync(cancellationToken);
    }

    private Task<int?> FindDatabaseIdAsync(SystemInstanceId id, CancellationToken cancellationToken) =>
        context.SystemInstances.Where(instance => instance.Id == id)
            .Select(instance => (int?)EF.Property<int>(instance, "id"))
            .SingleOrDefaultAsync(cancellationToken);
}

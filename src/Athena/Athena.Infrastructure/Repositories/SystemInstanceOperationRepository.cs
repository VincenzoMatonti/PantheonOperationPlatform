using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Management.Enums;
using Athena.Domain.System.Management.Repositories;
using Athena.Domain.System.Registry.Entities;
using Athena.Domain.System.Registry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.Infrastructure.Repositories;

public sealed class SystemInstanceOperationRepository(AthenaDbContext context) : ISystemInstanceOperationRepository
{
    public async Task AddAsync(SystemInstanceOperation operation, CancellationToken cancellationToken = default)
    {
        var systemInstanceDatabaseId = await context.SystemInstances
            .Where(instance => instance.Id == operation.SystemInstanceId)
            .Select(instance => (int?)EF.Property<int>(instance, "id"))
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"SystemInstance '{operation.SystemInstanceId.Value}' does not exist.");

        await context.SystemInstanceOperations.AddAsync(operation, cancellationToken);
        context.Entry(operation).Property<int>("system_instance_database_id").CurrentValue = systemInstanceDatabaseId;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddWithLifecycleAsync(
        SystemInstanceOperation operation,
        SystemInstanceLifecycle lifecycle,
        CancellationToken cancellationToken = default)
    {
        if (operation.SystemInstanceId != lifecycle.Id)
            throw new ArgumentException("The lifecycle must belong to the operation's SystemInstance.", nameof(lifecycle));

        var systemInstanceDatabaseId = await FindSystemInstanceDatabaseIdAsync(operation.SystemInstanceId, cancellationToken);
        await context.SystemInstanceOperations.AddAsync(operation, cancellationToken);
        context.Entry(operation).Property<int>("system_instance_database_id").CurrentValue = systemInstanceDatabaseId;
        PrepareLifecycleUpdate(lifecycle, systemInstanceDatabaseId);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveWithLifecycleAsync(
        SystemInstanceOperation operation,
        SystemInstanceLifecycle lifecycle,
        SystemInstance? systemInstance = null,
        CancellationToken cancellationToken = default)
    {
        if (operation.SystemInstanceId != lifecycle.Id || systemInstance is not null && systemInstance.Id != lifecycle.Id)
            throw new ArgumentException("The operation, lifecycle, and optional SystemInstance must have the same identifier.");

        var systemInstanceDatabaseId = await FindSystemInstanceDatabaseIdAsync(operation.SystemInstanceId, cancellationToken);
        var operationEntry = context.Entry(operation);
        if (operationEntry.State == EntityState.Detached) context.SystemInstanceOperations.Update(operation);
        context.Entry(operation).Property<int>("system_instance_database_id").CurrentValue = systemInstanceDatabaseId;
        PrepareLifecycleUpdate(lifecycle, systemInstanceDatabaseId);
        if (systemInstance is not null)
        {
            var instanceEntry = context.Entry(systemInstance);
            if (instanceEntry.State == EntityState.Detached) context.SystemInstances.Update(systemInstance);
        }
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(SystemInstanceOperation operation, CancellationToken cancellationToken = default)
    {
        var entry = context.Entry(operation);
        if (entry.State == EntityState.Detached) context.SystemInstanceOperations.Update(operation);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SystemInstanceOperation>> GetBySystemInstanceIdAsync(
        SystemInstanceId systemInstanceId,
        CancellationToken cancellationToken = default) =>
        await context.SystemInstanceOperations
            .AsNoTracking()
            .Where(operation => operation.SystemInstanceId == systemInstanceId)
            .OrderByDescending(operation => operation.CreatedAtUtc)
            .ToListAsync(cancellationToken);

    public Task<bool> HasActiveOperationAsync(SystemInstanceId systemInstanceId, CancellationToken cancellationToken = default) =>
        context.SystemInstanceOperations.AnyAsync(
            operation => operation.SystemInstanceId == systemInstanceId
                && (operation.Status == SystemInstanceOperationStatus.Pending
                    || operation.Status == SystemInstanceOperationStatus.Running),
            cancellationToken);

    private async Task<int> FindSystemInstanceDatabaseIdAsync(SystemInstanceId id, CancellationToken cancellationToken) =>
        await context.SystemInstances
            .Where(instance => instance.Id == id)
            .Select(instance => (int?)EF.Property<int>(instance, "id"))
            .SingleOrDefaultAsync(cancellationToken)
        ?? throw new InvalidOperationException($"SystemInstance '{id.Value}' does not exist.");

    private void PrepareLifecycleUpdate(SystemInstanceLifecycle lifecycle, int systemInstanceDatabaseId)
    {
        var lifecycleEntry = context.Entry(lifecycle);
        if (lifecycleEntry.State == EntityState.Detached)
        {
            context.SystemInstanceLifecycles.Update(lifecycle);
            lifecycleEntry = context.Entry(lifecycle);
        }
        lifecycleEntry.Property<int>("system_instance_database_id").CurrentValue = systemInstanceDatabaseId;
    }
}

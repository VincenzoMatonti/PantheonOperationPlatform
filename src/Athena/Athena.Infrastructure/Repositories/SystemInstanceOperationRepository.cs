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

    public async Task AddWithRegistryAsync(
        SystemInstanceOperation operation,
        SystemInstance systemInstance,
        SystemInstanceLifecycle lifecycle,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentNullException.ThrowIfNull(systemInstance);
        ArgumentNullException.ThrowIfNull(lifecycle);
        if (operation.SystemInstanceId != systemInstance.Id || systemInstance.Id != lifecycle.Id)
            throw new ArgumentException("The operation, registry entity, and lifecycle must share the same SystemInstance identifier.");

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        var systemDatabaseId = await context.Systems
            .Where(system => system.Id == systemInstance.SystemId && !system.IsDeleted)
            .Select(system => (int?)EF.Property<int>(system, "id"))
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"System '{systemInstance.SystemId.Value}' does not exist.");
        var workloadDatabaseId = await context.Workloads
            .Where(workload => workload.Id == systemInstance.WorkloadId && !workload.IsDeleted)
            .Select(workload => (int?)EF.Property<int>(workload, "id"))
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Workload '{systemInstance.WorkloadId.Value}' does not exist.");

        await context.SystemInstances.AddAsync(systemInstance, cancellationToken);
        var instanceEntry = context.Entry(systemInstance);
        instanceEntry.Property<int>("system_database_id").CurrentValue = systemDatabaseId;
        instanceEntry.Property<int>("workload_database_id").CurrentValue = workloadDatabaseId;
        await context.SaveChangesAsync(cancellationToken);

        var instanceDatabaseId = await context.SystemInstances
            .Where(instance => instance.Id == systemInstance.Id)
            .Select(instance => EF.Property<int>(instance, "id"))
            .SingleAsync(cancellationToken);
        context.Entry(lifecycle).Property<int>("system_instance_database_id").CurrentValue = instanceDatabaseId;
        context.Entry(operation).Property<int>("system_instance_database_id").CurrentValue = instanceDatabaseId;
        await context.SystemInstanceLifecycles.AddAsync(lifecycle, cancellationToken);
        await context.SystemInstanceOperations.AddAsync(operation, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
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
        if (entry.State == EntityState.Detached)
        {
            context.SystemInstanceOperations.Update(operation);
            entry = context.Entry(operation);
            entry.Property(candidate => candidate.Version).OriginalValue = operation.Version - 1;
        }
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
                    || operation.Status == SystemInstanceOperationStatus.Running
                    || operation.Status == SystemInstanceOperationStatus.OutcomeUnknown),
            cancellationToken);

    public Task<SystemInstanceOperation?> GetByIdAsync(Guid operationId, CancellationToken cancellationToken = default) =>
        context.SystemInstanceOperations.AsNoTracking()
            .SingleOrDefaultAsync(operation => operation.Id == operationId, cancellationToken);

    public async Task<SystemInstanceOperation?> ClaimNextAsync(
        string leaseOwner,
        TimeSpan leaseDuration,
        DateTime nowUtc,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(leaseOwner);
        if (leaseDuration <= TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(leaseDuration));
        if (nowUtc.Kind != DateTimeKind.Utc) throw new ArgumentException("Claim time must be UTC.", nameof(nowUtc));

        while (true)
        {
            var candidate = await context.SystemInstanceOperations
                .AsNoTracking()
                .Where(operation => operation.Status == SystemInstanceOperationStatus.Pending
                    || operation.Status == SystemInstanceOperationStatus.Running
                    || operation.Status == SystemInstanceOperationStatus.OutcomeUnknown)
                .Where(operation => operation.NextAttemptAtUtc == null || operation.NextAttemptAtUtc <= nowUtc)
                .Where(operation => operation.LeaseExpiresAtUtc == null || operation.LeaseExpiresAtUtc <= nowUtc)
                .OrderBy(operation => operation.CreatedAtUtc)
                .ThenBy(operation => operation.Id)
                .Select(operation => new { operation.Id, operation.Version })
                .FirstOrDefaultAsync(cancellationToken);

            if (candidate is null) return null;

            var claimed = await context.SystemInstanceOperations
                .Where(operation => operation.Id == candidate.Id
                    && operation.Version == candidate.Version
                    && (operation.Status == SystemInstanceOperationStatus.Pending
                        || operation.Status == SystemInstanceOperationStatus.Running
                        || operation.Status == SystemInstanceOperationStatus.OutcomeUnknown)
                    && (operation.NextAttemptAtUtc == null || operation.NextAttemptAtUtc <= nowUtc)
                    && (operation.LeaseExpiresAtUtc == null || operation.LeaseExpiresAtUtc <= nowUtc))
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(operation => operation.LeaseOwner, leaseOwner)
                    .SetProperty(operation => operation.LeaseExpiresAtUtc, nowUtc + leaseDuration)
                    .SetProperty(operation => operation.LastAttemptAtUtc, nowUtc)
                    .SetProperty(operation => operation.AttemptCount, operation => operation.AttemptCount + 1)
                    .SetProperty(operation => operation.Version, operation => operation.Version + 1),
                    cancellationToken);

            if (claimed == 1) return await GetByIdAsync(candidate.Id, cancellationToken);
        }
    }

    public async Task<bool> SaveIfLeaseOwnerAsync(
        SystemInstanceOperation operation,
        string leaseOwner,
        CancellationToken cancellationToken = default)
    {
        return await SaveIfLeaseOwnerCoreAsync(operation, leaseOwner, cancellationToken) is not null;
    }

    public async Task<bool> SaveWithLifecycleIfLeaseOwnerAsync(
        SystemInstanceOperation operation,
        SystemInstanceLifecycle lifecycle,
        SystemInstance? systemInstance,
        string leaseOwner,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(lifecycle);
        if (operation.SystemInstanceId != lifecycle.Id || systemInstance is not null && operation.SystemInstanceId != systemInstance.Id)
            throw new ArgumentException("The lifecycle and optional registry entity must belong to the operation's SystemInstance.", nameof(lifecycle));

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        var databaseId = await SaveIfLeaseOwnerCoreAsync(operation, leaseOwner, cancellationToken);
        if (databaseId is null)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }

        PrepareLifecycleUpdate(lifecycle, databaseId.Value);
        if (systemInstance is not null)
        {
            var instanceEntry = context.Entry(systemInstance);
            if (instanceEntry.State == EntityState.Detached) context.SystemInstances.Update(systemInstance);
        }
        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return true;
    }

    private async Task<int?> SaveIfLeaseOwnerCoreAsync(
        SystemInstanceOperation operation,
        string leaseOwner,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentException.ThrowIfNullOrWhiteSpace(leaseOwner);

        var databaseId = await context.SystemInstanceOperations
            .Where(candidate => candidate.Id == operation.Id
                && (candidate.Status == SystemInstanceOperationStatus.Pending
                    || candidate.Status == SystemInstanceOperationStatus.Running
                    || candidate.Status == SystemInstanceOperationStatus.OutcomeUnknown)
                && candidate.LeaseOwner == leaseOwner
                && candidate.LeaseExpiresAtUtc > DateTime.UtcNow)
            .Select(candidate => (int?)EF.Property<int>(candidate, "system_instance_database_id"))
            .SingleOrDefaultAsync(cancellationToken);
        if (databaseId is null) return null;

        var affected = await context.SystemInstanceOperations
            .Where(candidate => candidate.Id == operation.Id
                && (candidate.Status == SystemInstanceOperationStatus.Pending
                    || candidate.Status == SystemInstanceOperationStatus.Running
                    || candidate.Status == SystemInstanceOperationStatus.OutcomeUnknown)
                && candidate.LeaseOwner == leaseOwner
                && candidate.LeaseExpiresAtUtc > DateTime.UtcNow)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(candidate => candidate.Status, operation.Status)
                .SetProperty(candidate => candidate.CommandPayloadJson, operation.CommandPayloadJson)
                .SetProperty(candidate => candidate.StartedAtUtc, operation.StartedAtUtc)
                .SetProperty(candidate => candidate.CompletedAtUtc, operation.CompletedAtUtc)
                .SetProperty(candidate => candidate.ErrorMessage, operation.ErrorMessage)
                .SetProperty(candidate => candidate.AgentAcceptedAtUtc, operation.AgentAcceptedAtUtc)
                .SetProperty(candidate => candidate.AgentCompletedAtUtc, operation.AgentCompletedAtUtc)
                .SetProperty(candidate => candidate.CommandDispatchedAtUtc, operation.CommandDispatchedAtUtc)
                .SetProperty(candidate => candidate.LastAttemptAtUtc, operation.LastAttemptAtUtc)
                .SetProperty(candidate => candidate.NextAttemptAtUtc, operation.NextAttemptAtUtc)
                .SetProperty(candidate => candidate.AttemptCount, operation.AttemptCount)
                .SetProperty(candidate => candidate.Version, candidate => candidate.Version + 1)
                .SetProperty(candidate => candidate.LeaseOwner, operation.LeaseOwner)
                .SetProperty(candidate => candidate.LeaseExpiresAtUtc, operation.LeaseExpiresAtUtc),
                cancellationToken);
        return affected == 1 ? databaseId : null;
    }

    public async Task<bool> RenewLeaseAsync(
        Guid operationId,
        string leaseOwner,
        TimeSpan leaseDuration,
        DateTime nowUtc,
        CancellationToken cancellationToken = default)
    {
        if (leaseDuration <= TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(leaseDuration));
        if (nowUtc.Kind != DateTimeKind.Utc) throw new ArgumentException("Renewal time must be UTC.", nameof(nowUtc));
        var affected = await context.SystemInstanceOperations
            .Where(operation => operation.Id == operationId
                && operation.IsActive
                && operation.LeaseOwner == leaseOwner
                && operation.LeaseExpiresAtUtc > nowUtc)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(operation => operation.LeaseExpiresAtUtc, nowUtc + leaseDuration)
                .SetProperty(operation => operation.Version, operation => operation.Version + 1),
                cancellationToken);
        return affected == 1;
    }

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

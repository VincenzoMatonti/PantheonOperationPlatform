using Athena.Domain.System.Registry.Entities;
using Athena.Domain.System.Registry.Enums;
using Athena.Domain.System.Registry.Repositories;
using Athena.Domain.System.Registry.ValueObjects;
using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Management.Enums;
using Athena.Domain.Workload.Registry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.Infrastructure.Repositories;

public class SystemInstanceRepository(AthenaDbContext context) : ISystemInstanceRepository
{
    private IQueryable<SystemInstance> ActiveInstances => context.SystemInstances.Where(instance => !instance.IsDeleted);

    public async Task<SystemInstance?> GetByIdAsync(SystemInstanceId id, CancellationToken cancellationToken = default)
    {
        return await ActiveInstances.FirstOrDefaultAsync(instance => instance.Id == id, cancellationToken);
    }

    public async Task<SystemInstance?> GetByNameAsync(WorkloadId workloadId, SystemInstanceName name, CancellationToken cancellationToken = default)
    {
        return await ActiveInstances.FirstOrDefaultAsync(instance => instance.WorkloadId == workloadId && instance.Name == name, cancellationToken);
    }

    public async Task<IReadOnlyList<SystemInstance>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await ActiveInstances.OrderBy(instance => instance.Name.Value).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SystemInstance>> GetBySystemIdAsync(SystemId systemId, CancellationToken cancellationToken = default)
    {
        return await ActiveInstances.Where(instance => instance.SystemId == systemId).OrderBy(instance => instance.Name.Value).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SystemInstance>> GetByWorkloadIdAsync(WorkloadId workloadId, CancellationToken cancellationToken = default)
    {
        return await ActiveInstances.Where(instance => instance.WorkloadId == workloadId).OrderBy(instance => instance.Name.Value).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SystemInstance>> GetByRegistrationStatusAsync(SystemInstanceRegistrationStatus status, CancellationToken cancellationToken = default)
    {
        return await ActiveInstances.Where(instance => instance.RegistrationStatus == status).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SystemInstance>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default)
    {
        return await context.SystemInstances.Where(instance => instance.IsDeleted == isDeleted).ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(SystemInstanceId id, CancellationToken cancellationToken = default)
    {
        return await context.SystemInstances.AnyAsync(instance => instance.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(WorkloadId workloadId, SystemInstanceName name, CancellationToken cancellationToken = default)
    {
        return await context.SystemInstances.AnyAsync(instance => instance.WorkloadId == workloadId && instance.Name == name, cancellationToken);
    }

    public async Task AddAsync(SystemInstance systemInstance, SystemInstanceLifecycle lifecycle, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(systemInstance);
        ArgumentNullException.ThrowIfNull(lifecycle);
        if (lifecycle.Id != systemInstance.Id)
            throw new ArgumentException("The lifecycle must belong to the SystemInstance being created.", nameof(lifecycle));

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
        var entry = context.Entry(systemInstance);
        entry.Property<int>("system_database_id").CurrentValue = systemDatabaseId;
        entry.Property<int>("workload_database_id").CurrentValue = workloadDatabaseId;
        await context.SaveChangesAsync(cancellationToken);

        var instanceDatabaseId = await context.SystemInstances
            .Where(instance => instance.Id == systemInstance.Id)
            .Select(instance => EF.Property<int>(instance, "id"))
            .SingleAsync(cancellationToken);
        context.Entry(lifecycle).Property<int>("system_instance_database_id").CurrentValue = instanceDatabaseId;
        await context.SystemInstanceLifecycles.AddAsync(lifecycle, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task UpdateAsync(SystemInstance systemInstance, CancellationToken cancellationToken = default)
    {
        var entry = context.Entry(systemInstance);
        if (entry.State == EntityState.Detached) context.SystemInstances.Update(systemInstance);
        if (systemInstance.IsDeleted || systemInstance.RegistrationStatus != SystemInstanceRegistrationStatus.Registered)
        {
            var instanceDatabaseId = await context.SystemInstances
                .Where(instance => instance.Id == systemInstance.Id)
                .Select(instance => (int?)EF.Property<int>(instance, "id"))
                .SingleOrDefaultAsync(cancellationToken);
            if (instanceDatabaseId is not null)
            {
                var lifecycle = await context.SystemInstanceLifecycles.SingleOrDefaultAsync(
                    entity => EF.Property<int>(entity, "system_instance_database_id") == instanceDatabaseId,
                    cancellationToken);
                if (lifecycle is not null && !lifecycle.IsDeleted)
                {
                    if (systemInstance.IsDeleted) lifecycle.Delete();
                    else lifecycle.SetOperationalStatus(SystemInstanceOperationalStatus.Unknown);
                }
            }
        }
        await context.SaveChangesAsync(cancellationToken);
    }
}
using Athena.Domain.System.Registry.Entities;
using Athena.Domain.System.Registry.Enums;
using Athena.Domain.System.Registry.Repositories;
using Athena.Domain.System.Registry.ValueObjects;
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

    public async Task<IReadOnlyList<SystemInstance>> GetByOperationalStatusAsync(SystemInstanceOperationalStatus status, CancellationToken cancellationToken = default)
    {
        return await ActiveInstances.Where(instance => instance.OperationalStatus == status).ToListAsync(cancellationToken);
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

    public async Task AddAsync(SystemInstance systemInstance, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(systemInstance);
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
    }

    public async Task UpdateAsync(SystemInstance systemInstance, CancellationToken cancellationToken = default)
    {
        var entry = context.Entry(systemInstance);
        if (entry.State == EntityState.Detached) context.SystemInstances.Update(systemInstance);
        await context.SaveChangesAsync(cancellationToken);
    }
}
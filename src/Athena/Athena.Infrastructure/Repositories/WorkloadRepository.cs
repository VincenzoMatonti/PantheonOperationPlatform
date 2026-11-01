using Athena.Domain.Block.Registry.ValueObjects;
using Athena.Domain.Workload.Management.Entities;
using Athena.Domain.Workload.Registry.Entities;
using Athena.Domain.Workload.Registry.Enums;
using Athena.Domain.Workload.Registry.Repositories;
using Athena.Domain.Workload.Registry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.Infrastructure.Repositories;

public class WorkloadRepository(AthenaDbContext context) : IWorkloadRepository
{
    private IQueryable<Workload> ActiveWorkloads => context.Workloads.Where(workload => !workload.IsDeleted);

    public async Task<Workload?> GetByIdAsync(WorkloadId id, CancellationToken cancellationToken = default)
    {
        return await ActiveWorkloads.FirstOrDefaultAsync(workload => workload.Id == id, cancellationToken);
    }

    public async Task<Workload?> GetByNameAsync(BlockId blockId, WorkloadName name, CancellationToken cancellationToken = default)
    {
        return await ActiveWorkloads.FirstOrDefaultAsync(workload => workload.BlockId == blockId && workload.Name == name, cancellationToken);
    }

    public async Task<IReadOnlyList<Workload>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await ActiveWorkloads.OrderBy(workload => workload.Name.Value).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Workload>> GetByBlockIdAsync(BlockId blockId, CancellationToken cancellationToken = default)
    {
        return await ActiveWorkloads.Where(workload => workload.BlockId == blockId).OrderBy(workload => workload.Name.Value).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Workload>> GetByRegistrationStatusAsync(WorkloadRegistrationStatus status, CancellationToken cancellationToken = default)
    {
        return await ActiveWorkloads.Where(workload => workload.RegistrationStatus == status).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Workload>> GetByAvailabilityStatusAsync(WorkloadAvailabilityStatus status, CancellationToken cancellationToken = default)
    {
        return await ActiveWorkloads.Where(workload => workload.AvailabilityStatus == status).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Workload>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default)
    {
        return await context.Workloads.Where(workload => workload.IsDeleted == isDeleted).ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(WorkloadId id, CancellationToken cancellationToken = default)
    {
        return await context.Workloads.AnyAsync(workload => workload.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(BlockId blockId, WorkloadName name, CancellationToken cancellationToken = default)
    {
        return await context.Workloads.AnyAsync(workload => workload.BlockId == blockId && workload.Name == name, cancellationToken);
    }

    public async Task AddAsync(Workload workload, WorkloadLifecycle lifecycle, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(workload);
        ArgumentNullException.ThrowIfNull(lifecycle);
        if (lifecycle.Id != workload.Id)
            throw new ArgumentException("The lifecycle must belong to the Workload being created.", nameof(lifecycle));

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        var blockDatabaseId = await context.Blocks
            .Where(block => block.Id == workload.BlockId && !block.IsDeleted)
            .Select(block => (int?)EF.Property<int>(block, "id"))
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Block '{workload.BlockId.Value}' does not exist.");
        context.Entry(workload).Property<int>("block_database_id").CurrentValue = blockDatabaseId;
        await context.Workloads.AddAsync(workload, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var workloadDatabaseId = await context.Workloads
            .Where(entity => entity.Id == workload.Id)
            .Select(entity => EF.Property<int>(entity, "id"))
            .SingleAsync(cancellationToken);
        context.Entry(lifecycle).Property<int>("workload_database_id").CurrentValue = workloadDatabaseId;
        await context.WorkloadLifecycles.AddAsync(lifecycle, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task UpdateAsync(Workload workload, CancellationToken cancellationToken = default)
    {
        var workloadEntry = context.Entry(workload);
        if (workloadEntry.State == EntityState.Detached) context.Workloads.Update(workload);
        if (workload.IsDeleted || workload.RegistrationStatus != WorkloadRegistrationStatus.Registered)
        {
            var workloadDatabaseId = await context.Workloads
                .Where(entity => entity.Id == workload.Id)
                .Select(entity => (int?)EF.Property<int>(entity, "id"))
                .SingleOrDefaultAsync(cancellationToken);
            if (workloadDatabaseId is not null)
            {
                var lifecycle = await context.WorkloadLifecycles.SingleOrDefaultAsync(
                    entity => EF.Property<int>(entity, "workload_database_id") == workloadDatabaseId,
                    cancellationToken);
                if (lifecycle is not null && !lifecycle.IsDeleted)
                {
                    if (workload.IsDeleted) lifecycle.Delete();
                    else lifecycle.SetOperationalStatus(Athena.Domain.Workload.Management.Enums.WorkloadOperationalStatus.Unknown);
                }
            }
        }
        await context.SaveChangesAsync(cancellationToken);
    }
}
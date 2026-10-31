using Athena.Domain.Block.Registry.ValueObjects;
using Athena.Domain.Workload.Registry.Entities;
using Athena.Domain.Workload.Registry.Enums;
using Athena.Domain.Workload.Registry.Repositories;
using Athena.Domain.Workload.Registry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.Infrastructure.Repositories;

public sealed class WorkloadRepository(AthenaDbContext context) : IWorkloadRepository
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

    public async Task AddAsync(Workload workload, CancellationToken cancellationToken = default)
    {
        await context.Workloads.AddAsync(workload, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Workload workload, CancellationToken cancellationToken = default)
    {
        context.Workloads.Update(workload);
        await context.SaveChangesAsync(cancellationToken);
    }
}
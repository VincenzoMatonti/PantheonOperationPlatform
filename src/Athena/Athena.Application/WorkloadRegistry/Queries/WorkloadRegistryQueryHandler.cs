using Athena.Application.WorkloadRegistry.Contracts;
using Athena.Domain.Block.Registry.ValueObjects;
using Athena.Domain.Workload.Registry.Enums;
using Athena.Domain.Workload.Registry.Repositories;
using Athena.Domain.Workload.Registry.ValueObjects;

namespace Athena.Application.WorkloadRegistry.Queries;

public sealed class WorkloadRegistryQueryHandler(IWorkloadRepository workloadRepository)
{
    private readonly IWorkloadRepository _workloadRepository = workloadRepository;

    public async Task<WorkloadDto?> HandleAsync(GetWorkloadByIdQuery query, CancellationToken cancellationToken = default)
    {
        var workload = await _workloadRepository.GetByIdAsync(WorkloadId.Create(query.Id), cancellationToken);
        return workload is null ? null : WorkloadDtoMapper.ToDto(workload);
    }

    public async Task<WorkloadDto?> HandleAsync(GetWorkloadByNameQuery query, CancellationToken cancellationToken = default)
    {
        var workload = await _workloadRepository.GetByNameAsync(BlockId.Create(query.BlockId), WorkloadName.Create(query.Name), cancellationToken);
        return workload is null ? null : WorkloadDtoMapper.ToDto(workload);
    }

    public async Task<IReadOnlyList<WorkloadDto>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var workloads = await _workloadRepository.GetAllAsync(cancellationToken);
        return [.. workloads.Select(WorkloadDtoMapper.ToDto)];
    }

    public async Task<IReadOnlyList<WorkloadDto>> HandleAsync(GetWorkloadsByBlockIdQuery query, CancellationToken cancellationToken = default)
    {
        var workloads = await _workloadRepository.GetByBlockIdAsync(BlockId.Create(query.BlockId), cancellationToken);
        return [.. workloads.Select(WorkloadDtoMapper.ToDto)];
    }

    public async Task<IReadOnlyList<WorkloadDto>> HandleAsync(GetWorkloadsByRegistrationStatusQuery query, CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(typeof(WorkloadRegistrationStatus), query.Status)) return [];
        var workloads = await _workloadRepository.GetByRegistrationStatusAsync((WorkloadRegistrationStatus)query.Status, cancellationToken);
        return [.. workloads.Select(WorkloadDtoMapper.ToDto)];
    }

    public async Task<IReadOnlyList<WorkloadDto>> HandleAsync(GetWorkloadsByAvailabilityStatusQuery query, CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(typeof(WorkloadAvailabilityStatus), query.Status)) return [];
        var workloads = await _workloadRepository.GetByAvailabilityStatusAsync((WorkloadAvailabilityStatus)query.Status, cancellationToken);
        return [.. workloads.Select(WorkloadDtoMapper.ToDto)];
    }

    public async Task<IReadOnlyList<WorkloadDto>> HandleAsync(GetWorkloadsByDeletionStatusQuery query, CancellationToken cancellationToken = default)
    {
        var workloads = await _workloadRepository.GetByDeletionStatusAsync(query.IsDeleted, cancellationToken);
        return [.. workloads.Select(WorkloadDtoMapper.ToDto)];
    }

    public async Task<bool> NameExistsAsync(Guid blockId, string name, CancellationToken cancellationToken = default)
    {
        return await _workloadRepository.ExistsByNameAsync(BlockId.Create(blockId), WorkloadName.Create(name), cancellationToken);
    }
}
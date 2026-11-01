using Athena.Application.WorkloadManagement.Contracts;
using Athena.Domain.Workload.Management.Repositories;
using Athena.Domain.Workload.Registry.Exceptions;
using Athena.Domain.Workload.Registry.Repositories;
using Athena.Domain.Workload.Registry.ValueObjects;

namespace Athena.Application.WorkloadManagement.Queries;

public sealed class WorkloadManagementQueryHandler(
    IWorkloadRepository workloadRepository,
    IWorkloadLifecycleRepository lifecycleRepository,
    IWorkloadOperationRepository operationRepository)
{
    public async Task<WorkloadLifecycleDto?> GetLifecycleAsync(GetWorkloadLifecycleQuery query, CancellationToken token)
    {
        var id = WorkloadId.Create(query.WorkloadId);
        _ = await workloadRepository.GetByIdAsync(id, token) ?? throw new WorkloadNotFoundException(id.Value);
        var lifecycle = await lifecycleRepository.GetByIdAsync(id, token);
        return lifecycle is null ? null : WorkloadManagementDtoMapper.ToDto(lifecycle);
    }

    public async Task<IReadOnlyList<WorkloadLifecycleDto>> GetLifecyclesAsync(GetWorkloadLifecyclesQuery query, CancellationToken token) =>
        (await lifecycleRepository.GetAllAsync(token)).Select(WorkloadManagementDtoMapper.ToDto).ToArray();

    public async Task<IReadOnlyList<WorkloadOperationDto>> GetOperationsAsync(GetWorkloadOperationsQuery query, CancellationToken token)
    {
        var id = WorkloadId.Create(query.WorkloadId);
        _ = await workloadRepository.GetByIdAsync(id, token) ?? throw new WorkloadNotFoundException(id.Value);
        return (await operationRepository.GetByWorkloadIdAsync(id, token)).Select(WorkloadManagementDtoMapper.ToDto).ToArray();
    }
}

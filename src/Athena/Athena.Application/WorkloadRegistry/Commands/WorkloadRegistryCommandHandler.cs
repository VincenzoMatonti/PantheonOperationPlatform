using Athena.Application.WorkloadRegistry.Contracts;
using Athena.Domain.Block.Registry.ValueObjects;
using Athena.Domain.Workload.Management.Entities;
using Athena.Domain.Workload.Registry.Entities;
using Athena.Domain.Workload.Registry.Repositories;
using Athena.Domain.Workload.Registry.ValueObjects;

namespace Athena.Application.WorkloadRegistry.Commands;

public sealed class WorkloadRegistryCommandHandler(IWorkloadRepository workloadRepository)
{
    private readonly IWorkloadRepository _workloadRepository = workloadRepository;

    public async Task<WorkloadDto> HandleAsync(CreateWorkloadCommand command, CancellationToken cancellationToken = default)
    {
        var id = WorkloadId.New();
        var workload = Workload.Create(id, BlockId.Create(command.BlockId), WorkloadName.Create(command.Name));
        var lifecycle = WorkloadLifecycle.Create(id);
        await _workloadRepository.AddAsync(workload, lifecycle, cancellationToken);
        return WorkloadDtoMapper.ToDto(workload);
    }

    public Task<WorkloadDto?> HandleAsync(RegisterWorkloadCommand command, CancellationToken cancellationToken = default) => MutateAsync(command.Id, workload => workload.Register(), cancellationToken);

    public Task<WorkloadDto?> HandleAsync(UnregisterWorkloadCommand command, CancellationToken cancellationToken = default) => MutateAsync(command.Id, workload => workload.Unregister(), cancellationToken);

    public Task<WorkloadDto?> HandleAsync(RenameWorkloadCommand command, CancellationToken cancellationToken = default) =>
    MutateAsync(command.Id, workload => workload.Rename(WorkloadName.Create(command.Name)), cancellationToken);

    public Task<WorkloadDto?> HandleAsync(EnableWorkloadCommand command, CancellationToken cancellationToken = default) => MutateAsync(command.Id, workload => workload.Enable(), cancellationToken);

    public Task<WorkloadDto?> HandleAsync(DisableWorkloadCommand command, CancellationToken cancellationToken = default) => MutateAsync(command.Id, workload => workload.Disable(), cancellationToken);

    public Task<WorkloadDto?> HandleAsync(DeleteWorkloadCommand command, CancellationToken cancellationToken = default) => MutateAsync(command.Id, workload => workload.Delete(), cancellationToken);

    public Task<WorkloadDto?> HandleAsync(MarkWorkloadAvailableCommand command, CancellationToken cancellationToken = default) => MutateAsync(command.Id, workload => workload.MarkAvailable(), cancellationToken);

    public Task<WorkloadDto?> HandleAsync(MarkWorkloadUnavailableCommand command, CancellationToken cancellationToken = default) => MutateAsync(command.Id, workload => workload.MarkUnavailable(), cancellationToken);

    public Task<WorkloadDto?> HandleAsync(MarkWorkloadAvailabilityUnknownCommand command, CancellationToken cancellationToken = default) =>
    MutateAsync(command.Id, workload => workload.MarkAvailabilityUnknown(), cancellationToken);

    public Task<WorkloadDto?> HandleAsync(MarkWorkloadErrorCommand command, CancellationToken cancellationToken = default) => MutateAsync(command.Id, workload => workload.MarkError(), cancellationToken);

    private async Task<WorkloadDto?> MutateAsync(Guid id, Action<Workload> mutation, CancellationToken cancellationToken)
    {
        var workload = await _workloadRepository.GetByIdAsync(WorkloadId.Create(id), cancellationToken);
        if (workload is null) return null;
        mutation(workload);
        await _workloadRepository.UpdateAsync(workload, cancellationToken);
        return WorkloadDtoMapper.ToDto(workload);
    }
}
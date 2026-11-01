using Athena.Application.WorkloadManagement.Contracts;
using Athena.Application.WorkloadManagement.Exceptions;
using Athena.Domain.Workload.Management.Entities;
using Athena.Domain.Workload.Management.Enums;
using Athena.Domain.Workload.Management.Repositories;
using Athena.Domain.Workload.Registry.Entities;
using Athena.Domain.Workload.Registry.Enums;
using Athena.Domain.Workload.Registry.Exceptions;
using Athena.Domain.Workload.Registry.Repositories;
using Athena.Domain.Workload.Registry.ValueObjects;

namespace Athena.Application.WorkloadManagement.Commands;

public sealed class WorkloadManagementCommandHandler(IWorkloadRepository workloadRepository, IWorkloadLifecycleRepository lifecycleRepository, IWorkloadOperationRepository operationRepository)
{
    public async Task<WorkloadLifecycleDto> CreateLifecycleAsync(CreateWorkloadLifecycleCommand command, CancellationToken token)
    {
        var id = WorkloadId.Create(command.WorkloadId);
        _ = await GetWorkloadAsync(id, token);
        if (await lifecycleRepository.GetByIdAsync(id, token) is not null)
            throw Error(WorkloadManagementErrorCode.WorkloadLifecycleAlreadyExists, $"Lifecycle for workload '{id.Value}' already exists.");
        var lifecycle = WorkloadLifecycle.Create(id);
        await lifecycleRepository.AddAsync(lifecycle, token);
        return WorkloadManagementDtoMapper.ToDto(lifecycle);
    }

    public async Task<WorkloadLifecycleDto> SetDesiredStateAsync(SetWorkloadDesiredStateCommand command, CancellationToken token)
    {
        if (!Enum.TryParse<WorkloadDesiredState>(command.DesiredState, true, out var desired) || !Enum.IsDefined(desired))
            throw Error(WorkloadManagementErrorCode.InvalidDesiredState, $"Desired state '{command.DesiredState}' is invalid.");
        var id = WorkloadId.Create(command.WorkloadId);
        EnsureRegistered(await GetWorkloadAsync(id, token));
        var lifecycle = await GetRequiredLifecycleAsync(id, token);
        lifecycle.SetDesiredState(desired);
        lifecycle.SetOperationalStatus(WorkloadOperationalStatus.Unknown);
        await lifecycleRepository.UpdateAsync(lifecycle, token);
        return WorkloadManagementDtoMapper.ToDto(lifecycle);
    }

    public async Task<WorkloadOperationDto> ExecuteOperationAsync(ExecuteWorkloadOperationCommand command, CancellationToken token)
    {
        if (!Enum.TryParse<WorkloadOperationType>(command.OperationType, true, out var type) || !Enum.IsDefined(type))
            throw Error(WorkloadManagementErrorCode.InvalidOperationType, $"Operation type '{command.OperationType}' is invalid.");
        var id = WorkloadId.Create(command.WorkloadId);
        EnsureRegistered(await GetWorkloadAsync(id, token));
        var lifecycle = await GetRequiredLifecycleAsync(id, token);
        var operation = WorkloadOperation.Create(Guid.NewGuid(), id, type);
        lifecycle.SetDesiredState(type switch
        {
            WorkloadOperationType.Start or WorkloadOperationType.Restart => WorkloadDesiredState.Running,
            WorkloadOperationType.Stop => WorkloadDesiredState.Stopped,
            _ => throw Error(WorkloadManagementErrorCode.InvalidOperationType, $"Operation type '{command.OperationType}' is invalid.")
        });
        lifecycle.SetOperationalStatus(WorkloadOperationalStatus.Unknown);
        operation.Start();
        operation.Complete();
        await operationRepository.SaveIntentAsync(operation, lifecycle, token);
        return WorkloadManagementDtoMapper.ToDto(operation);
    }

    private async Task<Workload> GetWorkloadAsync(WorkloadId id, CancellationToken token) => await workloadRepository.GetByIdAsync(id, token) ?? throw new WorkloadNotFoundException(id.Value);

    private static void EnsureRegistered(Workload workload)
    {
        if (workload.RegistrationStatus != WorkloadRegistrationStatus.Registered) throw new WorkloadNotRegisteredException(workload.Id.Value);
    }

    private async Task<WorkloadLifecycle> GetRequiredLifecycleAsync(WorkloadId id, CancellationToken token) =>
        await lifecycleRepository.GetByIdAsync(id, token) ?? throw Error(WorkloadManagementErrorCode.WorkloadLifecycleNotFound, $"Lifecycle for workload '{id.Value}' was not found.");

    private static WorkloadManagementException Error(WorkloadManagementErrorCode code, string message) => new(code, message);

}

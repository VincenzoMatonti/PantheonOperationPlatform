using Athena.Application.Common.Services;
using Athena.Application.WorkloadManagement.Contracts;
using Athena.Application.WorkloadManagement.Exceptions;
using Athena.Domain.Workload.Management.Entities;
using Athena.Domain.Workload.Management.Enums;
using Athena.Domain.Workload.Management.Repositories;
using Athena.Domain.Workload.Registry.Exceptions;
using Athena.Domain.Workload.Registry.Repositories;
using Athena.Domain.Workload.Registry.ValueObjects;

namespace Athena.Application.WorkloadManagement.UseCases;

public sealed class WorkloadManagementUseCaseHandler(
    IWorkloadRepository workloadRepository,
    IWorkloadLifecycleRepository lifecycleRepository,
    IWorkloadOperationRepository operationRepository,
    ApplicationExceptionService exceptionService) : IWorkloadManagement
{
    public Task<WorkloadLifecycleDto> CreateLifecycleAsync(Guid id, CancellationToken token = default) =>
        RunAsync(async () =>
        {
            var workloadId = WorkloadId.Create(id);
            await GetWorkloadAsync(workloadId, token);
            if (await lifecycleRepository.GetByIdAsync(workloadId, token) is not null)
                throw Error(WorkloadManagementErrorCode.WorkloadLifecycleAlreadyExists, $"Lifecycle for workload '{id}' already exists.");
            var lifecycle = WorkloadLifecycle.Create(workloadId);
            await lifecycleRepository.AddAsync(lifecycle, token);
            return ToDto(lifecycle);
        });

    public Task<WorkloadLifecycleDto?> GetLifecycleAsync(Guid id, CancellationToken token = default) =>
        RunAsync(async () =>
        {
            var workloadId = WorkloadId.Create(id);
            await GetWorkloadAsync(workloadId, token);
            var lifecycle = await lifecycleRepository.GetByIdAsync(workloadId, token);
            return lifecycle is null ? null : ToDto(lifecycle);
        });

    public Task<IReadOnlyList<WorkloadLifecycleDto>> GetLifecyclesAsync(CancellationToken token = default) =>
        RunAsync<IReadOnlyList<WorkloadLifecycleDto>>(async () => (await lifecycleRepository.GetAllAsync(token)).Select(ToDto).ToArray());

    public Task<WorkloadLifecycleDto> SetDesiredStateAsync(Guid id, string state, CancellationToken token = default) =>
        RunAsync(async () =>
        {
            if (!Enum.TryParse<WorkloadDesiredState>(state, true, out var desired) || !Enum.IsDefined(desired))
                throw Error(WorkloadManagementErrorCode.InvalidDesiredState, $"Desired state '{state}' is invalid.");
            var workloadId = WorkloadId.Create(id);
            EnsureRegistered(await GetWorkloadAsync(workloadId, token));
            var lifecycle = await GetRequiredLifecycleAsync(workloadId, token);
            lifecycle.SetDesiredState(desired);
            lifecycle.SetOperationalStatus(WorkloadOperationalStatus.Unknown);
            await lifecycleRepository.UpdateAsync(lifecycle, token);
            return ToDto(lifecycle);
        });

    public Task<WorkloadOperationDto> ExecuteOperationAsync(Guid id, string type, CancellationToken token = default) =>
        RunAsync(async () =>
        {
            if (!Enum.TryParse<WorkloadOperationType>(type, true, out var operationType) || !Enum.IsDefined(operationType))
                throw Error(WorkloadManagementErrorCode.InvalidOperationType, $"Operation type '{type}' is invalid.");
            var workloadId = WorkloadId.Create(id);
            EnsureRegistered(await GetWorkloadAsync(workloadId, token));
            var lifecycle = await GetRequiredLifecycleAsync(workloadId, token);
            var operation = WorkloadOperation.Create(Guid.NewGuid(), workloadId, operationType);
            var desired = operationType switch
            {
                WorkloadOperationType.Start or WorkloadOperationType.Restart => WorkloadDesiredState.Running,
                WorkloadOperationType.Stop => WorkloadDesiredState.Stopped,
                _ => throw Error(WorkloadManagementErrorCode.InvalidOperationType, $"Operation type '{type}' is invalid.")
            };
            lifecycle.SetDesiredState(desired);
            lifecycle.SetOperationalStatus(WorkloadOperationalStatus.Unknown);
            operation.Start();
            operation.Complete();
            await operationRepository.SaveIntentAsync(operation, lifecycle, token);
            return ToDto(operation);
        });

    public Task<IReadOnlyList<WorkloadOperationDto>> GetOperationsAsync(Guid id, CancellationToken token = default) =>
        RunAsync<IReadOnlyList<WorkloadOperationDto>>(async () =>
        {
            var workloadId = WorkloadId.Create(id);
            await GetWorkloadAsync(workloadId, token);
            return (await operationRepository.GetByWorkloadIdAsync(workloadId, token)).Select(ToDto).ToArray();
        });

    private async Task<Athena.Domain.Workload.Registry.Entities.Workload> GetWorkloadAsync(WorkloadId id, CancellationToken token) =>
        await workloadRepository.GetByIdAsync(id, token) ?? throw new WorkloadNotFoundException(id.Value);

    private static void EnsureRegistered(Athena.Domain.Workload.Registry.Entities.Workload workload)
    {
        if (workload.RegistrationStatus != Athena.Domain.Workload.Registry.Enums.WorkloadRegistrationStatus.Registered)
            throw new WorkloadNotRegisteredException(workload.Id.Value);
    }

    private async Task<WorkloadLifecycle> GetRequiredLifecycleAsync(WorkloadId id, CancellationToken token) =>
        await lifecycleRepository.GetByIdAsync(id, token)
        ?? throw Error(WorkloadManagementErrorCode.WorkloadLifecycleNotFound, $"Lifecycle for workload '{id.Value}' was not found.");

    private Task<T> RunAsync<T>(Func<Task<T>> operation) => exceptionService.ExecuteAsync(operation);
    private static WorkloadManagementException Error(WorkloadManagementErrorCode code, string message) => new(code, message);
    private static WorkloadLifecycleDto ToDto(WorkloadLifecycle lifecycle) =>
        new(lifecycle.Id.Value, lifecycle.DesiredState.ToString(), lifecycle.OperationalStatus.ToString(), lifecycle.CreatedAtUtc, lifecycle.UpdatedAtUtc, lifecycle.IsDeleted);
    private static WorkloadOperationDto ToDto(WorkloadOperation operation) =>
        new(operation.Id, operation.WorkloadId.Value, operation.Type.ToString(), operation.Status.ToString(),
            operation.CreatedAtUtc, operation.StartedAtUtc, operation.CompletedAtUtc, operation.ErrorMessage, operation.IsDeleted);
}

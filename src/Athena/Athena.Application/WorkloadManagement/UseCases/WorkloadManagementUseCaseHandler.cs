using Athena.Application.Common.Services;
using Athena.Application.WorkloadManagement.Commands;
using Athena.Application.WorkloadManagement.Contracts;
using Athena.Application.WorkloadManagement.Queries;

namespace Athena.Application.WorkloadManagement.UseCases;

public sealed class WorkloadManagementUseCaseHandler(
    WorkloadManagementCommandHandler commandHandler,
    WorkloadManagementQueryHandler queryHandler,
    ApplicationExceptionService exceptionService) : IWorkloadManagement
{
    public Task<WorkloadLifecycleDto> CreateLifecycleAsync(CreateWorkloadLifecycleCommand command, CancellationToken token = default) =>
        ExecuteAsync(() => commandHandler.CreateLifecycleAsync(command, token));

    public Task<WorkloadLifecycleDto?> GetLifecycleAsync(GetWorkloadLifecycleQuery query, CancellationToken token = default) =>
        ExecuteAsync(() => queryHandler.GetLifecycleAsync(query, token));

    public Task<IReadOnlyList<WorkloadLifecycleDto>> GetLifecyclesAsync(GetWorkloadLifecyclesQuery query, CancellationToken token = default) =>
        ExecuteAsync(() => queryHandler.GetLifecyclesAsync(query, token));

    public Task<WorkloadLifecycleDto> SetDesiredStateAsync(SetWorkloadDesiredStateCommand command, CancellationToken token = default) =>
        ExecuteAsync(() => commandHandler.SetDesiredStateAsync(command, token));

    public Task<WorkloadOperationDto> ExecuteOperationAsync(ExecuteWorkloadOperationCommand command, CancellationToken token = default) =>
        ExecuteAsync(() => commandHandler.ExecuteOperationAsync(command, token));

    public Task<IReadOnlyList<WorkloadOperationDto>> GetOperationsAsync(GetWorkloadOperationsQuery query, CancellationToken token = default) =>
        ExecuteAsync(() => queryHandler.GetOperationsAsync(query, token));

    public Task<WorkloadLifecycleDto> CreateLifecycleAsync(Guid workloadId, CancellationToken token = default) =>
        CreateLifecycleAsync(new CreateWorkloadLifecycleCommand(workloadId), token);

    public Task<WorkloadLifecycleDto?> GetLifecycleAsync(Guid workloadId, CancellationToken token = default) =>
        GetLifecycleAsync(new GetWorkloadLifecycleQuery(workloadId), token);

    public Task<IReadOnlyList<WorkloadLifecycleDto>> GetLifecyclesAsync(CancellationToken token = default) =>
        GetLifecyclesAsync(new GetWorkloadLifecyclesQuery(), token);

    public Task<WorkloadLifecycleDto> SetDesiredStateAsync(Guid workloadId, string desiredState, CancellationToken token = default) =>
        SetDesiredStateAsync(new SetWorkloadDesiredStateCommand(workloadId, desiredState), token);

    public Task<WorkloadOperationDto> ExecuteOperationAsync(Guid workloadId, string operationType, CancellationToken token = default) =>
        ExecuteOperationAsync(new ExecuteWorkloadOperationCommand(workloadId, operationType), token);

    public Task<IReadOnlyList<WorkloadOperationDto>> GetOperationsAsync(Guid workloadId, CancellationToken token = default) =>
        GetOperationsAsync(new GetWorkloadOperationsQuery(workloadId), token);

    private Task<T> ExecuteAsync<T>(Func<Task<T>> operation) => exceptionService.ExecuteAsync(operation);
}

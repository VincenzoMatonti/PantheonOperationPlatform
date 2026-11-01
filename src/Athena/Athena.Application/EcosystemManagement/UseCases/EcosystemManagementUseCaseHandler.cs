using Athena.Application.Common.Services;
using Athena.Application.EcosystemManagement.Commands;
using Athena.Application.EcosystemManagement.Contracts;
using Athena.Application.EcosystemManagement.Queries;

namespace Athena.Application.EcosystemManagement.UseCases;

public sealed class EcosystemManagementUseCaseHandler(
    EcosystemManagementCommandHandler commandHandler,
    EcosystemManagementQueryHandler queryHandler,
    ApplicationExceptionService exceptionService) : IEcosystemManagement
{
    public Task<EcosystemLifecycleDto> CreateLifecycleAsync(CreateEcosystemLifecycleCommand command, CancellationToken token = default) =>
        ExecuteAsync(() => commandHandler.CreateLifecycleAsync(command, token));

    public Task<EcosystemLifecycleDto?> GetLifecycleAsync(GetEcosystemLifecycleQuery query, CancellationToken token = default) =>
        ExecuteAsync(() => queryHandler.GetLifecycleAsync(query, token));

    public Task<IReadOnlyList<EcosystemLifecycleDto>> GetLifecyclesAsync(GetEcosystemLifecyclesQuery query, CancellationToken token = default) =>
        ExecuteAsync(() => queryHandler.GetLifecyclesAsync(query, token));

    public Task<EcosystemLifecycleDto> SetDesiredStateAsync(SetEcosystemDesiredStateCommand command, CancellationToken token = default) =>
        ExecuteAsync(() => commandHandler.SetDesiredStateAsync(command, token));

    public Task<EcosystemOperationDto> ExecuteOperationAsync(ExecuteEcosystemOperationCommand command, CancellationToken token = default) =>
        ExecuteAsync(() => commandHandler.ExecuteOperationAsync(command, token));

    public Task<IReadOnlyList<EcosystemOperationDto>> GetOperationsAsync(GetEcosystemOperationsQuery query, CancellationToken token = default) =>
        ExecuteAsync(() => queryHandler.GetOperationsAsync(query, token));

    public Task<EcosystemLifecycleDto> CreateLifecycleAsync(Guid ecosystemId, CancellationToken token = default) =>
        CreateLifecycleAsync(new CreateEcosystemLifecycleCommand(ecosystemId), token);

    public Task<EcosystemLifecycleDto?> GetLifecycleAsync(Guid ecosystemId, CancellationToken token = default) =>
        GetLifecycleAsync(new GetEcosystemLifecycleQuery(ecosystemId), token);

    public Task<IReadOnlyList<EcosystemLifecycleDto>> GetLifecyclesAsync(CancellationToken token = default) =>
        GetLifecyclesAsync(new GetEcosystemLifecyclesQuery(), token);

    public Task<EcosystemLifecycleDto> SetDesiredStateAsync(Guid ecosystemId, string desiredState, CancellationToken token = default) =>
        SetDesiredStateAsync(new SetEcosystemDesiredStateCommand(ecosystemId, desiredState), token);

    public Task<EcosystemOperationDto> ExecuteOperationAsync(Guid ecosystemId, string operationType, CancellationToken token = default) =>
        ExecuteOperationAsync(new ExecuteEcosystemOperationCommand(ecosystemId, operationType), token);

    public Task<IReadOnlyList<EcosystemOperationDto>> GetOperationsAsync(Guid ecosystemId, CancellationToken token = default) =>
        GetOperationsAsync(new GetEcosystemOperationsQuery(ecosystemId), token);

    private Task<T> ExecuteAsync<T>(Func<Task<T>> operation) => exceptionService.ExecuteAsync(operation);
}

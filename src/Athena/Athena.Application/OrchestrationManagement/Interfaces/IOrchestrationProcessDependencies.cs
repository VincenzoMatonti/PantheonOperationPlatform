using Athena.Application.OrchestrationManagement.Commands;
using Athena.Application.OrchestrationManagement.DTOs;
using Athena.Application.OrchestrationManagement.Queries;

namespace Athena.Application.OrchestrationManagement.Interfaces;

public interface IOrchestrationLifecycleQueries
{
    Task<OrchestrationLifecycleDto?> GetOrchestrationLifecycleByIdAsync(
        GetOrchestrationLifecycleByIdQuery query,
        CancellationToken cancellationToken = default);
}

public interface IOrchestrationLifecycleCommands
{
    Task<OrchestrationLifecycleDto> SetOrchestrationDesiredStateAsync(SetOrchestrationDesiredStateCommand command, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleDto> MarkOrchestrationStartingAsync(MarkOrchestrationStartingCommand command, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleDto> MarkOrchestrationStoppingAsync(MarkOrchestrationStoppingCommand command, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleDto> MarkOrchestrationFailedAsync(MarkOrchestrationFailedCommand command, CancellationToken cancellationToken = default);
}

public interface IOrchestrationLifecycleOperations
{
    Task<OrchestrationLifecycleOperationDto> CreateOrchestrationLifecycleOperationAsync(CreateOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleOperationDto> StartOrchestrationLifecycleOperationAsync(StartOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleOperationDto> CompleteOrchestrationLifecycleOperationAsync(CompleteOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleOperationDto> FailOrchestrationLifecycleOperationAsync(FailOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default);
    Task<OrchestrationLifecycleOperationDto> CancelOrchestrationLifecycleOperationAsync(CancelOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default);
}

public interface IOrchestrationStatusSynchronizer
{
    Task SynchronizeForSystemInstancesAsync(
        IEnumerable<Guid> systemInstanceIds,
        CancellationToken cancellationToken = default,
        Guid? excludedOrchestrationId = null);
}

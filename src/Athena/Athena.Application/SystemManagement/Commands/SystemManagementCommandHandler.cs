using Athena.Application.SystemManagement.Dtos;
using Athena.Application.SystemManagement.Enums;
using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Management.Repositories;
using Athena.Domain.System.Registry.ValueObjects;
using DomainDesiredState = Athena.Domain.System.Management.Enums.SystemDesiredState;
using DomainOperationType = Athena.Domain.System.Management.Enums.SystemLifecycleOperationType;

namespace Athena.Application.SystemManagement.Commands;

public class SystemManagementCommandHandler(
    ISystemLifecycleRepository systemLifecycleRepository,
    ISystemLifecycleOperationRepository systemLifecycleOperationRepository)
{
    private readonly ISystemLifecycleRepository _systemLifecycleRepository = systemLifecycleRepository;
    private readonly ISystemLifecycleOperationRepository _systemLifecycleOperationRepository = systemLifecycleOperationRepository;

    // =================================================================================================================
    // SYSTEM LIFECYCLE COMMANDS
    public async Task<SystemLifecycleDto> CreateSystemLifecycleAsync(
        CreateSystemLifecycleCommand command, CancellationToken cancellationToken = default)
    {
        var systemId = SystemId.Create(command.SystemId);
        var lifecycle = SystemLifecycle.Create(systemId);
        await _systemLifecycleRepository.AddAsync(lifecycle, cancellationToken);
        return ConvertSystemLifecycleEntitiesToDto(lifecycle);
    }

    public async Task<SystemLifecycleDto?> SetSystemDesiredStateAsync(
        SetSystemDesiredStateCommand command, CancellationToken cancellationToken = default)
    {
        var lifecycle = await GetSystemLifecycleAsync(command.SystemId, cancellationToken);
        if (lifecycle is null) return null;
        lifecycle.SetDesiredState(ConvertSystemDesiredStateToDomain(command.DesiredState));
        await _systemLifecycleRepository.UpdateAsync(lifecycle, cancellationToken);
        return ConvertSystemLifecycleEntitiesToDto(lifecycle);
    }

    public Task<SystemLifecycleDto?> MarkSystemStartingAsync(
        MarkSystemStartingCommand command, CancellationToken cancellationToken = default) =>
        UpdateSystemLifecycleAsync(command.SystemId, lifecycle => lifecycle.MarkStarting(), cancellationToken);

    public Task<SystemLifecycleDto?> MarkSystemRunningAsync(
        MarkSystemRunningCommand command, CancellationToken cancellationToken = default) =>
        UpdateSystemLifecycleAsync(command.SystemId, lifecycle => lifecycle.MarkRunning(), cancellationToken);

    public Task<SystemLifecycleDto?> MarkSystemStoppingAsync(
        MarkSystemStoppingCommand command, CancellationToken cancellationToken = default) =>
        UpdateSystemLifecycleAsync(command.SystemId, lifecycle => lifecycle.MarkStopping(), cancellationToken);

    public Task<SystemLifecycleDto?> MarkSystemStoppedAsync(
        MarkSystemStoppedCommand command, CancellationToken cancellationToken = default) =>
        UpdateSystemLifecycleAsync(command.SystemId, lifecycle => lifecycle.MarkStopped(), cancellationToken);

    public Task<SystemLifecycleDto?> MarkSystemFailedAsync(
        MarkSystemFailedCommand command, CancellationToken cancellationToken = default) =>
        UpdateSystemLifecycleAsync(command.SystemId, lifecycle => lifecycle.MarkFailed(), cancellationToken);

    public Task<SystemLifecycleDto?> MarkSystemUnknownAsync(
        MarkSystemUnknownCommand command, CancellationToken cancellationToken = default) =>
        UpdateSystemLifecycleAsync(command.SystemId, lifecycle => lifecycle.MarkUnknown(), cancellationToken);

    public Task<SystemLifecycleDto?> DeleteSystemLifecycleAsync(
        DeleteSystemLifecycleCommand command, CancellationToken cancellationToken = default) =>
        UpdateSystemLifecycleAsync(command.SystemId, lifecycle => lifecycle.Delete(), cancellationToken);

    // =================================================================================================================
    // SYSTEM LIFECYCLE OPERATION COMMANDS
    public async Task<SystemLifecycleOperationDto?> CreateSystemLifecycleOperationAsync(
        CreateSystemLifecycleOperationCommand command,
        CancellationToken cancellationToken = default)
    {
        var systemId = SystemId.Create(command.SystemId);
        var lifecycle = await _systemLifecycleRepository.GetBySystemIdAsync(systemId, cancellationToken);
        if (lifecycle is null) return null;
        var operation = SystemLifecycleOperation.Create(systemId, ConvertSystemLifecycleOperationTypeToDomain(command.Type));
        await _systemLifecycleOperationRepository.AddAsync(operation, cancellationToken);
        return ConvertSystemLifecycleOperationEntitiesToDto(operation);
    }

    public Task<SystemLifecycleOperationDto?> StartSystemLifecycleOperationAsync(
        StartSystemLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        UpdateSystemLifecycleOperationAsync(command.OperationId, operation => operation.Start(), cancellationToken);

    public Task<SystemLifecycleOperationDto?> CompleteSystemLifecycleOperationAsync(
        CompleteSystemLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        UpdateSystemLifecycleOperationAsync(command.OperationId, operation => operation.Complete(), cancellationToken);

    public Task<SystemLifecycleOperationDto?> FailSystemLifecycleOperationAsync(
        FailSystemLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        UpdateSystemLifecycleOperationAsync(command.OperationId, operation => operation.Fail(command.ErrorMessage), cancellationToken);

    public Task<SystemLifecycleOperationDto?> CancelSystemLifecycleOperationAsync(
        CancelSystemLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        UpdateSystemLifecycleOperationAsync(command.OperationId, operation => operation.Cancel(), cancellationToken);

    public Task<SystemLifecycleOperationDto?> DeleteSystemLifecycleOperationAsync(
        DeleteSystemLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        UpdateSystemLifecycleOperationAsync(command.OperationId, operation => operation.Delete(), cancellationToken);

    // =================================================================================================================
    // PRIVATE HELPERS
    private async Task<SystemLifecycle?> GetSystemLifecycleAsync(
        Guid systemId, CancellationToken cancellationToken)
    {
        return await _systemLifecycleRepository.GetBySystemIdAsync(SystemId.Create(systemId), cancellationToken);
    }

    private async Task<SystemLifecycleDto?> UpdateSystemLifecycleAsync(
        Guid systemId, Action<SystemLifecycle> update,
        CancellationToken cancellationToken)
    {
        var lifecycle = await GetSystemLifecycleAsync(systemId, cancellationToken);
        if (lifecycle is null) return null;
        update(lifecycle);
        await _systemLifecycleRepository.UpdateAsync(lifecycle, cancellationToken);
        return ConvertSystemLifecycleEntitiesToDto(lifecycle);
    }

    private async Task<SystemLifecycleOperationDto?> UpdateSystemLifecycleOperationAsync(
        Guid operationId, Action<SystemLifecycleOperation> update,
        CancellationToken cancellationToken)
    {
        var operation = await _systemLifecycleOperationRepository.GetByIdAsync(operationId, cancellationToken);
        if (operation is null) return null;
        update(operation);
        await _systemLifecycleOperationRepository.UpdateAsync(operation, cancellationToken);
        return ConvertSystemLifecycleOperationEntitiesToDto(operation);
    }

    // =================================================================================================================
    // ENUM CONVERSION: APPLICATION -> DOMAIN
    public static DomainDesiredState ConvertSystemDesiredStateToDomain(
        SystemDesiredStateFilter desiredState)
    {
        return desiredState switch
        {
            SystemDesiredStateFilter.Running => DomainDesiredState.Running,
            SystemDesiredStateFilter.Stopped => DomainDesiredState.Stopped,
            SystemDesiredStateFilter.Unknown => DomainDesiredState.Unknown,
            _ => throw new ArgumentOutOfRangeException(nameof(desiredState))
        };
    }

    public static DomainOperationType ConvertSystemLifecycleOperationTypeToDomain(
        SystemLifecycleOperationTypeFilter type)
    {
        return type switch
        {
            SystemLifecycleOperationTypeFilter.Start => DomainOperationType.Start,
            SystemLifecycleOperationTypeFilter.Stop => DomainOperationType.Stop,
            SystemLifecycleOperationTypeFilter.Restart => DomainOperationType.Restart,
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }

    // =================================================================================================================
    // DTO CONVERSION
    public static SystemLifecycleDto ConvertSystemLifecycleEntitiesToDto(
        SystemLifecycle lifecycle)
    {
        return new SystemLifecycleDto
        {
            SystemId = lifecycle.SystemId.Value,
            DesiredState = lifecycle.DesiredState.ToString(),
            OperationalStatus = lifecycle.OperationalStatus.ToString(),
            CreatedAtUtc = lifecycle.CreatedAtUtc,
            UpdatedAtUtc = lifecycle.UpdatedAtUtc,
            IsDeleted = lifecycle.IsDeleted
        };
    }

    public static List<SystemLifecycleDto> ConvertSystemLifecycleEntitiesToDto(
        IReadOnlyList<SystemLifecycle> lifecycles)
    {
        return [.. lifecycles.Select(ConvertSystemLifecycleEntitiesToDto)];
    }

    public static SystemLifecycleOperationDto ConvertSystemLifecycleOperationEntitiesToDto(
        SystemLifecycleOperation operation)
    {
        return new SystemLifecycleOperationDto
        {
            Id = operation.Id,
            SystemId = operation.SystemId.Value,
            Type = operation.Type.ToString(),
            Status = operation.Status.ToString(),
            CreatedAtUtc = operation.CreatedAtUtc,
            UpdatedAtUtc = operation.UpdatedAtUtc,
            StartedAtUtc = operation.StartedAtUtc,
            CompletedAtUtc = operation.CompletedAtUtc,
            ErrorMessage = operation.ErrorMessage,
            IsDeleted = operation.IsDeleted
        };
    }

    public static List<SystemLifecycleOperationDto> ConvertSystemLifecycleOperationEntitiesToDto(
        IReadOnlyList<SystemLifecycleOperation> operations)
    {
        return [.. operations.Select(ConvertSystemLifecycleOperationEntitiesToDto)];
    }
}

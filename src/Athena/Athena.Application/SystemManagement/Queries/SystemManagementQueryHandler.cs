using Athena.Application.SystemManagement.Dtos;
using Athena.Application.SystemManagement.Enums;
using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Management.Repositories;
using Athena.Domain.System.Registry.ValueObjects;
using DomainDesiredState = Athena.Domain.System.Management.Enums.SystemDesiredState;
using DomainOperationalStatus = Athena.Domain.System.Management.Enums.SystemOperationalStatus;
using DomainOperationStatus = Athena.Domain.System.Management.Enums.SystemLifecycleOperationStatus;

namespace Athena.Application.SystemManagement.Queries;

public class SystemManagementQueryHandler(
    ISystemLifecycleRepository systemLifecycleRepository,
    ISystemLifecycleOperationRepository systemLifecycleOperationRepository)
{
    private readonly ISystemLifecycleRepository _systemLifecycleRepository = systemLifecycleRepository;
    private readonly ISystemLifecycleOperationRepository _systemLifecycleOperationRepository = systemLifecycleOperationRepository;

    // =================================================================================================================
    // SYSTEM LIFECYCLE QUERIES
    public async Task<SystemLifecycleDto?> GetSystemLifecycleBySystemIdAsync(
        GetSystemLifecycleBySystemIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var lifecycle = await _systemLifecycleRepository.GetBySystemIdAsync(SystemId.Create(query.SystemId), cancellationToken);
        return lifecycle is null ? null : ConvertSystemLifecycleEntitiesToDto(lifecycle);
    }

    public async Task<List<SystemLifecycleDto>> GetAllSystemLifecyclesAsync(
        CancellationToken cancellationToken = default)
    {
        var lifecycles = await _systemLifecycleRepository.GetAllAsync(cancellationToken);
        return ConvertSystemLifecycleEntitiesToDto(lifecycles);
    }

    public async Task<List<SystemLifecycleDto>> GetSystemLifecyclesByDesiredStateAsync(
        GetSystemLifecyclesByDesiredStateQuery query,
        CancellationToken cancellationToken = default)
    {
        var desiredState = ConvertSystemDesiredStateToDomain(query.DesiredState);
        var lifecycles = await _systemLifecycleRepository.GetByDesiredStateAsync(desiredState, cancellationToken);
        return ConvertSystemLifecycleEntitiesToDto(lifecycles);
    }

    public async Task<List<SystemLifecycleDto>> GetSystemLifecyclesByOperationalStatusAsync(
        GetSystemLifecyclesByOperationalStatusQuery query,
        CancellationToken cancellationToken = default)
    {
        var operationalStatus = ConvertSystemOperationalStatusToDomain(query.OperationalStatus);
        var lifecycles = await _systemLifecycleRepository.GetByOperationalStatusAsync(operationalStatus, cancellationToken);
        return ConvertSystemLifecycleEntitiesToDto(lifecycles);
    }

    public async Task<List<SystemLifecycleDto>> GetSystemLifecyclesByDeletionStatusAsync(
        GetSystemLifecyclesByDeletionStatusQuery query,
        CancellationToken cancellationToken = default)
    {
        var lifecycles = await _systemLifecycleRepository.GetByDeletionStatusAsync(query.IsDeleted, cancellationToken);
        return ConvertSystemLifecycleEntitiesToDto(lifecycles);
    }

    public async Task<List<SystemLifecycleDto>> GetDeletedSystemLifecyclesAsync(
        CancellationToken cancellationToken = default)
    {
        var lifecycles = await _systemLifecycleRepository.GetByDeletionStatusAsync(true, cancellationToken);
        return ConvertSystemLifecycleEntitiesToDto(lifecycles);
    }

    // =================================================================================================================
    // SYSTEM LIFECYCLE OPERATION QUERIES
    public async Task<SystemLifecycleOperationDto?> GetSystemLifecycleOperationByIdAsync(
        GetSystemLifecycleOperationByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var operation = await _systemLifecycleOperationRepository.GetByIdAsync(query.OperationId, cancellationToken);
        return operation is null ? null : ConvertSystemLifecycleOperationEntitiesToDto(operation);
    }

    public async Task<List<SystemLifecycleOperationDto>> GetSystemLifecycleOperationsBySystemIdAsync(
        GetSystemLifecycleOperationsBySystemIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var operations = await _systemLifecycleOperationRepository.GetBySystemIdAsync(SystemId.Create(query.SystemId), cancellationToken);
        return ConvertSystemLifecycleOperationEntitiesToDto(operations);
    }

    public async Task<List<SystemLifecycleOperationDto>> GetSystemLifecycleOperationsByStatusAsync(
        GetSystemLifecycleOperationsByStatusQuery query,
        CancellationToken cancellationToken = default)
    {
        var status = ConvertSystemLifecycleOperationStatusToDomain(query.Status);
        var operations = await _systemLifecycleOperationRepository.GetByStatusAsync(status, cancellationToken);
        return ConvertSystemLifecycleOperationEntitiesToDto(operations);
    }

    public async Task<List<SystemLifecycleOperationDto>> GetSystemLifecycleOperationsBySystemIdAndStatusAsync(
        GetSystemLifecycleOperationsBySystemIdAndStatusQuery query,
        CancellationToken cancellationToken = default)
    {
        var systemId = SystemId.Create(query.SystemId);
        var status = ConvertSystemLifecycleOperationStatusToDomain(query.Status);
        var operations = await _systemLifecycleOperationRepository.GetBySystemIdAndStatusAsync(systemId, status, cancellationToken);
        return ConvertSystemLifecycleOperationEntitiesToDto(operations);
    }

    public async Task<List<SystemLifecycleOperationDto>> GetSystemLifecycleOperationsByDeletionStatusAsync(
        GetSystemLifecycleOperationsByDeletionStatusQuery query,
        CancellationToken cancellationToken = default)
    {
        var operations = await _systemLifecycleOperationRepository.GetByDeletionStatusAsync(query.IsDeleted, cancellationToken);
        return ConvertSystemLifecycleOperationEntitiesToDto(operations);
    }

    public async Task<List<SystemLifecycleOperationDto>> GetDeletedSystemLifecycleOperationsAsync(
        CancellationToken cancellationToken = default)
    {
        var operations = await _systemLifecycleOperationRepository.GetByDeletionStatusAsync(true, cancellationToken);
        return ConvertSystemLifecycleOperationEntitiesToDto(operations);
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

    public static DomainOperationalStatus ConvertSystemOperationalStatusToDomain(
        SystemOperationalStatusFilter status)
    {
        return status switch
        {
            SystemOperationalStatusFilter.Unknown => DomainOperationalStatus.Unknown,
            SystemOperationalStatusFilter.Starting => DomainOperationalStatus.Starting,
            SystemOperationalStatusFilter.Running => DomainOperationalStatus.Running,
            SystemOperationalStatusFilter.Stopping => DomainOperationalStatus.Stopping,
            SystemOperationalStatusFilter.Stopped => DomainOperationalStatus.Stopped,
            SystemOperationalStatusFilter.Failed => DomainOperationalStatus.Failed,
            _ => throw new ArgumentOutOfRangeException(nameof(status))
        };
    }

    public static DomainOperationStatus ConvertSystemLifecycleOperationStatusToDomain(
        SystemLifecycleOperationStatusFilter status)
    {
        return status switch
        {
            SystemLifecycleOperationStatusFilter.Pending => DomainOperationStatus.Pending,
            SystemLifecycleOperationStatusFilter.Running => DomainOperationStatus.Running,
            SystemLifecycleOperationStatusFilter.Completed => DomainOperationStatus.Completed,
            SystemLifecycleOperationStatusFilter.Failed => DomainOperationStatus.Failed,
            SystemLifecycleOperationStatusFilter.Cancelled => DomainOperationStatus.Cancelled,
            _ => throw new ArgumentOutOfRangeException(nameof(status))
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

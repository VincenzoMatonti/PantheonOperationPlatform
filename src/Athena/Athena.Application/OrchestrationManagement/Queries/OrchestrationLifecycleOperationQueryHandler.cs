using Athena.Application.OrchestrationManagement.DTOs;
using Athena.Domain.OrchestrationManagement.Entities;
using Athena.Domain.OrchestrationManagement.Repositories;
using Athena.Domain.OrchestrationRegistry.ValueObjects;
using ApplicationOperationType = Athena.Application.OrchestrationManagement.Enums.OrchestrationLifecycleOperationType;
using ApplicationOperationStatus = Athena.Application.OrchestrationManagement.Enums.OrchestrationLifecycleOperationStatus;
using DomainOperationType = Athena.Domain.OrchestrationManagement.Enums.OrchestrationLifecycleOperationType;
using DomainOperationStatus = Athena.Domain.OrchestrationManagement.Enums.OrchestrationLifecycleOperationStatus;

namespace Athena.Application.OrchestrationManagement.Queries;

public class OrchestrationLifecycleOperationQueryHandler(IOrchestrationLifecycleOperationRepository orchestrationLifecycleOperationRepository)
{
    private readonly IOrchestrationLifecycleOperationRepository _orchestrationLifecycleOperationRepository = orchestrationLifecycleOperationRepository;

    public async Task<OrchestrationLifecycleOperationDto?> GetOrchestrationLifecycleOperationByIdAsync(
            GetOrchestrationLifecycleOperationByIdQuery query, CancellationToken cancellationToken = default)
    {
        var operation = await _orchestrationLifecycleOperationRepository.GetByIdAsync(query.Id, cancellationToken);
        return operation is null ? null : ConvertOrchestrationLifecycleOperationEntitiesToDto(operation);
    }

    public async Task<List<OrchestrationLifecycleOperationDto>> GetAllOrchestrationLifecycleOperationsAsync(CancellationToken cancellationToken = default)
    {
        var operations = await _orchestrationLifecycleOperationRepository.GetAllAsync(cancellationToken);
        return [.. operations.Select(ConvertOrchestrationLifecycleOperationEntitiesToDto)];
    }

    public async Task<List<OrchestrationLifecycleOperationDto>> GetOrchestrationLifecycleOperationsByOrchestrationIdAsync(
            GetOrchestrationLifecycleOperationsByOrchestrationIdQuery query, CancellationToken cancellationToken = default)
    {
        var orchestrationId = OrchestrationId.Create(query.OrchestrationId);
        var operations = await _orchestrationLifecycleOperationRepository.GetByOrchestrationIdAsync(orchestrationId, cancellationToken);
        return [.. operations.Select(ConvertOrchestrationLifecycleOperationEntitiesToDto)];
    }

    public async Task<List<OrchestrationLifecycleOperationDto>> GetOrchestrationLifecycleOperationsByTypeAsync(
            GetOrchestrationLifecycleOperationsByTypeQuery query, CancellationToken cancellationToken = default)
    {
        var type = ConvertOperationTypeToDomain(query.Type);
        var operations = await _orchestrationLifecycleOperationRepository.GetByTypeAsync(type, cancellationToken);
        return [.. operations.Select(ConvertOrchestrationLifecycleOperationEntitiesToDto)];
    }

    public async Task<List<OrchestrationLifecycleOperationDto>> GetOrchestrationLifecycleOperationsByStatusAsync(
            GetOrchestrationLifecycleOperationsByStatusQuery query, CancellationToken cancellationToken = default)
    {
        var status = ConvertOperationStatusToDomain(query.Status);
        var operations = await _orchestrationLifecycleOperationRepository.GetByStatusAsync(status, cancellationToken);
        return [.. operations.Select(ConvertOrchestrationLifecycleOperationEntitiesToDto)];
    }

    public async Task<List<OrchestrationLifecycleOperationDto>> GetOrchestrationLifecycleOperationsByOrchestrationIdAndStatusAsync(
            GetOrchestrationLifecycleOperationsByOrchestrationIdAndStatusQuery query, CancellationToken cancellationToken = default)
    {
        var orchestrationId = OrchestrationId.Create(query.OrchestrationId);
        var status = ConvertOperationStatusToDomain(query.Status);
        var operations = await _orchestrationLifecycleOperationRepository.GetByOrchestrationIdAndStatusAsync(orchestrationId, status, cancellationToken);
        return [.. operations.Select(ConvertOrchestrationLifecycleOperationEntitiesToDto)];
    }

    public async Task<List<OrchestrationLifecycleOperationDto>> GetPendingOrchestrationLifecycleOperationsAsync(CancellationToken cancellationToken = default)
    {
        var operations = await _orchestrationLifecycleOperationRepository.GetPendingAsync(cancellationToken);
        return [.. operations.Select(ConvertOrchestrationLifecycleOperationEntitiesToDto)];
    }

    public async Task<List<OrchestrationLifecycleOperationDto>> GetRunningOrchestrationLifecycleOperationsAsync(CancellationToken cancellationToken = default)
    {
        var operations = await _orchestrationLifecycleOperationRepository.GetRunningAsync(cancellationToken);
        return [.. operations.Select(ConvertOrchestrationLifecycleOperationEntitiesToDto)];
    }

    public async Task<List<OrchestrationLifecycleOperationDto>> GetFailedOrchestrationLifecycleOperationsAsync(CancellationToken cancellationToken = default)
    {
        var operations = await _orchestrationLifecycleOperationRepository.GetFailedAsync(cancellationToken);
        return [.. operations.Select(ConvertOrchestrationLifecycleOperationEntitiesToDto)];
    }

    public async Task<List<OrchestrationLifecycleOperationDto>> GetDeletedOrchestrationLifecycleOperationsAsync(CancellationToken cancellationToken = default)
    {
        var operations = await _orchestrationLifecycleOperationRepository.GetDeletedAsync(cancellationToken);
        return [.. operations.Select(ConvertOrchestrationLifecycleOperationEntitiesToDto)];
    }

    // APPLICATION -> DOMAIN
    public static DomainOperationType ConvertOperationTypeToDomain(ApplicationOperationType type) => type switch
    {
        ApplicationOperationType.Start => DomainOperationType.Start,
        ApplicationOperationType.Stop => DomainOperationType.Stop,
        ApplicationOperationType.Restart => DomainOperationType.Restart,
        _ => DomainOperationType.Start
    };

    public static DomainOperationStatus ConvertOperationStatusToDomain(ApplicationOperationStatus status) => status switch
    {
        ApplicationOperationStatus.Pending => DomainOperationStatus.Pending,
        ApplicationOperationStatus.Running => DomainOperationStatus.Running,
        ApplicationOperationStatus.Completed => DomainOperationStatus.Completed,
        ApplicationOperationStatus.Failed => DomainOperationStatus.Failed,
        ApplicationOperationStatus.Cancelled => DomainOperationStatus.Cancelled,
        _ => DomainOperationStatus.Pending
    };

    // DOMAIN -> APPLICATION
    public static ApplicationOperationType ConvertOperationTypeToApplication(DomainOperationType type) => type switch
    {
        DomainOperationType.Start => ApplicationOperationType.Start,
        DomainOperationType.Stop => ApplicationOperationType.Stop,
        DomainOperationType.Restart => ApplicationOperationType.Restart,
        _ => ApplicationOperationType.Start
    };

    public static ApplicationOperationStatus ConvertOperationStatusToApplication(DomainOperationStatus status) => status switch
    {
        DomainOperationStatus.Pending => ApplicationOperationStatus.Pending,
        DomainOperationStatus.Running => ApplicationOperationStatus.Running,
        DomainOperationStatus.Completed => ApplicationOperationStatus.Completed,
        DomainOperationStatus.Failed => ApplicationOperationStatus.Failed,
        DomainOperationStatus.Cancelled => ApplicationOperationStatus.Cancelled,
        _ => ApplicationOperationStatus.Pending
    };

    public static OrchestrationLifecycleOperationDto ConvertOrchestrationLifecycleOperationEntitiesToDto(OrchestrationLifecycleOperation operation) => new()
    {
        Id = operation.Id,
        OrchestrationId = operation.OrchestrationId.Value,
        Type = ConvertOperationTypeToApplication(operation.Type),
        Status = ConvertOperationStatusToApplication(operation.Status),
        CreatedAtUtc = operation.CreatedAtUtc,
        UpdatedAtUtc = operation.UpdatedAtUtc,
        StartedAtUtc = operation.StartedAtUtc,
        CompletedAtUtc = operation.CompletedAtUtc,
        ErrorMessage = operation.ErrorMessage,
        IsDeleted = operation.IsDeleted
    };
}
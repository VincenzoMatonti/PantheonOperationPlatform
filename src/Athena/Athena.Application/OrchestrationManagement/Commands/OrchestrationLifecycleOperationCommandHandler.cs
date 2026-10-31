using Athena.Application.OrchestrationManagement.DTOs;
using Athena.Domain.Orchestration.Management.Entities;
using Athena.Domain.Orchestration.Management.Repositories;
using Athena.Domain.Orchestration.Registry.ValueObjects;
using ApplicationOperationType = Athena.Application.OrchestrationManagement.Enums.OrchestrationLifecycleOperationType;
using ApplicationOperationStatus = Athena.Application.OrchestrationManagement.Enums.OrchestrationLifecycleOperationStatus;
using DomainOperationType = Athena.Domain.Orchestration.Management.Enums.OrchestrationLifecycleOperationType;
using DomainOperationStatus = Athena.Domain.Orchestration.Management.Enums.OrchestrationLifecycleOperationStatus;

namespace Athena.Application.OrchestrationManagement.Commands;

public class OrchestrationLifecycleOperationCommandHandler(
    IOrchestrationLifecycleRepository orchestrationLifecycleRepository,
    IOrchestrationLifecycleOperationRepository orchestrationLifecycleOperationRepository)
{
    private readonly IOrchestrationLifecycleRepository _orchestrationLifecycleRepository = orchestrationLifecycleRepository;
    private readonly IOrchestrationLifecycleOperationRepository _orchestrationLifecycleOperationRepository = orchestrationLifecycleOperationRepository;

    public async Task<OrchestrationLifecycleOperationDto?> CreateOrchestrationLifecycleOperationAsync(
        CreateOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default)
    {
        var orchestrationId = OrchestrationId.Create(command.OrchestrationId);
        var lifecycle = await _orchestrationLifecycleRepository.GetByIdAsync(orchestrationId, cancellationToken);
        if (lifecycle is null) return null;
        var operation = OrchestrationLifecycleOperation.Create(command.Id, orchestrationId, ConvertOperationTypeToDomain(command.Type));
        await _orchestrationLifecycleOperationRepository.AddAsync(operation, cancellationToken);
        return ConvertOrchestrationLifecycleOperationEntitiesToDto(operation);
    }

    public Task<OrchestrationLifecycleOperationDto?> StartOrchestrationLifecycleOperationAsync(
        StartOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        UpdateOrchestrationLifecycleOperationAsync(command.Id, operation => operation.Start(), cancellationToken);

    public Task<OrchestrationLifecycleOperationDto?> CompleteOrchestrationLifecycleOperationAsync(
        CompleteOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        UpdateOrchestrationLifecycleOperationAsync(command.Id, operation => operation.Complete(), cancellationToken);

    public Task<OrchestrationLifecycleOperationDto?> FailOrchestrationLifecycleOperationAsync(
        FailOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        UpdateOrchestrationLifecycleOperationAsync(command.Id, operation => operation.Fail(command.ErrorMessage), cancellationToken);

    public Task<OrchestrationLifecycleOperationDto?> CancelOrchestrationLifecycleOperationAsync(
        CancelOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        UpdateOrchestrationLifecycleOperationAsync(command.Id, operation => operation.Cancel(), cancellationToken);

    public Task<OrchestrationLifecycleOperationDto?> DeleteOrchestrationLifecycleOperationAsync(
        DeleteOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        UpdateOrchestrationLifecycleOperationAsync(command.Id, operation => operation.Delete(), cancellationToken);

    private async Task<OrchestrationLifecycleOperationDto?> UpdateOrchestrationLifecycleOperationAsync(
        Guid operationId, Action<OrchestrationLifecycleOperation> update, CancellationToken cancellationToken)
    {
        var operation = await _orchestrationLifecycleOperationRepository.GetByIdAsync(operationId, cancellationToken);
        if (operation is null) return null;
        update(operation);
        await _orchestrationLifecycleOperationRepository.UpdateAsync(operation, cancellationToken);
        return ConvertOrchestrationLifecycleOperationEntitiesToDto(operation);
    }

    // APPLICATION -> DOMAIN
    public static DomainOperationType ConvertOperationTypeToDomain(ApplicationOperationType type) => type switch
    {
        ApplicationOperationType.Start => DomainOperationType.Start,
        ApplicationOperationType.Stop => DomainOperationType.Stop,
        ApplicationOperationType.Restart => DomainOperationType.Restart,
        _ => DomainOperationType.Start
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

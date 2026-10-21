using Athena.Domain.OrchestrationManagement.Enums;
using Athena.Domain.OrchestrationManagement.Exceptions;
using Athena.Domain.OrchestrationRegistry.ValueObjects;

namespace Athena.Domain.OrchestrationManagement.Entities;

public class OrchestrationLifecycleOperation
{
    public Guid Id { get; private set; }
    public OrchestrationId OrchestrationId { get; private set; } = null!;
    public OrchestrationLifecycleOperationType Type { get; private set; }
    public OrchestrationLifecycleOperationStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public DateTime? StartedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }
    public string? ErrorMessage { get; private set; }
    public bool IsDeleted { get; private set; }

    public static OrchestrationLifecycleOperation Create(Guid id, OrchestrationId orchestrationId, OrchestrationLifecycleOperationType type)
    {
        if (id == Guid.Empty) throw new OrchestrationLifecycleOperationIdRequiredException();
        if (orchestrationId == null) throw new OrchestrationLifecycleOperationOrchestrationIdRequiredException();
        if (!Enum.IsDefined(type)) throw new OrchestrationLifecycleOperationInvalidTypeException();
        return new OrchestrationLifecycleOperation
        {
            Id = id,
            OrchestrationId = orchestrationId,
            Type = type,
            Status = OrchestrationLifecycleOperationStatus.Pending,
            CreatedAtUtc = DateTime.UtcNow,
            IsDeleted = false
        };
    }

    public void Start()
    {
        EnsureNotDeleted();
        EnsureNotTerminal();
        if (Status == OrchestrationLifecycleOperationStatus.Running) throw new OrchestrationLifecycleOperationAlreadyRunningException(Id);
        if (Status != OrchestrationLifecycleOperationStatus.Pending) throw new OrchestrationLifecycleOperationCannotStartFromCurrentStatusException(Id, Status);
        Status = OrchestrationLifecycleOperationStatus.Running;
        StartedAtUtc = DateTime.UtcNow;
        Touch();
    }

    public void Complete()
    {
        EnsureNotDeleted();
        EnsureNotTerminal();
        if (Status != OrchestrationLifecycleOperationStatus.Running) throw new OrchestrationLifecycleOperationCannotCompleteFromCurrentStatusException(Id, Status);
        Status = OrchestrationLifecycleOperationStatus.Completed;
        CompletedAtUtc = DateTime.UtcNow;
        ErrorMessage = null;
        Touch();
    }

    public void Fail(string errorMessage)
    {
        EnsureNotDeleted();
        EnsureNotTerminal();
        if (Status != OrchestrationLifecycleOperationStatus.Running) throw new OrchestrationLifecycleOperationCannotFailFromCurrentStatusException(Id, Status);
        if (string.IsNullOrWhiteSpace(errorMessage)) throw new OrchestrationLifecycleOperationErrorMessageRequiredException();
        Status = OrchestrationLifecycleOperationStatus.Failed;
        ErrorMessage = errorMessage.Trim();
        CompletedAtUtc = DateTime.UtcNow;
        Touch();
    }

    public void Cancel()
    {
        EnsureNotDeleted();
        EnsureNotTerminal();
        if (Status != OrchestrationLifecycleOperationStatus.Pending) throw new OrchestrationLifecycleOperationCannotCancelFromCurrentStatusException(Id, Status);
        Status = OrchestrationLifecycleOperationStatus.Cancelled;
        CompletedAtUtc = DateTime.UtcNow;
        Touch();
    }

    public void Delete()
    {
        if (IsDeleted) throw new OrchestrationLifecycleOperationAlreadyDeletedException(Id);
        IsDeleted = true;
        Touch();
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted)
            throw new OrchestrationLifecycleOperationDeletedCannotBeModifiedException(Id);
    }

    private void EnsureNotTerminal()
    {
        switch (Status)
        {
            case OrchestrationLifecycleOperationStatus.Completed:
                throw new OrchestrationLifecycleOperationAlreadyCompletedException(Id);

            case OrchestrationLifecycleOperationStatus.Failed:
                throw new OrchestrationLifecycleOperationAlreadyFailedException(Id);

            case OrchestrationLifecycleOperationStatus.Cancelled:
                throw new OrchestrationLifecycleOperationAlreadyCancelledException(Id);
        }
    }

    private void Touch()
    {
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
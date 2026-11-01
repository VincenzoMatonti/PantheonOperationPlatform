using Athena.Domain.Orchestration.Management.Exceptions;
using Athena.Domain.Orchestration.Management.ValueObjects;
using Athena.Domain.Orchestration.Registry.ValueObjects;
using Athena.Domain.System.Registry.ValueObjects;

namespace Athena.Domain.Orchestration.Management.Entities;

public class OrchestrationSystemInstance
{
    public OrchestrationSystemInstanceId Id { get; private set; } = null!;

    public OrchestrationId OrchestrationId { get; private set; } = null!;

    public SystemInstanceId SystemInstanceId { get; private set; } = null!;

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? UpdatedAtUtc { get; private set; }

    public bool IsDeleted { get; private set; }

    public static OrchestrationSystemInstance Create(OrchestrationId orchestrationId, SystemInstanceId systemInstanceId)
    {
        if (orchestrationId is null) throw new OrchestrationSystemInstanceOrchestrationIdRequiredException();
        if (systemInstanceId is null) throw new OrchestrationSystemInstanceSystemInstanceIdRequiredException();
        return new OrchestrationSystemInstance
        {
            Id = OrchestrationSystemInstanceId.New(),
            OrchestrationId = orchestrationId,
            SystemInstanceId = systemInstanceId,
            CreatedAtUtc = DateTime.UtcNow,
            IsDeleted = false
        };
    }

    public void Delete()
    {
        if (IsDeleted) throw new OrchestrationSystemInstanceAlreadyDeletedException(Id);
        IsDeleted = true;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Restore()
    {
        if (!IsDeleted) throw new OrchestrationSystemInstanceAlreadyRestoredException(Id);
        IsDeleted = false;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
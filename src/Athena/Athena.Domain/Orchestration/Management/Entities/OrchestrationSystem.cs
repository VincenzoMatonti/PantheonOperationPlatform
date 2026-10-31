using Athena.Domain.Orchestration.Management.Exceptions;
using Athena.Domain.Orchestration.Management.ValueObjects;
using Athena.Domain.Orchestration.Registry.ValueObjects;
using Athena.Domain.System.Registry.ValueObjects;

namespace Athena.Domain.Orchestration.Management.Entities;

public class OrchestrationSystem
{
    public OrchestrationSystemId Id { get; private set; } = null!;

    public OrchestrationId OrchestrationId { get; private set; } = null!;

    public SystemId SystemId { get; private set; } = null!;

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? UpdatedAtUtc { get; private set; }

    public bool IsDeleted { get; private set; }

    public static OrchestrationSystem Create(OrchestrationId orchestrationId, SystemId systemId)
    {
        if (orchestrationId is null) throw new OrchestrationSystemOrchestrationIdRequiredException();
        if (systemId is null) throw new OrchestrationSystemSystemIdRequiredException();
        return new OrchestrationSystem
        {
            Id = OrchestrationSystemId.New(),
            OrchestrationId = orchestrationId,
            SystemId = systemId,
            CreatedAtUtc = DateTime.UtcNow,
            IsDeleted = false
        };
    }

    public void Delete()
    {
        if (IsDeleted) throw new OrchestrationSystemAlreadyDeletedException(Id);
        IsDeleted = true;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Restore()
    {
        if (!IsDeleted) throw new OrchestrationSystemAlreadyRestoredException(Id);
        IsDeleted = false;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
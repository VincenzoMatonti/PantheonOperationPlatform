
using Athena.Domain.OrchestrationManagement.Enums;

namespace Athena.Domain.OrchestrationManagement.Exceptions;

public class OrchestrationLifecycleIdRequiredException() : OrchestrationLifecycleException(
        OrchestrationLifecycleErrorCode.OrchestrationIdRequired, "An orchestration ID is required.");

public class OrchestrationLifecycleAlreadyDeletedException(Guid orchestrationId) : OrchestrationLifecycleException(
        OrchestrationLifecycleErrorCode.OrchestrationLifecycleAlreadyDeleted,
        $"The lifecycle for orchestration '{orchestrationId}' has already been deleted.");

public class OrchestrationLifecycleDesiredStateAlreadySetException(Guid orchestrationId, OrchestrationDesiredState desiredState) :
        OrchestrationLifecycleException(OrchestrationLifecycleErrorCode.DesiredStateAlreadySet,
        $"The desired state '{desiredState}' is already set for orchestration '{orchestrationId}'.");

public class OrchestrationLifecycleAlreadyInOperationalStatusException(Guid orchestrationId, OrchestrationOperationalStatus status)
        : OrchestrationLifecycleException(GetErrorCode(status),
        $"The lifecycle for orchestration '{orchestrationId}' is already in operational status '{status}'.")
{
    private static OrchestrationLifecycleErrorCode GetErrorCode(OrchestrationOperationalStatus status) => status switch
    {
        OrchestrationOperationalStatus.Starting => OrchestrationLifecycleErrorCode.AlreadyStarting,
        OrchestrationOperationalStatus.Running => OrchestrationLifecycleErrorCode.AlreadyRunning,
        OrchestrationOperationalStatus.Stopping => OrchestrationLifecycleErrorCode.AlreadyStopping,
        OrchestrationOperationalStatus.Stopped => OrchestrationLifecycleErrorCode.AlreadyStopped,
        OrchestrationOperationalStatus.Failed => OrchestrationLifecycleErrorCode.AlreadyFailed,
        OrchestrationOperationalStatus.Unknown => OrchestrationLifecycleErrorCode.AlreadyOperationalStatusUnknown,
        _ => OrchestrationLifecycleErrorCode.InvalidOperationalStatus
    };
}

public class OrchestrationLifecycleInvalidDesiredStateException() : OrchestrationLifecycleException(
        OrchestrationLifecycleErrorCode.InvalidDesiredState, "The specified orchestration lifecycle desired state is invalid.");

public class OrchestrationLifecycleInvalidOperationalStatusException() : OrchestrationLifecycleException(
        OrchestrationLifecycleErrorCode.InvalidOperationalStatus, "The specified orchestration lifecycle operational status is invalid.");

public class OrchestrationLifecycleDeletedCannotBeModifiedException(Guid orchestrationId) : OrchestrationLifecycleException(
        OrchestrationLifecycleErrorCode.OrchestrationLifecycleDeletedCannotBeModified,
        $"The lifecycle for orchestration '{orchestrationId}' cannot be modified because it has been deleted.");
using Athena.Domain.SystemManagement.Enums;

namespace Athena.Domain.SystemManagement.Exceptions;

public class SystemLifecycleSystemIdRequiredException() : SystemLifecycleException(
        SystemLifecycleErrorCode.SystemIdRequired, "A system ID is required to create a system lifecycle.")
{
}

public class SystemLifecycleInvalidDesiredStateException() : SystemLifecycleException(
        SystemLifecycleErrorCode.InvalidDesiredState, "The specified desired state is invalid.")
{
}

public class SystemLifecycleDesiredStateAlreadySetException(Guid systemId, string desiredState) : SystemLifecycleException(
        SystemLifecycleErrorCode.DesiredStateAlreadySet, $"System '{systemId}' already has desired state '{desiredState}'.")
{
}

public class SystemLifecycleAlreadyStartingException(Guid systemId) : SystemLifecycleException(
        SystemLifecycleErrorCode.AlreadyStarting, $"System '{systemId}' is already starting.")
{
}

public class SystemLifecycleAlreadyRunningException(Guid systemId) : SystemLifecycleException(
        SystemLifecycleErrorCode.AlreadyRunning, $"System '{systemId}' is already running.")
{
}

public class SystemLifecycleAlreadyStoppingException(Guid systemId) : SystemLifecycleException(
        SystemLifecycleErrorCode.AlreadyStopping, $"System '{systemId}' is already stopping.")
{
}

public class SystemLifecycleAlreadyStoppedException(Guid systemId) : SystemLifecycleException(
        SystemLifecycleErrorCode.AlreadyStopped, $"System '{systemId}' is already stopped.")
{
}

public class SystemLifecycleAlreadyFailedException(Guid systemId) : SystemLifecycleException(
        SystemLifecycleErrorCode.AlreadyFailed, $"System '{systemId}' is already in a failed state.")
{
}

public class SystemLifecycleAlreadyUnknownException(Guid systemId) : SystemLifecycleException(
        SystemLifecycleErrorCode.AlreadyUnknown, $"System '{systemId}' already has an unknown operational status.")
{
}

public class SystemLifecycleAlreadyDeletedException(Guid systemId) : SystemLifecycleException(
        SystemLifecycleErrorCode.AlreadyDeleted, $"The lifecycle for system '{systemId}' has already been deleted.")
{
}

public class SystemLifecycleDeletedCannotBeModifiedException(Guid systemId) : SystemLifecycleException(
        SystemLifecycleErrorCode.DeletedLifecycleCannotBeModified, $"The lifecycle for system '{systemId}' cannot be modified because it has been deleted.")
{
}

public class SystemLifecycleInvalidOperationalTransitionException(Guid systemId, SystemOperationalStatus currentStatus, SystemOperationalStatus requiredStatus)
        : SystemLifecycleException(SystemLifecycleErrorCode.InvalidOperationalTransition,
        $"System '{systemId}' cannot perform the requested transition from status '{currentStatus}'. Required status: '{requiredStatus}'.")
{
}

public class SystemLifecycleCannotBeDeletedWhileTransitioningException(Guid systemId, SystemOperationalStatus operationalStatus)
        : SystemLifecycleException(SystemLifecycleErrorCode.OperationInProgress,
        $"The lifecycle for system '{systemId}' cannot be deleted while its operational status is '{operationalStatus}'.")
{
}
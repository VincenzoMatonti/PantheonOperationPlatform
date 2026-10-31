using Athena.Domain.Orchestration.Management.ValueObjects;

namespace Athena.Domain.Orchestration.Management.Exceptions;

public class OrchestrationSystemOrchestrationIdRequiredException() : OrchestrationSystemException(
    OrchestrationSystemErrorCode.OrchestrationIdRequired, "An orchestration ID is required to create an orchestration-system association.");

public class OrchestrationSystemSystemIdRequiredException() : OrchestrationSystemException(
    OrchestrationSystemErrorCode.SystemIdRequired, "A system ID is required to create an orchestration-system association.");

public class OrchestrationSystemAlreadyDeletedException(OrchestrationSystemId id) : OrchestrationSystemException(
    OrchestrationSystemErrorCode.OrchestrationSystemAlreadyDeleted,
    $"The orchestration-system association '{id}' has already been deleted.");

public class OrchestrationSystemDeletedCannotBeModifiedException(OrchestrationSystemId id) : OrchestrationSystemException(
    OrchestrationSystemErrorCode.OrchestrationSystemDeletedCannotBeModified,
    $"The orchestration-system association '{id}' cannot be modified because it has been deleted.");

public class OrchestrationSystemAlreadyRestoredException(OrchestrationSystemId id) : OrchestrationSystemException(
    OrchestrationSystemErrorCode.OrchestrationSystemAlreadyRestored, $"The orchestration-system association '{id}' is already active.");
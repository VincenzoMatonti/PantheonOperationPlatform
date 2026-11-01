using Athena.Domain.Orchestration.Management.ValueObjects;

namespace Athena.Domain.Orchestration.Management.Exceptions;

public class OrchestrationSystemInstanceOrchestrationIdRequiredException() : OrchestrationSystemInstanceException(
    OrchestrationSystemInstanceErrorCode.OrchestrationIdRequired, "An orchestration ID is required to create an orchestration-system association.");

public class OrchestrationSystemInstanceSystemInstanceIdRequiredException() : OrchestrationSystemInstanceException(
    OrchestrationSystemInstanceErrorCode.SystemInstanceIdRequired, "A system ID is required to create an orchestration-system association.");

public class OrchestrationSystemInstanceAlreadyDeletedException(OrchestrationSystemInstanceId id) : OrchestrationSystemInstanceException(
    OrchestrationSystemInstanceErrorCode.OrchestrationSystemInstanceAlreadyDeleted,
    $"The orchestration-system association '{id}' has already been deleted.");

public class OrchestrationSystemInstanceDeletedCannotBeModifiedException(OrchestrationSystemInstanceId id) : OrchestrationSystemInstanceException(
    OrchestrationSystemInstanceErrorCode.OrchestrationSystemInstanceDeletedCannotBeModified,
    $"The orchestration-system association '{id}' cannot be modified because it has been deleted.");

public class OrchestrationSystemInstanceAlreadyRestoredException(OrchestrationSystemInstanceId id) : OrchestrationSystemInstanceException(
    OrchestrationSystemInstanceErrorCode.OrchestrationSystemInstanceAlreadyRestored, $"The orchestration-system association '{id}' is already active.");
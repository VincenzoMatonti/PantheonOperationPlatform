using Athena.Application.OrchestrationManagement.Enums;

namespace Athena.Application.OrchestrationManagement.Commands;

public class CreateOrchestrationLifecycleOperationCommand
{
    public Guid Id { get; init; }

    public Guid OrchestrationId { get; init; }

    public OrchestrationLifecycleOperationType Type { get; init; }
}

public class StartOrchestrationLifecycleOperationCommand
{
    public Guid Id { get; init; }
}

public class CompleteOrchestrationLifecycleOperationCommand
{
    public Guid Id { get; init; }
}

public class FailOrchestrationLifecycleOperationCommand
{
    public Guid Id { get; init; }

    public string ErrorMessage { get; init; } = string.Empty;
}

public class CancelOrchestrationLifecycleOperationCommand
{
    public Guid Id { get; init; }
}

public class DeleteOrchestrationLifecycleOperationCommand
{
    public Guid Id { get; init; }
}

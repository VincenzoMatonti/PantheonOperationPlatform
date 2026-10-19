

using Athena.Application.OrchestrationManagement.Enums;

namespace Athena.Application.OrchestrationManagement.Commands;

public class SetOrchestrationDesiredStateCommand
{
    public Guid OrchestrationId { get; init; }

    public OrchestrationDesiredState DesiredState { get; init; }
}

public class MarkOrchestrationStartingCommand
{
    public Guid OrchestrationId { get; init; }
}

public class MarkOrchestrationRunningCommand
{
    public Guid OrchestrationId { get; init; }
}

public class MarkOrchestrationStoppingCommand
{
    public Guid OrchestrationId { get; init; }
}

public class MarkOrchestrationStoppedCommand
{
    public Guid OrchestrationId { get; init; }
}

public class MarkOrchestrationFailedCommand
{
    public Guid OrchestrationId { get; init; }
}

public class MarkOrchestrationOperationalStatusUnknownCommand
{
    public Guid OrchestrationId { get; init; }
}

public class DeleteOrchestrationLifecycleCommand
{
    public Guid OrchestrationId { get; init; }
}

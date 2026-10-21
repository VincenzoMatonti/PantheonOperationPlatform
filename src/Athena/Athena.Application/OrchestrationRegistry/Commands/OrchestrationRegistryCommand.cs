namespace Athena.Application.OrchestrationRegistry.Commands;

public class CreateOrchestrationCommand
{
    public Guid OrchestrationId { get; init; }

    public string Name { get; init; } = string.Empty;
}

public class RegisterOrchestrationCommand
{
    public Guid OrchestrationId { get; init; }
}

public class UnregisterOrchestrationCommand
{
    public Guid OrchestrationId { get; init; }
}

public class EnableOrchestrationCommand
{
    public Guid OrchestrationId { get; init; }
}

public class DisableOrchestrationCommand
{
    public Guid OrchestrationId { get; init; }
}

public class RenameOrchestrationCommand
{
    public Guid OrchestrationId { get; init; }

    public string Name { get; init; } = string.Empty;
}

public class MarkOrchestrationAvailableCommand
{
    public Guid OrchestrationId { get; init; }
}

public class MarkOrchestrationUnavailableCommand
{
    public Guid OrchestrationId { get; init; }
}

public class MarkOrchestrationAvailabilityUnknownCommand
{
    public Guid OrchestrationId { get; init; }
}

public class DeleteOrchestrationCommand
{
    public Guid OrchestrationId { get; init; }
}
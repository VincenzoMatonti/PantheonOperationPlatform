using Athena.Application.SystemManagement.Enums;

namespace Athena.Application.SystemManagement.Commands;

public class CreateSystemLifecycleCommand
{
    public Guid SystemId { get; set; }
}

public class SetSystemDesiredStateCommand
{
    public Guid SystemId { get; set; }
    public SystemDesiredStateFilter DesiredState { get; set; }
}

public class MarkSystemStartingCommand
{
    public Guid SystemId { get; set; }
}

public class MarkSystemRunningCommand
{
    public Guid SystemId { get; set; }
}

public class MarkSystemStoppingCommand
{
    public Guid SystemId { get; set; }
}

public class MarkSystemStoppedCommand
{
    public Guid SystemId { get; set; }
}

public class MarkSystemFailedCommand
{
    public Guid SystemId { get; set; }
}

public class MarkSystemUnknownCommand
{
    public Guid SystemId { get; set; }
}

public class DeleteSystemLifecycleCommand
{
    public Guid SystemId { get; set; }
}
namespace Athena.Application.OrchestrationManagement.Commands;

public class CreateOrchestrationSystemCommand
{
    public Guid OrchestrationId { get; set; }
    public Guid SystemId { get; set; }
}

public class DeleteOrchestrationSystemCommand
{
    public Guid Id { get; set; }
}

public class RestoreOrchestrationSystemCommand
{
    public Guid Id { get; set; }
}
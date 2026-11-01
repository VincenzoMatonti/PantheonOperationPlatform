namespace Athena.Application.OrchestrationManagement.Commands;

public class CreateOrchestrationSystemInstanceCommand
{
    public Guid OrchestrationId { get; set; }
    public Guid SystemInstanceId { get; set; }
}

public class DeleteOrchestrationSystemInstanceCommand
{
    public Guid Id { get; set; }
}

public class RestoreOrchestrationSystemInstanceCommand
{
    public Guid Id { get; set; }
}
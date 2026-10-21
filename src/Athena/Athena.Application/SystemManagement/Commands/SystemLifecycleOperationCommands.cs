using Athena.Application.SystemManagement.Enums;

namespace Athena.Application.SystemManagement.Commands;

public class CreateSystemLifecycleOperationCommand
{
    public Guid SystemId { get; set; }
    public SystemLifecycleOperationTypeFilter Type { get; set; }
}

public class StartSystemLifecycleOperationCommand
{
    public Guid OperationId { get; set; }
}

public class CompleteSystemLifecycleOperationCommand
{
    public Guid OperationId { get; set; }
}

public class FailSystemLifecycleOperationCommand
{
    public Guid OperationId { get; set; }
    public required string ErrorMessage { get; set; }
}

public class CancelSystemLifecycleOperationCommand
{
    public Guid OperationId { get; set; }
}

public class DeleteSystemLifecycleOperationCommand
{
    public Guid OperationId { get; set; }
}
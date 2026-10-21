using Athena.Application.SystemManagement.Enums;

namespace Athena.Api.SystemManagement.Requests;

public class SetSystemDesiredStateRequest
{
    public SystemDesiredStateFilter DesiredState { get; init; }
}

public class CreateSystemLifecycleOperationRequest
{
    public SystemLifecycleOperationTypeFilter Type { get; init; }
}

public class FailSystemLifecycleOperationRequest
{
    public required string ErrorMessage { get; init; }
}
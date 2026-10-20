namespace Athena.Cli.Application.Contracts.Systems;

public sealed record SetSystemDesiredStateRequest(string DesiredState);

public sealed record CreateSystemLifecycleOperationRequest(string Type);

public sealed record FailSystemLifecycleOperationRequest(string ErrorMessage);

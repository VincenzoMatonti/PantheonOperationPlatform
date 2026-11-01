namespace Athena.Application.EcosystemManagement.Commands;

public sealed record CreateEcosystemLifecycleCommand(Guid EcosystemId);
public sealed record SetEcosystemDesiredStateCommand(Guid EcosystemId, string DesiredState);
public sealed record ExecuteEcosystemOperationCommand(Guid EcosystemId, string OperationType);

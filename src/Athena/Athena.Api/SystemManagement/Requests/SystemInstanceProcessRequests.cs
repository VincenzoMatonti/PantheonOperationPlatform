namespace Athena.Api.SystemManagement.Requests;

public sealed record CreateSystemInstanceRequest(
    Guid SystemInstanceId,
    Guid? SystemId,
    Guid? WorkloadId,
    string? Name,
    string? Details,
    string? ConfigurationReference);

public sealed record RegisterSystemInstanceRequest(
    Guid SystemInstanceId,
    string? Details,
    string? ConfigurationReference);

public sealed record ConfigureSystemInstanceRequest(string ConfigurationReference);
public sealed record ResolveSystemInstanceOperationRequest(string Reason);
public sealed record GetSystemInstanceLogsRequest(int Tail = 100);

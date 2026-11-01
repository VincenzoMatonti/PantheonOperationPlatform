namespace Athena.Application.OrchestrationManagement.Exceptions;

public sealed class OrchestrationSystemInstanceOperationsFailedException(
    Guid orchestrationId,
    IReadOnlyDictionary<Guid, Exception> failures)
    : OrchestrationManagementApplicationException(
        OrchestrationManagementApplicationErrorCode.SystemInstanceOperationsFailed,
        $"Orchestration '{orchestrationId}' had failures for SystemInstances: {string.Join("; ", failures.Select(failure => $"{failure.Key}: {failure.Value.Message}"))}")
{
    public IReadOnlyDictionary<Guid, Exception> Failures { get; } = failures;
}

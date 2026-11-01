using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Management.Enums;
using Athena.Domain.System.Registry.ValueObjects;

namespace Athena.UnitTests;

public sealed class SystemInstanceOperationStateTests
{
    [Fact]
    public void AcceptedCommand_RemainsRunningUntilCorrelatedTerminalResult()
    {
        var operation = CreateRunningOperation();
        var acceptedAtUtc = DateTime.UtcNow;

        operation.MarkAccepted(acceptedAtUtc);

        Assert.Equal(SystemInstanceOperationStatus.Running, operation.Status);
        Assert.Equal(acceptedAtUtc, operation.AgentAcceptedAtUtc);
        Assert.Null(operation.CompletedAtUtc);

        operation.Complete(DateTime.UtcNow);

        Assert.Equal(SystemInstanceOperationStatus.Completed, operation.Status);
        Assert.NotNull(operation.AgentCompletedAtUtc);
        Assert.NotNull(operation.CompletedAtUtc);
        Assert.False(operation.IsActive);
    }

    [Fact]
    public void OutcomeUnknown_CanRecoverFromCorrelatedSuccess()
    {
        var operation = CreateRunningOperation();
        operation.MarkOutcomeUnknown("Agent response timed out.");

        operation.Complete(DateTime.UtcNow);

        Assert.Equal(SystemInstanceOperationStatus.Completed, operation.Status);
    }

    [Fact]
    public void FailedOperation_CannotBeOverwrittenByLateSuccess()
    {
        var operation = CreateRunningOperation();
        operation.Fail("Agent rejected the command.");

        Assert.ThrowsAny<Exception>(() => operation.Complete(DateTime.UtcNow));
        Assert.Equal(SystemInstanceOperationStatus.Failed, operation.Status);
    }

    [Fact]
    public void OutcomeUnknown_CanBeClosedAsIndeterminateWithoutClaimingSuccessOrFailure()
    {
        var operation = CreateRunningOperation();
        operation.MarkOutcomeUnknown("Agent correlation is unavailable.");

        operation.MarkIndeterminate("Operator confirmed the result cannot be recovered.");

        Assert.Equal(SystemInstanceOperationStatus.Indeterminate, operation.Status);
        Assert.Equal("Operator confirmed the result cannot be recovered.", operation.ErrorMessage);
        Assert.NotNull(operation.CompletedAtUtc);
        Assert.False(operation.IsActive);
    }

    [Fact]
    public void OutcomeUnknown_CannotBeResolvedWhileAWorkerLeaseIsActive()
    {
        var operation = CreateRunningOperation();
        operation.MarkOutcomeUnknown("Agent response was lost.");
        var nowUtc = DateTime.UtcNow;
        operation.Claim("worker", nowUtc.AddMinutes(1), nowUtc);

        Assert.ThrowsAny<Exception>(() =>
            operation.MarkIndeterminate("Operator confirmed the result cannot be recovered."));
        Assert.Equal(SystemInstanceOperationStatus.OutcomeUnknown, operation.Status);
    }

    [Fact]
    public void StaleObservation_DoesNotOverwriteNewerOperationalState()
    {
        var lifecycle = SystemInstanceLifecycle.Create(SystemInstanceId.New());
        var newer = DateTime.UtcNow;
        var older = newer.AddSeconds(-1);
        lifecycle.SetObservedOperationalStatus(SystemInstanceOperationalStatus.Running, newer);

        var changed = lifecycle.SetObservedOperationalStatus(SystemInstanceOperationalStatus.Stopped, older);

        Assert.False(changed);
        Assert.Equal(SystemInstanceOperationalStatus.Running, lifecycle.OperationalStatus);
        Assert.Equal(newer, lifecycle.ObservedAtUtc);
    }

    private static SystemInstanceOperation CreateRunningOperation()
    {
        var operation = SystemInstanceOperation.Create(SystemInstanceId.New(), SystemInstanceOperationType.Start);
        operation.Start();
        operation.MarkCommandDispatched(DateTime.UtcNow);
        return operation;
    }
}

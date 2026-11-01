using Athena.Application.BackgroundTasks.Synchronization;
using Athena.Application.BackgroundTasks.Operations;
using Athena.Domain.System.Registry.Entities;
using Athena.Domain.System.Registry.Enums;
using Athena.Domain.System.Registry.Exceptions;
using Athena.Domain.System.Registry.Repositories;
using Athena.Domain.System.Registry.ValueObjects;
using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Management.Enums;
using Athena.Domain.System.Management.Repositories;
using Athena.Domain.Workload.Registry.ValueObjects;
using Microsoft.Extensions.Logging.Abstractions;
using Athena.Application.SystemInstanceManagement.Contracts;
using Athena.Application.SystemInstanceManagement.Exceptions;
using Athena.Application.SystemInstanceManagement.Enums;
using Athena.Application.SystemInstanceManagement.Services;
using Athena.Application.SystemInstanceManagement.Interfaces;

namespace Athena.UnitTests;

public sealed class SystemInstanceProcessManagementTests
{
    [Fact]
    public async Task StartAsync_CommandResponseDoesNotAssertObservedStatusOrCompleteOperation()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        fixture.Agent.Status = Status(fixture.InstanceId, "Running", true);
        var service = fixture.CreateService();

        var result = await service.StartAsync(fixture.InstanceId);

        Assert.Equal(fixture.InstanceId, result.SystemInstanceId);
        Assert.Equal(fixture.InstanceId, fixture.Agent.LastCommandedInstanceId);
        Assert.Equal(SystemInstanceDesiredState.Running, fixture.Lifecycle.DesiredState);
        Assert.Equal(SystemInstanceOperationalStatus.Unknown, fixture.Lifecycle.OperationalStatus);
        var operation = Assert.Single(fixture.Operations.Items);
        Assert.Equal(SystemInstanceOperationType.Start, operation.Type);
        Assert.Equal(SystemInstanceOperationStatus.Running, operation.Status);
        Assert.NotNull(operation.StartedAtUtc);
        Assert.Null(operation.CompletedAtUtc);
    }

    [Fact]
    public async Task StartAsync_WhenAgentFails_PersistsFailedOperationAndPreservesException()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        var expected = new HttpRequestException(
            "agent rejected the command",
            null,
            System.Net.HttpStatusCode.BadRequest);
        fixture.Agent.CommandFailure = expected;
        var service = fixture.CreateService();

        var actual = await Assert.ThrowsAsync<HttpRequestException>(() => service.StartAsync(fixture.InstanceId));

        Assert.Same(expected, actual);
        Assert.Equal(SystemInstanceDesiredState.Running, fixture.Lifecycle.DesiredState);
        var operation = Assert.Single(fixture.Operations.Items);
        Assert.Equal(SystemInstanceOperationStatus.Failed, operation.Status);
        Assert.Equal("agent rejected the command", operation.ErrorMessage);
    }

    [Fact]
    public async Task StartAsync_WhenAgentResponseCannotBeCorrelated_LeavesOperationRunning()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        fixture.Agent.Status = Status(Guid.NewGuid(), "Running", true);
        var service = fixture.CreateService();

        await Assert.ThrowsAsync<ExecutionAgentResponseException>(() => service.StartAsync(fixture.InstanceId));

        Assert.Equal(SystemInstanceOperationalStatus.Unknown, fixture.Lifecycle.OperationalStatus);
        Assert.Equal(SystemInstanceOperationStatus.Running, Assert.Single(fixture.Operations.Items).Status);
    }

    [Fact]
    public async Task RegisterAsync_DoesNotReactivateDisabledInstanceOrCallAgent()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        fixture.Instance.Disable();
        var service = fixture.CreateService();

        await Assert.ThrowsAsync<SystemInstanceDisabledCannotBeModifiedException>(
            () => service.RegisterAsync(new SystemInstanceExecutionCommandRequest(fixture.InstanceId)));

        Assert.Null(fixture.Agent.LastCommandedInstanceId);
        Assert.Empty(fixture.Operations.Items);
        Assert.Equal(SystemInstanceRegistrationStatus.Disabled, fixture.Instance.RegistrationStatus);
    }

    [Fact]
    public async Task RemoveAsync_WhenAgentFails_DoesNotDeleteLocalInstance()
    {
        var fixture = new Fixture();
        fixture.Agent.CommandFailure = new HttpRequestException(
            "agent rejected the command",
            null,
            System.Net.HttpStatusCode.BadRequest);
        var service = fixture.CreateService();

        await Assert.ThrowsAsync<HttpRequestException>(() => service.RemoveAsync(fixture.InstanceId));

        Assert.False(fixture.Instance.IsDeleted);
        Assert.Equal(SystemInstanceOperationStatus.Failed, Assert.Single(fixture.Operations.Items).Status);
    }

    [Fact]
    public async Task CreateAsync_WhenAgentFails_PreservesLocalInstanceAndMarksOperationFailed()
    {
        var fixture = new Fixture();
        fixture.Agent.CommandFailure = new HttpRequestException(
            "agent rejected the command",
            null,
            System.Net.HttpStatusCode.BadRequest);
        var createdId = Guid.NewGuid();
        var service = fixture.CreateService();

        await Assert.ThrowsAsync<HttpRequestException>(() => service.CreateAsync(
            new SystemInstanceExecutionCommandRequest(createdId, Guid.NewGuid(), Guid.NewGuid(), "created")));

        var created = Assert.Single(fixture.Instances.Items, instance => instance.Id.Value == createdId);
        Assert.Equal(SystemInstanceRegistrationStatus.Unregistered, created.RegistrationStatus);
        Assert.Equal(SystemInstanceOperationalStatus.Unknown, Assert.Single(fixture.Lifecycles.Items, lifecycle => lifecycle.Id.Value == createdId).OperationalStatus);
        Assert.Equal(SystemInstanceOperationStatus.Failed, Assert.Single(fixture.Operations.Items).Status);
    }

    [Fact]
    public async Task StartAsync_WhenAgentTransportFails_LeavesOperationRunningForRecovery()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        fixture.Agent.CommandFailure = new HttpRequestException("connection failed");
        var service = fixture.CreateService();

        await Assert.ThrowsAsync<HttpRequestException>(() => service.StartAsync(fixture.InstanceId));

        var operation = Assert.Single(fixture.Operations.Items);
        Assert.Equal(SystemInstanceOperationStatus.Running, operation.Status);
        Assert.Equal(SystemInstanceOperationStatus.Running, fixture.Operations.PersistedStatuses[operation.Id]);
    }

    [Fact]
    public async Task StartAsync_WhenAgentServerErrorIsAmbiguous_LeavesOperationRunning()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        fixture.Agent.CommandFailure = new HttpRequestException(
            "agent server error",
            null,
            System.Net.HttpStatusCode.ServiceUnavailable);

        await Assert.ThrowsAsync<HttpRequestException>(() => fixture.CreateService().StartAsync(fixture.InstanceId));

        Assert.Equal(SystemInstanceOperationStatus.Running, Assert.Single(fixture.Operations.Items).Status);
    }

    [Fact]
    public async Task EnqueueOperationAsync_PersistsDesiredStateAndPendingOperationWithoutCallingAgent()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();

        var operation = await fixture.CreateService().EnqueueOperationAsync(
            fixture.InstanceId,
            SystemInstanceOperationType.Start);

        Assert.NotEqual(Guid.Empty, operation.Id);
        Assert.Equal("Pending", operation.Status);
        Assert.Equal(SystemInstanceDesiredState.Running, fixture.Lifecycle.DesiredState);
        Assert.Equal(SystemInstanceOperationStatus.Pending, Assert.Single(fixture.Operations.Items).Status);
        Assert.Empty(fixture.Agent.CommandedInstanceIds);
    }

    [Fact]
    public async Task OperationProcessor_CompletesOnlyFromCorrelatedAgentResult()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        var queued = await fixture.CreateService().EnqueueOperationAsync(fixture.InstanceId, SystemInstanceOperationType.Start);
        fixture.Agent.CorrelatedResult = new SystemInstanceExecutionResult(
            queued.Id,
            fixture.InstanceId,
            AgentOperationStatus.Completed,
            DateTime.UtcNow,
            DateTime.UtcNow,
            DateTime.UtcNow,
            ObservedState: "Running",
            ObservedAtUtc: DateTime.UtcNow);

        await fixture.CreateProcessor().ProcessOneAsync(false, TimeSpan.FromMinutes(1), TimeSpan.Zero);

        Assert.Equal(SystemInstanceOperationStatus.Completed, Assert.Single(fixture.Operations.Items).Status);
        Assert.Equal(queued.Id, fixture.Agent.LastCorrelatedCommand?.OperationId);
        Assert.Equal(SystemInstanceOperationalStatus.Running, fixture.Lifecycle.OperationalStatus);
    }

    [Fact]
    public async Task OperationProcessor_PreservesAmbiguousOutcome()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        var queued = await fixture.CreateService().EnqueueOperationAsync(fixture.InstanceId, SystemInstanceOperationType.Restart);
        fixture.Agent.CorrelatedCommandFailure = new TimeoutException("Agent response timed out.");

        await fixture.CreateProcessor().ProcessOneAsync(false, TimeSpan.FromMinutes(1), TimeSpan.Zero);

        Assert.Equal(SystemInstanceOperationStatus.OutcomeUnknown, Assert.Single(fixture.Operations.Items).Status);
        Assert.Equal(queued.Id, fixture.Agent.LastCorrelatedCommand?.OperationId);
        Assert.Equal(1, fixture.Agent.CorrelatedCommandCount);
    }

    [Fact]
    public async Task CreateOperation_IsQueuedAndRegistryEntityExistsBeforeAgentExecution()
    {
        var fixture = new Fixture();
        var id = Guid.NewGuid();

        var operation = await fixture.CreateService().CreateOperationAsync(
            new SystemInstanceExecutionCommandRequest(
                id,
                Guid.NewGuid(),
                Guid.NewGuid(),
                "created",
                ConfigurationReference: "profile/test"));

        Assert.Equal("Pending", operation.Status);
        Assert.Equal(SystemInstanceOperationType.Create, Assert.Single(fixture.Operations.Items).Type);
        Assert.True(await fixture.Instances.ExistsByIdAsync(SystemInstanceId.Create(id)));
        Assert.Empty(fixture.Agent.CommandedInstanceIds);
    }

    [Fact]
    public async Task CreateOperation_RejectsInlineDetailsThatCouldContainSecrets()
    {
        var fixture = new Fixture();

        await Assert.ThrowsAsync<ArgumentException>(() => fixture.CreateService().CreateOperationAsync(
            new SystemInstanceExecutionCommandRequest(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                "created",
                Details: "{\"password\":\"plaintext\"}")));

        Assert.Empty(fixture.Operations.Items);
        Assert.DoesNotContain(fixture.Instances.Items, instance => instance.Id.Value != fixture.InstanceId);
    }

    [Fact]
    public async Task OperationProcessor_RegistersLocallyOnlyAfterCorrelatedAgentCompletion()
    {
        var fixture = new Fixture();
        var queued = await fixture.CreateService().RegisterOperationAsync(
            new SystemInstanceExecutionCommandRequest(fixture.InstanceId, ConfigurationReference: "profile/test"));
        Assert.Equal(SystemInstanceRegistrationStatus.Unregistered, fixture.Instance.RegistrationStatus);
        fixture.Agent.CorrelatedResult = new SystemInstanceExecutionResult(
            queued.Id,
            fixture.InstanceId,
            AgentOperationStatus.Completed,
            DateTime.UtcNow,
            DateTime.UtcNow,
            DateTime.UtcNow);

        await fixture.CreateProcessor().ProcessOneAsync(false, TimeSpan.FromMinutes(1), TimeSpan.Zero);

        Assert.Equal(SystemInstanceOperationStatus.Completed, Assert.Single(fixture.Operations.Items).Status);
        Assert.Equal(SystemInstanceRegistrationStatus.Registered, fixture.Instance.RegistrationStatus);
    }

    [Fact]
    public async Task OperationProcessor_RemovesRegistryInstanceOnlyAfterCorrelatedAgentCompletion()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        var queued = await fixture.CreateService().RemoveOperationAsync(fixture.InstanceId);
        fixture.Agent.CorrelatedResult = new SystemInstanceExecutionResult(
            queued.Id,
            fixture.InstanceId,
            AgentOperationStatus.Completed,
            DateTime.UtcNow,
            DateTime.UtcNow,
            DateTime.UtcNow);

        await fixture.CreateProcessor().ProcessOneAsync(false, TimeSpan.FromMinutes(1), TimeSpan.Zero);

        Assert.Equal(SystemInstanceOperationStatus.Completed, Assert.Single(fixture.Operations.Items).Status);
        Assert.True(fixture.Instance.IsDeleted);
    }

    [Fact]
    public async Task OperationProcessor_ExhaustsRetriesAsIndeterminateWithoutClaimingFailure()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        var operation = SystemInstanceOperation.Create(
            fixture.Instance.Id,
            SystemInstanceOperationType.Start,
            maxAttempts: 1);
        fixture.Operations.Items.Add(operation);

        await fixture.CreateProcessor().ProcessOneAsync(false, TimeSpan.FromMinutes(1), TimeSpan.Zero);
        await fixture.CreateProcessor().ProcessOneAsync(false, TimeSpan.FromMinutes(1), TimeSpan.Zero);

        Assert.Equal(SystemInstanceOperationStatus.Indeterminate, operation.Status);
        Assert.Contains("maximum of 1 attempts", operation.ErrorMessage);
        Assert.Equal(1, fixture.Agent.CorrelatedCommandCount);
    }

    [Fact]
    public async Task StartAsync_WhenLifecycleIsMissing_FailsBeforeCallingAgent()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        fixture.Lifecycles.Items.Clear();
        var service = fixture.CreateService();

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.StartAsync(fixture.InstanceId));

        Assert.Null(fixture.Agent.LastCommandedInstanceId);
        Assert.Empty(fixture.Operations.Items);
    }

    [Fact]
    public async Task SynchronizeAsync_ContinuesAcrossInstancesAndMarksAgentFailuresUnreachable()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        var otherInstance = SystemInstance.Create(
            SystemInstanceId.New(),
            SystemId.Create(Guid.NewGuid()),
            WorkloadId.Create(Guid.NewGuid()),
            SystemInstanceName.Create("second"));
        otherInstance.Register();
        fixture.Instances.Items.Add(otherInstance);
        fixture.Lifecycles.Items.Add(SystemInstanceLifecycle.Create(otherInstance.Id));
        fixture.Agent.CommandFailure = new HttpRequestException("agent unavailable");
        var synchronization = new SystemSynchronizationService(
            fixture.Instances,
            fixture.CreateService(),
            NullLogger<SystemSynchronizationService>.Instance);

        await synchronization.SynchronizeAsync();

        Assert.Equal(2, fixture.Agent.CommandedInstanceIds.Count);
        Assert.Equal(SystemInstanceOperationalStatus.Unreachable, fixture.Lifecycles.Items.Single(lifecycle => lifecycle.Id == fixture.Instance.Id).OperationalStatus);
        Assert.Equal(SystemInstanceOperationalStatus.Unreachable, fixture.Lifecycles.Items.Single(lifecycle => lifecycle.Id == otherInstance.Id).OperationalStatus);
    }

    [Fact]
    public async Task SynchronizeAsync_AppliesObservedStatusWithoutChangingDesiredState()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        fixture.Lifecycle.SetDesiredState(SystemInstanceDesiredState.Running);
        fixture.Agent.Status = Status(fixture.InstanceId, "Stopped", false);
        var synchronization = CreateSynchronization(fixture);

        await synchronization.SynchronizeAsync();

        Assert.Equal(SystemInstanceDesiredState.Running, fixture.Lifecycle.DesiredState);
        Assert.Equal(SystemInstanceOperationalStatus.Stopped, fixture.Lifecycle.OperationalStatus);
    }

    [Fact]
    public async Task SynchronizeAsync_UnregisteredAndDisabledInstancesAreNotQueried()
    {
        var fixture = new Fixture();
        var disabled = SystemInstance.Create(
            SystemInstanceId.New(),
            SystemId.Create(Guid.NewGuid()),
            WorkloadId.Create(Guid.NewGuid()),
            SystemInstanceName.Create("disabled"));
        disabled.Register();
        disabled.Disable();
        fixture.Instances.Items.Add(disabled);
        fixture.Lifecycles.Items.Add(SystemInstanceLifecycle.Create(disabled.Id));

        await CreateSynchronization(fixture).SynchronizeAsync();

        Assert.Empty(fixture.Agent.CommandedInstanceIds);
    }

    [Fact]
    public async Task SynchronizeAsync_MissingLifecycleDoesNotPreventLaterInstances()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        fixture.Lifecycles.Items.Clear();
        var otherInstance = CreateRegisteredInstance("second");
        fixture.Instances.Items.Add(otherInstance);
        fixture.Lifecycles.Items.Add(SystemInstanceLifecycle.Create(otherInstance.Id));

        await CreateSynchronization(fixture).SynchronizeAsync();

        Assert.Equal([otherInstance.Id.Value], fixture.Agent.CommandedInstanceIds);
        Assert.Equal(SystemInstanceOperationalStatus.Stopped,
            fixture.Lifecycles.Items.Single().OperationalStatus);
    }

    [Fact]
    public async Task SynchronizeAsync_AgentTimeoutMarksUnreachableWithoutChangingDesiredStateToStopped()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        fixture.Lifecycle.SetDesiredState(SystemInstanceDesiredState.Running);
        fixture.Agent.CommandFailure = new TaskCanceledException("agent timeout");

        await CreateSynchronization(fixture).SynchronizeAsync();

        Assert.Equal(SystemInstanceDesiredState.Running, fixture.Lifecycle.DesiredState);
        Assert.Equal(SystemInstanceOperationalStatus.Unreachable, fixture.Lifecycle.OperationalStatus);
    }

    [Fact]
    public async Task SynchronizeAsync_InvalidAgentStateBecomesUnknownAndContinues()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        fixture.Lifecycle.SetDesiredState(SystemInstanceDesiredState.Running);
        fixture.Lifecycle.SetOperationalStatus(SystemInstanceOperationalStatus.Running);
        fixture.Agent.Status = Status(fixture.InstanceId, "not-a-state", false);
        var otherInstance = CreateRegisteredInstance("second");
        fixture.Instances.Items.Add(otherInstance);
        fixture.Lifecycles.Items.Add(SystemInstanceLifecycle.Create(otherInstance.Id));
        fixture.Agent.Statuses[otherInstance.Id.Value] = Status(otherInstance.Id.Value, "Running", true);

        await CreateSynchronization(fixture).SynchronizeAsync();

        Assert.Equal(SystemInstanceOperationalStatus.Unknown, fixture.Lifecycle.OperationalStatus);
        Assert.Equal(SystemInstanceDesiredState.Running, fixture.Lifecycle.DesiredState);
        Assert.Equal(SystemInstanceOperationalStatus.Running,
            fixture.Lifecycles.Items.Single(lifecycle => lifecycle.Id == otherInstance.Id).OperationalStatus);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task SynchronizeAsync_ObservesInstanceEvenWithActiveOperation(bool running)
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        fixture.Lifecycle.SetDesiredState(SystemInstanceDesiredState.Running);
        fixture.Lifecycle.SetOperationalStatus(SystemInstanceOperationalStatus.Starting);
        var operation = SystemInstanceOperation.Create(fixture.Instance.Id, SystemInstanceOperationType.Start);
        if (running)
            operation.Start();
        fixture.Operations.Items.Add(operation);

        await CreateSynchronization(fixture).SynchronizeAsync();

        Assert.Equal([fixture.InstanceId], fixture.Agent.CommandedInstanceIds);
        Assert.Equal(running ? SystemInstanceOperationStatus.Running : SystemInstanceOperationStatus.Pending, operation.Status);
        Assert.Equal(SystemInstanceOperationalStatus.Stopped, fixture.Lifecycle.OperationalStatus);
        Assert.Equal(SystemInstanceDesiredState.Running, fixture.Lifecycle.DesiredState);
    }

    [Fact]
    public async Task SynchronizeAsync_CancellationStopsCycleBeforePollingInstances()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => CreateSynchronization(fixture).SynchronizeAsync(cancellation.Token));

        Assert.Empty(fixture.Agent.CommandedInstanceIds);
    }

    [Fact]
    public async Task SynchronizeAsync_CancellationDuringAgentRequestStopsCycle()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        using var cancellation = new CancellationTokenSource();
        fixture.Agent.OnCommand = cancellation.Cancel;

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => CreateSynchronization(fixture).SynchronizeAsync(cancellation.Token));

        Assert.Equal(SystemInstanceOperationalStatus.Unknown, fixture.Lifecycle.OperationalStatus);
    }

    [Fact]
    public async Task SynchronizeAsync_AgentFailureOnOneInstanceDoesNotPreventNextObservation()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        var otherInstance = CreateRegisteredInstance("second");
        fixture.Instances.Items.Add(otherInstance);
        fixture.Lifecycles.Items.Add(SystemInstanceLifecycle.Create(otherInstance.Id));
        fixture.Agent.Failures[fixture.InstanceId] = new HttpRequestException("connection failed");
        fixture.Agent.Statuses[otherInstance.Id.Value] = Status(otherInstance.Id.Value, "Running", true);

        await CreateSynchronization(fixture).SynchronizeAsync();

        Assert.Equal(SystemInstanceOperationalStatus.Unreachable, fixture.Lifecycle.OperationalStatus);
        Assert.Equal(SystemInstanceOperationalStatus.Running,
            fixture.Lifecycles.Items.Single(lifecycle => lifecycle.Id == otherInstance.Id).OperationalStatus);
    }

    [Fact]
    public async Task GetLogsAsync_RejectsInvalidTailBeforeCallingAgent()
    {
        var fixture = new Fixture();
        var service = fixture.CreateService();

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetLogsAsync(fixture.InstanceId, 0));

        Assert.Null(fixture.Agent.LastCommandedInstanceId);
    }

    private static SystemInstanceExecutionStatus Status(Guid id, string state, bool running) =>
        new(id, state, running, true, DateTime.UtcNow);

    private static SystemInstance CreateRegisteredInstance(string name)
    {
        var instance = SystemInstance.Create(
            SystemInstanceId.New(),
            SystemId.Create(Guid.NewGuid()),
            WorkloadId.Create(Guid.NewGuid()),
            SystemInstanceName.Create(name));
        instance.Register();
        return instance;
    }

    private static SystemSynchronizationService CreateSynchronization(Fixture fixture) =>
        new(fixture.Instances, fixture.CreateService(), NullLogger<SystemSynchronizationService>.Instance);

    private sealed class Fixture
    {
        public Guid InstanceId { get; } = Guid.NewGuid();
        public SystemInstance Instance { get; }
        public InMemorySystemInstanceLifecycleRepository Lifecycles { get; } = new();
        public InMemorySystemInstanceRepository Instances { get; }
        public InMemorySystemInstanceOperationRepository Operations { get; }
        public FakeExecutionAgent Agent { get; } = new();
        public SystemInstanceLifecycle Lifecycle => Lifecycles.Items.Single(item => item.Id.Value == InstanceId);

        public Fixture()
        {
            Instances = new InMemorySystemInstanceRepository(Lifecycles);
            Operations = new InMemorySystemInstanceOperationRepository(Lifecycles, Instances);
            Instance = SystemInstance.Create(
                SystemInstanceId.Create(InstanceId),
                SystemId.Create(Guid.NewGuid()),
                WorkloadId.Create(Guid.NewGuid()),
                SystemInstanceName.Create("unit-test"));
            Instances.Items.Add(Instance);
            Lifecycles.Items.Add(SystemInstanceLifecycle.Create(Instance.Id));
        }

        public SystemInstanceProcessManagementService CreateService() =>
            new(Agent, Instances, Lifecycles, Operations);

        public SystemInstanceOperationProcessor CreateProcessor() =>
            new(Operations, Instances, Lifecycles, Agent, Agent, NullLogger<SystemInstanceOperationProcessor>.Instance);
    }

    private sealed class FakeExecutionAgent : ISystemInstanceExecutionAgent, ICorrelatedSystemInstanceExecutionAgent
    {
        public Guid? LastCommandedInstanceId { get; private set; }
        public List<Guid> CommandedInstanceIds { get; } = [];
        public SystemInstanceExecutionStatus? Status { get; set; }
        public Exception? CommandFailure { get; set; }
        public Dictionary<Guid, Exception> Failures { get; } = [];
        public Dictionary<Guid, SystemInstanceExecutionStatus> Statuses { get; } = [];
        public Action? OnCommand { get; set; }
        public SystemInstanceExecutionCommand? LastCorrelatedCommand { get; private set; }
        public int CorrelatedCommandCount { get; private set; }
        public Exception? CorrelatedCommandFailure { get; set; }
        public SystemInstanceExecutionResult? CorrelatedResult { get; set; }

        public Task<SystemInstanceExecutionResult> ExecuteAsync(SystemInstanceExecutionCommand command, CancellationToken cancellationToken = default)
        {
            LastCorrelatedCommand = command;
            CorrelatedCommandCount++;
            if (CorrelatedCommandFailure is not null)
                return Task.FromException<SystemInstanceExecutionResult>(CorrelatedCommandFailure);
            return Task.FromResult(CorrelatedResult ?? new SystemInstanceExecutionResult(
                command.OperationId,
                command.SystemInstanceId,
                AgentOperationStatus.Accepted,
                DateTime.UtcNow,
                DateTime.UtcNow));
        }

        public Task<SystemInstanceExecutionResult> GetOperationAsync(
            Guid systemInstanceId,
            Guid operationId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(CorrelatedResult ?? new SystemInstanceExecutionResult(
                operationId,
                systemInstanceId,
                AgentOperationStatus.Running,
                DateTime.UtcNow,
                DateTime.UtcNow));

        public Task<SystemInstanceExecutionStatus> CreateAsync(SystemInstanceExecutionCommandRequest request, CancellationToken cancellationToken = default) =>
            CommandAsync(request.SystemInstanceId, cancellationToken);

        public Task<SystemInstanceExecutionStatus> RegisterAsync(SystemInstanceExecutionCommandRequest request, CancellationToken cancellationToken = default) =>
            CommandAsync(request.SystemInstanceId, cancellationToken);

        public Task<SystemInstanceExecutionStatus> StartAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            CommandAsync(systemInstanceId, cancellationToken);

        public Task<SystemInstanceExecutionStatus> StopAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            CommandAsync(systemInstanceId, cancellationToken);

        public Task<SystemInstanceExecutionStatus> RestartAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            CommandAsync(systemInstanceId, cancellationToken);

        public Task<SystemInstanceExecutionStatus> RemoveAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            CommandAsync(systemInstanceId, cancellationToken);

        public Task<SystemInstanceExecutionStatus> GetStatusAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            CommandAsync(systemInstanceId, cancellationToken);

        public Task<SystemInstanceExecutionHealth> GetHealthAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            throw new NotImplementedException();

        public Task<IReadOnlyList<SystemInstanceExecutionLogEntry>> GetLogsAsync(Guid systemInstanceId, int tail = 100, CancellationToken cancellationToken = default) =>
            throw new NotImplementedException();

        private Task<SystemInstanceExecutionStatus> CommandAsync(Guid id, CancellationToken cancellationToken = default)
        {
            LastCommandedInstanceId = id;
            CommandedInstanceIds.Add(id);
            OnCommand?.Invoke();
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled<SystemInstanceExecutionStatus>(cancellationToken);
            if (Failures.TryGetValue(id, out var failure))
                return Task.FromException<SystemInstanceExecutionStatus>(failure);
            if (CommandFailure is not null) return Task.FromException<SystemInstanceExecutionStatus>(CommandFailure);
            return Task.FromResult(Statuses.GetValueOrDefault(id) ?? Status ?? SystemInstanceProcessManagementTests.Status(id, "Stopped", false));
        }
    }

    private sealed class InMemorySystemInstanceRepository : ISystemInstanceRepository
    {
        private readonly InMemorySystemInstanceLifecycleRepository _lifecycles;

        public InMemorySystemInstanceRepository(InMemorySystemInstanceLifecycleRepository lifecycles) =>
            _lifecycles = lifecycles;

        public List<SystemInstance> Items { get; } = [];

        public Task<SystemInstance?> GetByIdAsync(SystemInstanceId id, CancellationToken cancellationToken = default) =>
            Task.FromResult(Items.SingleOrDefault(item => item.Id == id));

        public Task<SystemInstance?> GetByNameAsync(WorkloadId workloadId, SystemInstanceName name, CancellationToken cancellationToken = default) =>
            Task.FromResult(Items.SingleOrDefault(item => item.WorkloadId == workloadId && item.Name == name));

        public Task<IReadOnlyList<SystemInstance>> GetAllAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<SystemInstance>>(Items.ToArray());

        public Task<IReadOnlyList<SystemInstance>> GetBySystemIdAsync(SystemId systemId, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<SystemInstance>>(Items.Where(item => item.SystemId == systemId).ToArray());

        public Task<IReadOnlyList<SystemInstance>> GetByWorkloadIdAsync(WorkloadId workloadId, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<SystemInstance>>(Items.Where(item => item.WorkloadId == workloadId).ToArray());

        public Task<IReadOnlyList<SystemInstance>> GetByRegistrationStatusAsync(SystemInstanceRegistrationStatus status, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<SystemInstance>>(Items.Where(item => !item.IsDeleted && item.RegistrationStatus == status).ToArray());

        public Task<IReadOnlyList<SystemInstance>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<SystemInstance>>(Items.Where(item => item.IsDeleted == isDeleted).ToArray());

        public Task<bool> ExistsByIdAsync(SystemInstanceId id, CancellationToken cancellationToken = default) =>
            Task.FromResult(Items.Any(item => item.Id == id));

        public Task<bool> ExistsByNameAsync(WorkloadId workloadId, SystemInstanceName name, CancellationToken cancellationToken = default) =>
            Task.FromResult(Items.Any(item => item.WorkloadId == workloadId && item.Name == name));

        public Task AddAsync(SystemInstance systemInstance, SystemInstanceLifecycle lifecycle, CancellationToken cancellationToken = default)
        {
            Items.Add(systemInstance);
            _lifecycles.Items.Add(lifecycle);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(SystemInstance systemInstance, CancellationToken cancellationToken = default) => Task.CompletedTask;
    }

    private sealed class InMemorySystemInstanceLifecycleRepository : ISystemInstanceLifecycleRepository
    {
        public List<SystemInstanceLifecycle> Items { get; } = [];
        public bool FailUpdates { get; set; }
        public Task<SystemInstanceLifecycle?> GetByIdAsync(SystemInstanceId id, CancellationToken cancellationToken = default) =>
            Task.FromResult(Items.SingleOrDefault(item => item.Id == id));
        public Task<IReadOnlyList<SystemInstanceLifecycle>> GetAllAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<SystemInstanceLifecycle>>(Items.ToArray());
        public Task<IReadOnlyList<SystemInstanceLifecycle>> GetByDesiredStateAsync(SystemInstanceDesiredState state, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<SystemInstanceLifecycle>>(Items.Where(item => item.DesiredState == state).ToArray());
        public Task<IReadOnlyList<SystemInstanceLifecycle>> GetByOperationalStatusAsync(SystemInstanceOperationalStatus status, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<SystemInstanceLifecycle>>(Items.Where(item => item.OperationalStatus == status).ToArray());
        public Task AddAsync(SystemInstanceLifecycle lifecycle, CancellationToken cancellationToken = default)
        {
            Items.Add(lifecycle);
            return Task.CompletedTask;
        }
        public Task UpdateAsync(SystemInstanceLifecycle lifecycle, CancellationToken cancellationToken = default)
        {
            if (FailUpdates) throw new IOException("local persistence failed");
            return Task.CompletedTask;
        }
    }

    private sealed class InMemorySystemInstanceOperationRepository : ISystemInstanceOperationRepository
    {
        private readonly InMemorySystemInstanceLifecycleRepository _lifecycles;
        private readonly InMemorySystemInstanceRepository _instances;

        public InMemorySystemInstanceOperationRepository(
            InMemorySystemInstanceLifecycleRepository lifecycles,
            InMemorySystemInstanceRepository instances)
        {
            _lifecycles = lifecycles;
            _instances = instances;
        }

        public List<SystemInstanceOperation> Items { get; } = [];
        public Dictionary<Guid, SystemInstanceOperationStatus> PersistedStatuses { get; } = [];
        private readonly Dictionary<Guid, string> _leases = [];

        public Task AddAsync(SystemInstanceOperation operation, CancellationToken cancellationToken = default)
        {
            Items.Add(operation);
            PersistedStatuses[operation.Id] = operation.Status;
            return Task.CompletedTask;
        }

        public Task AddWithLifecycleAsync(
            SystemInstanceOperation operation,
            SystemInstanceLifecycle lifecycle,
            CancellationToken cancellationToken = default)
        {
            Items.Add(operation);
            PersistedStatuses[operation.Id] = operation.Status;
            return Task.CompletedTask;
        }

        public Task AddWithRegistryAsync(
            SystemInstanceOperation operation,
            SystemInstance systemInstance,
            SystemInstanceLifecycle lifecycle,
            CancellationToken cancellationToken = default)
        {
            Items.Add(operation);
            PersistedStatuses[operation.Id] = operation.Status;
            _instances.Items.Add(systemInstance);
            _lifecycles.Items.Add(lifecycle);
            return Task.CompletedTask;
        }

        public Task SaveWithLifecycleAsync(
            SystemInstanceOperation operation,
            SystemInstanceLifecycle lifecycle,
            SystemInstance? systemInstance = null,
            CancellationToken cancellationToken = default)
        {
            if (_lifecycles.FailUpdates) throw new IOException("local persistence failed");
            PersistedStatuses[operation.Id] = operation.Status;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(SystemInstanceOperation operation, CancellationToken cancellationToken = default)
        {
            PersistedStatuses[operation.Id] = operation.Status;
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<SystemInstanceOperation>> GetBySystemInstanceIdAsync(
            SystemInstanceId systemInstanceId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<SystemInstanceOperation>>(
                Items.Where(item => item.SystemInstanceId == systemInstanceId).ToArray());

        public Task<bool> HasActiveOperationAsync(SystemInstanceId systemInstanceId, CancellationToken cancellationToken = default) =>
            Task.FromResult(Items.Any(item => item.SystemInstanceId == systemInstanceId
                && item.IsActive));

        public Task<SystemInstanceOperation?> ClaimNextAsync(string leaseOwner, TimeSpan leaseDuration, DateTime nowUtc, CancellationToken cancellationToken = default)
        {
            var operation = Items.FirstOrDefault(item => item.IsActive
                && Enum.IsDefined(item.Type)
                && (item.NextAttemptAtUtc is null || item.NextAttemptAtUtc <= nowUtc)
                && (item.LeaseExpiresAtUtc is null || item.LeaseExpiresAtUtc <= nowUtc));
            if (operation is null) return Task.FromResult<SystemInstanceOperation?>(null);
            operation.Claim(leaseOwner, nowUtc + leaseDuration, nowUtc);
            _leases[operation.Id] = leaseOwner;
            return Task.FromResult<SystemInstanceOperation?>(operation);
        }

        public Task<bool> RenewLeaseAsync(Guid operationId, string leaseOwner, TimeSpan leaseDuration, DateTime nowUtc, CancellationToken cancellationToken = default) =>
            Task.FromResult(_leases.TryGetValue(operationId, out var owner) && owner == leaseOwner);

        public Task<bool> SaveIfLeaseOwnerAsync(SystemInstanceOperation operation, string leaseOwner, CancellationToken cancellationToken = default)
        {
            if (!_leases.TryGetValue(operation.Id, out var currentOwner) || currentOwner != leaseOwner)
                return Task.FromResult(false);
            PersistedStatuses[operation.Id] = operation.Status;
            if (operation.LeaseOwner is null) _leases.Remove(operation.Id);
            return Task.FromResult(true);
        }

        public Task<bool> SaveWithLifecycleIfLeaseOwnerAsync(
            SystemInstanceOperation operation,
            SystemInstanceLifecycle lifecycle,
            SystemInstance? systemInstance,
            string leaseOwner,
            CancellationToken cancellationToken = default)
        {
            if (!_leases.TryGetValue(operation.Id, out var currentOwner) || currentOwner != leaseOwner)
                return Task.FromResult(false);
            if (_lifecycles.FailUpdates) throw new IOException("local persistence failed");
            PersistedStatuses[operation.Id] = operation.Status;
            if (operation.LeaseOwner is null) _leases.Remove(operation.Id);
            return Task.FromResult(true);
        }

        public Task<SystemInstanceOperation?> GetByIdAsync(Guid operationId, CancellationToken cancellationToken = default) =>
            Task.FromResult(Items.SingleOrDefault(operation => operation.Id == operationId));
    }
}

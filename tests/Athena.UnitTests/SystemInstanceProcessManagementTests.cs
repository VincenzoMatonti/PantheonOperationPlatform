using Athena.Application.SystemManagement.Contracts;
using Athena.Application.SystemManagement.Services;
using Athena.Application.BackgroundTasks.Synchronization;
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

namespace Athena.UnitTests;

public sealed class SystemInstanceProcessManagementTests
{
    [Fact]
    public async Task StartAsync_UpdatesSingleInstanceAndCompletesOperation()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        fixture.Agent.Status = Status(fixture.InstanceId, "Running", true);
        var service = fixture.CreateService();

        var result = await service.StartAsync(fixture.InstanceId);

        Assert.Equal(fixture.InstanceId, result.SystemInstanceId);
        Assert.Equal(fixture.InstanceId, fixture.Agent.LastCommandedInstanceId);
        Assert.Equal(SystemInstanceDesiredState.Running, fixture.Lifecycle.DesiredState);
        Assert.Equal(SystemInstanceOperationalStatus.Running, fixture.Lifecycle.OperationalStatus);
        var operation = Assert.Single(fixture.Operations.Items);
        Assert.Equal(SystemInstanceOperationType.Start, operation.Type);
        Assert.Equal(SystemInstanceOperationStatus.Completed, operation.Status);
        Assert.NotNull(operation.StartedAtUtc);
        Assert.NotNull(operation.CompletedAtUtc);
    }

    [Fact]
    public async Task StartAsync_WhenAgentFails_PersistsFailedOperationAndPreservesException()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        var expected = new HttpRequestException("agent unavailable");
        fixture.Agent.CommandFailure = expected;
        var service = fixture.CreateService();

        var actual = await Assert.ThrowsAsync<HttpRequestException>(() => service.StartAsync(fixture.InstanceId));

        Assert.Same(expected, actual);
        Assert.Equal(SystemInstanceDesiredState.Running, fixture.Lifecycle.DesiredState);
        var operation = Assert.Single(fixture.Operations.Items);
        Assert.Equal(SystemInstanceOperationStatus.Failed, operation.Status);
        Assert.Equal("agent unavailable", operation.ErrorMessage);
    }

    [Fact]
    public async Task StartAsync_WhenAgentRespondsForAnotherInstance_FailsOperationWithoutApplyingStatus()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        fixture.Agent.Status = Status(Guid.NewGuid(), "Running", true);
        var service = fixture.CreateService();

        await Assert.ThrowsAsync<ExecutionAgentResponseException>(() => service.StartAsync(fixture.InstanceId));

        Assert.Equal(SystemInstanceOperationalStatus.Unknown, fixture.Lifecycle.OperationalStatus);
        Assert.Equal(SystemInstanceOperationStatus.Failed, Assert.Single(fixture.Operations.Items).Status);
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
        fixture.Agent.CommandFailure = new HttpRequestException("agent unavailable");
        var service = fixture.CreateService();

        await Assert.ThrowsAsync<HttpRequestException>(() => service.RemoveAsync(fixture.InstanceId));

        Assert.False(fixture.Instance.IsDeleted);
        Assert.Equal(SystemInstanceOperationStatus.Failed, Assert.Single(fixture.Operations.Items).Status);
    }

    [Fact]
    public async Task CreateAsync_WhenAgentFails_PreservesLocalInstanceAndMarksOperationFailed()
    {
        var fixture = new Fixture();
        fixture.Agent.CommandFailure = new HttpRequestException("agent unavailable");
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
    public async Task StartAsync_WhenLocalStateUpdateFails_RecordsRemoteOperationAsCompleted()
    {
        var fixture = new Fixture();
        fixture.Instance.Register();
        fixture.Agent.Status = Status(fixture.InstanceId, "Running", true);
        fixture.Agent.OnCommand = () => fixture.Lifecycles.FailUpdates = true;
        var service = fixture.CreateService();

        await Assert.ThrowsAsync<IOException>(() => service.StartAsync(fixture.InstanceId));

        Assert.Equal(SystemInstanceOperationStatus.Completed, Assert.Single(fixture.Operations.Items).Status);
    }

    [Fact]
    public async Task SynchronizeAsync_ContinuesAcrossInstancesAndMarksAgentFailuresUnreachable()
    {
        var fixture = new Fixture();
        var otherInstance = SystemInstance.Create(
            SystemInstanceId.New(),
            SystemId.Create(Guid.NewGuid()),
            WorkloadId.Create(Guid.NewGuid()),
            SystemInstanceName.Create("second"));
        fixture.Instances.Items.Add(otherInstance);
        fixture.Lifecycles.Items.Add(SystemInstanceLifecycle.Create(otherInstance.Id));
        fixture.Agent.CommandFailure = new HttpRequestException("agent unavailable");
        var synchronization = new SystemSynchronizationService(
            fixture.CreateService(),
            NullLogger<SystemSynchronizationService>.Instance);

        await synchronization.SynchronizeAsync();

        Assert.Equal(2, fixture.Agent.CommandedInstanceIds.Count);
        Assert.Equal(SystemInstanceOperationalStatus.Unreachable, fixture.Lifecycles.Items.Single(lifecycle => lifecycle.Id == fixture.Instance.Id).OperationalStatus);
        Assert.Equal(SystemInstanceOperationalStatus.Unreachable, fixture.Lifecycles.Items.Single(lifecycle => lifecycle.Id == otherInstance.Id).OperationalStatus);
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

    private sealed class Fixture
    {
        public Guid InstanceId { get; } = Guid.NewGuid();
        public SystemInstance Instance { get; }
        public InMemorySystemInstanceRepository Instances { get; } = new();
        public InMemorySystemInstanceLifecycleRepository Lifecycles { get; } = new();
        public InMemorySystemInstanceOperationRepository Operations { get; } = new();
        public FakeExecutionAgent Agent { get; } = new();
        public SystemInstanceLifecycle Lifecycle => Lifecycles.Items.Single(item => item.Id.Value == InstanceId);

        public Fixture()
        {
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
    }

    private sealed class FakeExecutionAgent : ISystemInstanceExecutionAgent
    {
        public Guid? LastCommandedInstanceId { get; private set; }
        public List<Guid> CommandedInstanceIds { get; } = [];
        public SystemInstanceExecutionStatus? Status { get; set; }
        public Exception? CommandFailure { get; set; }
        public Action? OnCommand { get; set; }

        public Task<SystemInstanceExecutionStatus> CreateAsync(SystemInstanceExecutionCommandRequest request, CancellationToken cancellationToken = default) =>
            CommandAsync(request.SystemInstanceId);

        public Task<SystemInstanceExecutionStatus> RegisterAsync(SystemInstanceExecutionCommandRequest request, CancellationToken cancellationToken = default) =>
            CommandAsync(request.SystemInstanceId);

        public Task<SystemInstanceExecutionStatus> StartAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            CommandAsync(systemInstanceId);

        public Task<SystemInstanceExecutionStatus> StopAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            CommandAsync(systemInstanceId);

        public Task<SystemInstanceExecutionStatus> RestartAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            CommandAsync(systemInstanceId);

        public Task<SystemInstanceExecutionStatus> RemoveAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            CommandAsync(systemInstanceId);

        public Task<SystemInstanceExecutionStatus> GetStatusAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            CommandAsync(systemInstanceId);

        public Task<SystemInstanceExecutionHealth> GetHealthAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            throw new NotImplementedException();

        public Task<IReadOnlyList<SystemInstanceExecutionLogEntry>> GetLogsAsync(Guid systemInstanceId, int tail = 100, CancellationToken cancellationToken = default) =>
            throw new NotImplementedException();

        private Task<SystemInstanceExecutionStatus> CommandAsync(Guid id)
        {
            LastCommandedInstanceId = id;
            CommandedInstanceIds.Add(id);
            OnCommand?.Invoke();
            if (CommandFailure is not null) return Task.FromException<SystemInstanceExecutionStatus>(CommandFailure);
            return Task.FromResult(Status ?? SystemInstanceProcessManagementTests.Status(id, "Stopped", false));
        }
    }

    private sealed class InMemorySystemInstanceRepository : ISystemInstanceRepository
    {
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
            Task.FromResult<IReadOnlyList<SystemInstance>>(Items.Where(item => item.RegistrationStatus == status).ToArray());

        public Task<IReadOnlyList<SystemInstance>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<SystemInstance>>(Items.Where(item => item.IsDeleted == isDeleted).ToArray());

        public Task<bool> ExistsByIdAsync(SystemInstanceId id, CancellationToken cancellationToken = default) =>
            Task.FromResult(Items.Any(item => item.Id == id));

        public Task<bool> ExistsByNameAsync(WorkloadId workloadId, SystemInstanceName name, CancellationToken cancellationToken = default) =>
            Task.FromResult(Items.Any(item => item.WorkloadId == workloadId && item.Name == name));

        public Task AddAsync(SystemInstance systemInstance, CancellationToken cancellationToken = default)
        {
            Items.Add(systemInstance);
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
        public List<SystemInstanceOperation> Items { get; } = [];

        public Task AddAsync(SystemInstanceOperation operation, CancellationToken cancellationToken = default)
        {
            Items.Add(operation);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(SystemInstanceOperation operation, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task<IReadOnlyList<SystemInstanceOperation>> GetBySystemInstanceIdAsync(
            SystemInstanceId systemInstanceId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<SystemInstanceOperation>>(
                Items.Where(item => item.SystemInstanceId == systemInstanceId).ToArray());
    }
}

using Athena.Application.Common.Interfaces;
using Athena.Application.Common.Services;
using Athena.Application.WorkloadManagement.Commands;
using Athena.Application.WorkloadManagement.Queries;
using Athena.Application.WorkloadManagement.UseCases;
using Athena.Domain.Block.Registry.ValueObjects;
using Athena.Domain.Workload.Management.Entities;
using Athena.Domain.Workload.Management.Enums;
using Athena.Domain.Workload.Management.Repositories;
using Athena.Domain.Workload.Registry.Entities;
using Athena.Domain.Workload.Registry.Enums;
using Athena.Domain.Workload.Registry.Exceptions;
using Athena.Domain.Workload.Registry.Repositories;
using Athena.Domain.Workload.Registry.ValueObjects;

namespace Athena.UnitTests;

public sealed class WorkloadManagementIntentTests
{
    [Fact]
    public async Task SetDesiredStateAsync_ResetsUnverifiedOperationalStatus()
    {
        var fixture = new Fixture();
        fixture.Lifecycle.SetOperationalStatus(WorkloadOperationalStatus.Running);

        var result = await fixture.Handler.SetDesiredStateAsync(fixture.Id, "Stopped");

        Assert.Equal("Stopped", result.DesiredState);
        Assert.Equal("Unknown", result.OperationalStatus);
        Assert.Equal(1, fixture.Lifecycles.UpdateCount);
    }

    [Fact]
    public async Task ExecuteOperationAsync_PersistsCompletedIntentAndLifecycleTogether()
    {
        var fixture = new Fixture();
        fixture.Lifecycle.SetOperationalStatus(WorkloadOperationalStatus.Running);

        var result = await fixture.Handler.ExecuteOperationAsync(fixture.Id, "Stop");

        Assert.Equal("Completed", result.Status);
        Assert.Equal(WorkloadDesiredState.Stopped, fixture.Lifecycle.DesiredState);
        Assert.Equal(WorkloadOperationalStatus.Unknown, fixture.Lifecycle.OperationalStatus);
        Assert.Equal(1, fixture.Operations.SaveIntentCount);
        Assert.Same(fixture.Lifecycle, fixture.Operations.SavedLifecycle);
        Assert.Equal(0, fixture.Operations.SeparateWriteCount);
    }

    [Fact]
    public async Task SetDesiredStateAsync_RejectsDisabledWorkload()
    {
        var fixture = new Fixture();
        fixture.Workload.Disable();

        await Assert.ThrowsAsync<WorkloadNotRegisteredException>(
            () => fixture.Handler.SetDesiredStateAsync(fixture.Id, "Running"));

        Assert.Equal(0, fixture.Lifecycles.UpdateCount);
    }

    private sealed class Fixture
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Workload Workload { get; }
        public WorkloadLifecycle Lifecycle { get; }
        public WorkloadRepository Registry { get; }
        public WorkloadLifecycleRepository Lifecycles { get; } = new();
        public WorkloadOperationRepository Operations { get; } = new();
        public WorkloadManagementUseCaseHandler Handler { get; }

        public Fixture()
        {
            var workloadId = WorkloadId.Create(Id);
            Workload = Workload.Create(workloadId, BlockId.Create(Guid.NewGuid()), WorkloadName.Create("unit-test"));
            Lifecycle = WorkloadLifecycle.Create(workloadId);
            Registry = new WorkloadRepository(Workload);
            Lifecycles.Item = Lifecycle;
            Handler = new WorkloadManagementUseCaseHandler(
                new WorkloadManagementCommandHandler(Registry, Lifecycles, Operations),
                new WorkloadManagementQueryHandler(Registry, Lifecycles, Operations),
                new ApplicationExceptionService(Array.Empty<IApplicationExceptionMapper>()));
        }
    }

    private sealed class WorkloadRepository(Workload item) : IWorkloadRepository
    {
        public Task<Workload?> GetByIdAsync(WorkloadId id, CancellationToken cancellationToken = default) =>
            Task.FromResult(item.Id == id && !item.IsDeleted ? item : null);
        public Task<Workload?> GetByNameAsync(BlockId blockId, WorkloadName name, CancellationToken cancellationToken = default) =>
            Task.FromResult<Workload?>(null);
        public Task<IReadOnlyList<Workload>> GetAllAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<Workload>>([]);
        public Task<IReadOnlyList<Workload>> GetByBlockIdAsync(BlockId blockId, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<Workload>>([]);
        public Task<IReadOnlyList<Workload>> GetByRegistrationStatusAsync(WorkloadRegistrationStatus status, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<Workload>>([]);
        public Task<IReadOnlyList<Workload>> GetByAvailabilityStatusAsync(WorkloadAvailabilityStatus status, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<Workload>>([]);
        public Task<IReadOnlyList<Workload>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<Workload>>([]);
        public Task<bool> ExistsByIdAsync(WorkloadId id, CancellationToken cancellationToken = default) =>
            Task.FromResult(item.Id == id);
        public Task<bool> ExistsByNameAsync(BlockId blockId, WorkloadName name, CancellationToken cancellationToken = default) =>
            Task.FromResult(false);
        public Task AddAsync(Workload workload, WorkloadLifecycle lifecycle, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();
        public Task UpdateAsync(Workload workload, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();
    }

    private sealed class WorkloadLifecycleRepository : IWorkloadLifecycleRepository
    {
        public WorkloadLifecycle? Item { get; set; }
        public int UpdateCount { get; private set; }

        public Task<WorkloadLifecycle?> GetByIdAsync(WorkloadId id, CancellationToken cancellationToken = default) =>
            Task.FromResult(Item?.Id == id ? Item : null);
        public Task<IReadOnlyList<WorkloadLifecycle>> GetAllAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<WorkloadLifecycle>>(Item is null ? [] : [Item]);
        public Task AddAsync(WorkloadLifecycle lifecycle, CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();
        public Task UpdateAsync(WorkloadLifecycle lifecycle, CancellationToken cancellationToken = default)
        {
            UpdateCount++;
            return Task.CompletedTask;
        }
    }

    private sealed class WorkloadOperationRepository : IWorkloadOperationRepository
    {
        public int SaveIntentCount { get; private set; }
        public int SeparateWriteCount { get; private set; }
        public WorkloadLifecycle? SavedLifecycle { get; private set; }

        public Task AddAsync(WorkloadOperation operation, CancellationToken cancellationToken = default)
        {
            SeparateWriteCount++;
            return Task.CompletedTask;
        }
        public Task SaveIntentAsync(WorkloadOperation operation, WorkloadLifecycle lifecycle, CancellationToken cancellationToken = default)
        {
            SaveIntentCount++;
            SavedLifecycle = lifecycle;
            return Task.CompletedTask;
        }
        public Task UpdateAsync(WorkloadOperation operation, CancellationToken cancellationToken = default)
        {
            SeparateWriteCount++;
            return Task.CompletedTask;
        }
        public Task<IReadOnlyList<WorkloadOperation>> GetByWorkloadIdAsync(WorkloadId id, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<WorkloadOperation>>([]);
    }
}

using Athena.Application.Common.Interfaces;
using Athena.Application.Common.Services;
using Athena.Application.OrchestrationManagement.Commands;
using Athena.Application.OrchestrationManagement.DTOs;
using Athena.Application.OrchestrationManagement.Enums;
using Athena.Application.OrchestrationManagement.Interfaces;
using Athena.Application.OrchestrationManagement.Queries;
using Athena.Application.OrchestrationManagement.UseCases;
using Athena.Application.SystemManagement.Contracts;
using Athena.Domain.Orchestration.Management.Entities;
using Athena.Domain.Orchestration.Management.Repositories;
using Athena.Domain.Orchestration.Management.ValueObjects;
using Athena.Domain.Orchestration.Registry.ValueObjects;
using Athena.Domain.System.Registry.ValueObjects;

namespace Athena.UnitTests;

public sealed class OrchestrationProcessUseCaseHandlerTests
{
    [Fact]
    public async Task ExecuteOperationAsync_WithoutSystemInstanceAssociations_DoesNotMutateLifecycleOrCreateOperation()
    {
        var commands = new RecordingLifecycleCommands();
        var operations = new RecordingLifecycleOperations();
        var handler = new OrchestrationProcessUseCaseHandler(
            new ApplicationExceptionService(Array.Empty<IApplicationExceptionMapper>()),
            new EmptyLifecycleQueries(),
            commands,
            operations,
            new NoOpStatusSynchronizer(),
            new EmptySystemInstanceAssociations(),
            new UnusedProcessCoordinator());

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.ExecuteOrchestrationOperationAsync(
            new CreateOrchestrationLifecycleOperationCommand
            {
                Id = Guid.NewGuid(),
                OrchestrationId = Guid.NewGuid(),
                Type = OrchestrationLifecycleOperationType.Start
            }));

        Assert.False(commands.WasCalled);
        Assert.False(operations.WasCalled);
    }

    private sealed class EmptyLifecycleQueries : IOrchestrationLifecycleQueries
    {
        public Task<OrchestrationLifecycleDto?> GetOrchestrationLifecycleByIdAsync(
            GetOrchestrationLifecycleByIdQuery query,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<OrchestrationLifecycleDto?>(null);
    }

    private sealed class RecordingLifecycleCommands : IOrchestrationLifecycleCommands
    {
        public bool WasCalled { get; private set; }

        public Task<OrchestrationLifecycleDto> SetOrchestrationDesiredStateAsync(SetOrchestrationDesiredStateCommand command, CancellationToken cancellationToken = default) => Record();
        public Task<OrchestrationLifecycleDto> MarkOrchestrationStartingAsync(MarkOrchestrationStartingCommand command, CancellationToken cancellationToken = default) => Record();
        public Task<OrchestrationLifecycleDto> MarkOrchestrationStoppingAsync(MarkOrchestrationStoppingCommand command, CancellationToken cancellationToken = default) => Record();
        public Task<OrchestrationLifecycleDto> MarkOrchestrationFailedAsync(MarkOrchestrationFailedCommand command, CancellationToken cancellationToken = default) => Record();

        private Task<OrchestrationLifecycleDto> Record()
        {
            WasCalled = true;
            return Task.FromResult(new OrchestrationLifecycleDto());
        }
    }

    private sealed class RecordingLifecycleOperations : IOrchestrationLifecycleOperations
    {
        public bool WasCalled { get; private set; }

        public Task<OrchestrationLifecycleOperationDto> CreateOrchestrationLifecycleOperationAsync(CreateOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default) => Record();
        public Task<OrchestrationLifecycleOperationDto> StartOrchestrationLifecycleOperationAsync(StartOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default) => Record();
        public Task<OrchestrationLifecycleOperationDto> CompleteOrchestrationLifecycleOperationAsync(CompleteOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default) => Record();
        public Task<OrchestrationLifecycleOperationDto> FailOrchestrationLifecycleOperationAsync(FailOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default) => Record();
        public Task<OrchestrationLifecycleOperationDto> CancelOrchestrationLifecycleOperationAsync(CancelOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default) => Record();

        private Task<OrchestrationLifecycleOperationDto> Record()
        {
            WasCalled = true;
            return Task.FromResult(new OrchestrationLifecycleOperationDto());
        }
    }

    private sealed class NoOpStatusSynchronizer : IOrchestrationStatusSynchronizer
    {
        public Task SynchronizeForSystemInstancesAsync(
            IEnumerable<Guid> systemInstanceIds,
            CancellationToken cancellationToken = default,
            Guid? excludedOrchestrationId = null) => Task.CompletedTask;
    }

    private sealed class EmptySystemInstanceAssociations : IOrchestrationSystemInstanceRepository
    {
        public Task<OrchestrationSystemInstance?> GetByIdAsync(OrchestrationSystemInstanceId id, bool includeDeleted = false, CancellationToken cancellationToken = default) =>
            Task.FromResult<OrchestrationSystemInstance?>(null);

        public Task<IReadOnlyCollection<OrchestrationSystemInstance>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyCollection<OrchestrationSystemInstance>>([]);

        public Task<IReadOnlyCollection<OrchestrationSystemInstance>> GetByOrchestrationIdAsync(OrchestrationId orchestrationId, bool includeDeleted = false, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyCollection<OrchestrationSystemInstance>>([]);

        public Task<IReadOnlyCollection<OrchestrationSystemInstance>> GetBySystemInstanceIdAsync(SystemInstanceId systemId, bool includeDeleted = false, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyCollection<OrchestrationSystemInstance>>([]);

        public Task<OrchestrationSystemInstance?> GetByOrchestrationAndSystemInstanceAsync(OrchestrationId orchestrationId, SystemInstanceId systemInstanceId, bool includeDeleted = false, CancellationToken cancellationToken = default) =>
            Task.FromResult<OrchestrationSystemInstance?>(null);

        public Task<bool> ExistsAsync(OrchestrationSystemInstanceId id, CancellationToken cancellationToken = default) =>
            Task.FromResult(false);

        public Task<bool> AssociationExistsAsync(OrchestrationId orchestrationId, SystemInstanceId systemId, bool includeDeleted = true, CancellationToken cancellationToken = default) =>
            Task.FromResult(false);

        public Task AddAsync(OrchestrationSystemInstance association, CancellationToken cancellationToken = default) =>
            Task.CompletedTask;

        public Task UpdateAsync(OrchestrationSystemInstance association, CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
    }

    private sealed class UnusedProcessCoordinator : ISystemInstanceProcessCoordinator
    {
        public Task EnsureOperableAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("No process should be coordinated without associations.");

        public Task<SystemInstanceExecutionStatus> StartAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("No process should be coordinated without associations.");

        public Task<SystemInstanceExecutionStatus> StopAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("No process should be coordinated without associations.");

        public Task<SystemInstanceExecutionStatus> RestartAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("No process should be coordinated without associations.");
    }
}

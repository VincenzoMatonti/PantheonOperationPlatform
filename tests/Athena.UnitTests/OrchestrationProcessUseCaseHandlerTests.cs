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
            new EmptySystemInstanceQueries(),
            new UnusedProcessCoordinator(),
            new UnusedResourceCoordinator());

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

    private sealed class EmptySystemInstanceQueries : IOrchestrationSystemInstanceQueries
    {
        public Task<IReadOnlyList<OrchestrationSystemInstanceDto>> GetByOrchestrationIdAsync(
            GetOrchestrationSystemInstancesByOrchestrationIdQuery query,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<OrchestrationSystemInstanceDto>>([]);
    }

    private sealed class UnusedProcessCoordinator : ISystemInstanceProcessCoordinator
    {
        public Task<SystemInstanceProcessDto?> GetByIdAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("No process should be queried without associations.");

        public Task EnsureOperableAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("No process should be coordinated without associations.");

        public Task<SystemInstanceExecutionStatus> StartAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("No process should be coordinated without associations.");

        public Task<SystemInstanceExecutionStatus> StopAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("No process should be coordinated without associations.");

        public Task<SystemInstanceExecutionStatus> RestartAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("No process should be coordinated without associations.");
    }

    private sealed class UnusedResourceCoordinator : IOrchestrationResourceCoordinator
    {
        public Task<IReadOnlyList<Guid>> ExecuteAsync(Guid orchestrationId, IReadOnlyCollection<Guid> systemInstanceIds, string operationType, CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("No resources should be coordinated without associations.");

        public Task<OrchestrationResourceLifecycleSummary> GetLifecycleSummaryAsync(IEnumerable<Guid> systemInstanceIds, CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("No resources should be summarized without associations.");
    }
}

using Athena.Application.BlockManagement.Contracts;
using Athena.Application.EcosystemManagement.Contracts;
using Athena.Application.OrchestrationManagement.Exceptions;
using Athena.Application.OrchestrationManagement.Interfaces;
using Athena.Application.SystemManagement.Contracts;
using Athena.Application.WorkloadManagement.Contracts;
using Athena.Domain.Block.Registry.Repositories;
using Athena.Domain.Block.Registry.ValueObjects;
using Athena.Domain.Ecosystem.Registry.ValueObjects;
using Athena.Domain.System.Registry.ValueObjects;
using Athena.Domain.Workload.Registry.Repositories;
using Athena.Domain.Workload.Registry.ValueObjects;

namespace Athena.Application.OrchestrationManagement.Services;

public sealed class OrchestrationResourceCoordinator(
    ISystemInstanceProcessCoordinator systemInstances,
    IWorkloadRepository workloads,
    IBlockRepository blocks,
    IEcosystemManagement ecosystems,
    IBlockManagement blockManagement,
    IWorkloadManagement workloadManagement)
    : IOrchestrationResourceCoordinator
{
    public async Task<IReadOnlyList<Guid>> ExecuteAsync(
        Guid orchestrationId,
        IReadOnlyCollection<Guid> systemInstanceIds,
        string operationType,
        CancellationToken cancellationToken = default)
    {
        var type = ParseOperation(operationType);
        var chains = await ResolveChainsAsync(systemInstanceIds, cancellationToken);
        var failures = new Dictionary<Guid, Exception>();
        var completedInstances = new HashSet<Guid>();
        var executedResources = new HashSet<(string Kind, Guid Id)>();
        var failedResources = new HashSet<(string Kind, Guid Id)>();

        if (type == "Stop")
        {
            foreach (var chain in chains)
            {
                await ExecuteOnceAsync("instance", chain.InstanceId, () => ExecuteInstanceAsync(chain.InstanceId, type, cancellationToken),
                    executedResources, failedResources, failures, completedInstances, cancellationToken);
            }

            foreach (var workloadId in chains.Select(chain => chain.WorkloadId).Distinct())
            {
                if (chains.Where(chain => chain.WorkloadId == workloadId)
                    .Any(chain => failedResources.Contains(("instance", chain.InstanceId))))
                {
                    MarkBlocked("workload", workloadId, $"Workload '{workloadId}' was not stopped because a SystemInstance failed to stop.", failedResources, failures);
                    continue;
                }
                await ExecuteOnceAsync("workload", workloadId, () => workloadManagement.ExecuteOperationAsync(workloadId, type, cancellationToken),
                    executedResources, failedResources, failures, null, cancellationToken);
            }

            foreach (var blockId in chains.Select(chain => chain.BlockId).Distinct())
            {
                if (chains.Where(chain => chain.BlockId == blockId)
                    .Any(chain => failedResources.Contains(("workload", chain.WorkloadId))))
                {
                    MarkBlocked("block", blockId, $"Block '{blockId}' was not stopped because a Workload failed to stop.", failedResources, failures);
                    continue;
                }
                await ExecuteOnceAsync("block", blockId, () => blockManagement.ExecuteOperationAsync(blockId, type, cancellationToken),
                    executedResources, failedResources, failures, null, cancellationToken);
            }

            foreach (var ecosystemId in chains.Select(chain => chain.EcosystemId).Distinct())
            {
                if (chains.Where(chain => chain.EcosystemId == ecosystemId)
                    .Any(chain => failedResources.Contains(("block", chain.BlockId))))
                {
                    MarkBlocked("ecosystem", ecosystemId, $"Ecosystem '{ecosystemId}' was not stopped because a Block failed to stop.", failedResources, failures);
                    continue;
                }
                await ExecuteOnceAsync("ecosystem", ecosystemId, () => ecosystems.ExecuteOperationAsync(ecosystemId, type, cancellationToken),
                    executedResources, failedResources, failures, null, cancellationToken);
            }
        }
        else
        {
            foreach (var chain in chains)
            {
                if (!await ExecuteOnceAsync("ecosystem", chain.EcosystemId, () => ecosystems.ExecuteOperationAsync(chain.EcosystemId, type, cancellationToken),
                    executedResources, failedResources, failures, null, cancellationToken))
                {
                    failures.TryAdd(chain.InstanceId, new InvalidOperationException($"Ecosystem '{chain.EcosystemId}' did not reach the requested state."));
                    continue;
                }
                if (!await ExecuteOnceAsync("block", chain.BlockId, () => blockManagement.ExecuteOperationAsync(chain.BlockId, type, cancellationToken),
                    executedResources, failedResources, failures, null, cancellationToken))
                {
                    failures.TryAdd(chain.InstanceId, new InvalidOperationException($"Block '{chain.BlockId}' did not reach the requested state."));
                    continue;
                }
                if (!await ExecuteOnceAsync("workload", chain.WorkloadId, () => workloadManagement.ExecuteOperationAsync(chain.WorkloadId, type, cancellationToken),
                    executedResources, failedResources, failures, null, cancellationToken))
                {
                    failures.TryAdd(chain.InstanceId, new InvalidOperationException($"Workload '{chain.WorkloadId}' did not reach the requested state."));
                    continue;
                }
                await ExecuteOnceAsync("instance", chain.InstanceId, () => ExecuteInstanceAsync(chain.InstanceId, type, cancellationToken),
                    executedResources, failedResources, failures, completedInstances, cancellationToken);
            }
        }

        if (failures.Count > 0)
            throw new OrchestrationSystemInstanceOperationsFailedException(orchestrationId, failures);
        return completedInstances.ToArray();
    }

    public async Task<OrchestrationResourceLifecycleSummary> GetLifecycleSummaryAsync(
        IEnumerable<Guid> systemInstanceIds,
        CancellationToken cancellationToken = default)
    {
        var chains = await ResolveChainsAsync(systemInstanceIds.Distinct().ToArray(), cancellationToken);
        var desiredStates = new List<string>();
        var operationalStatuses = new List<string>();
        var workloadsSeen = new HashSet<Guid>();
        var blocksSeen = new HashSet<Guid>();
        var ecosystemsSeen = new HashSet<Guid>();

        foreach (var chain in chains)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var instance = await systemInstances.GetByIdAsync(chain.InstanceId, cancellationToken);
            if (instance is not null)
            {
                desiredStates.Add(instance.DesiredState);
                operationalStatuses.Add(instance.OperationalStatus);
            }
            if (workloadsSeen.Add(chain.WorkloadId))
            {
                var lifecycle = await workloadManagement.GetLifecycleAsync(chain.WorkloadId, cancellationToken);
                if (lifecycle is not null) { desiredStates.Add(lifecycle.DesiredState); operationalStatuses.Add(lifecycle.OperationalStatus); }
            }
            if (blocksSeen.Add(chain.BlockId))
            {
                var lifecycle = await blockManagement.GetLifecycleAsync(chain.BlockId, cancellationToken);
                if (lifecycle is not null) { desiredStates.Add(lifecycle.DesiredState); operationalStatuses.Add(lifecycle.OperationalStatus); }
            }
            if (ecosystemsSeen.Add(chain.EcosystemId))
            {
                var lifecycle = await ecosystems.GetLifecycleAsync(chain.EcosystemId, cancellationToken);
                if (lifecycle is not null) { desiredStates.Add(lifecycle.DesiredState); operationalStatuses.Add(lifecycle.OperationalStatus); }
            }
        }

        return new OrchestrationResourceLifecycleSummary(
            DetermineDesiredState(desiredStates),
            DetermineOperationalStatus(operationalStatuses));
    }

    private async Task<IReadOnlyList<ResourceChain>> ResolveChainsAsync(IEnumerable<Guid> ids, CancellationToken token)
    {
        var chains = new List<ResourceChain>();
        foreach (var id in ids)
        {
            token.ThrowIfCancellationRequested();
            var instance = await systemInstances.GetByIdAsync(id, token)
                ?? throw new InvalidOperationException($"SystemInstance '{id}' was not found.");
            var workloadId = WorkloadId.Create(instance.WorkloadId);
            var workload = await workloads.GetByIdAsync(workloadId, token)
                ?? throw new InvalidOperationException($"Workload '{instance.WorkloadId}' for SystemInstance '{id}' was not found.");
            var block = await blocks.GetByIdAsync(workload.BlockId, token)
                ?? throw new InvalidOperationException($"Block '{workload.BlockId.Value}' for Workload '{workloadId.Value}' was not found.");
            chains.Add(new ResourceChain(id, workloadId.Value, block.Id.Value, block.EcosystemId.Value));
        }
        return chains;
    }

    private Task<SystemInstanceExecutionStatus> ExecuteInstanceAsync(Guid id, string operation, CancellationToken token)
    {
        return operation switch
        {
            "Start" => systemInstances.StartAsync(id, token),
            "Stop" => systemInstances.StopAsync(id, token),
            "Restart" => systemInstances.RestartAsync(id, token),
            _ => throw new InvalidOperationException($"Unsupported orchestration operation '{operation}'.")
        };
    }

    private static async Task<bool> ExecuteOnceAsync<T>(
        string kind,
        Guid id,
        Func<Task<T>> execute,
        ISet<(string Kind, Guid Id)> executedResources,
        ISet<(string Kind, Guid Id)> failedResources,
        IDictionary<Guid, Exception> failures,
        ISet<Guid>? successfulInstanceIds,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var resource = (kind, id);
        if (!executedResources.Add(resource)) return !failedResources.Contains(resource);
        try
        {
            _ = await execute();
            successfulInstanceIds?.Add(id);
            return true;
        }
        catch (Exception exception) when (!token.IsCancellationRequested)
        {
            failedResources.Add(resource);
            failures.TryAdd(id, exception);
            return false;
        }
    }

    private static string ParseOperation(string operationType) =>
        operationType.Trim().ToLowerInvariant() switch
        {
            "start" => "Start",
            "stop" => "Stop",
            "restart" => "Restart",
            _ => throw new InvalidOperationException($"Unsupported orchestration operation '{operationType}'.")
        };

    private static void MarkBlocked(
        string kind,
        Guid id,
        string message,
        ISet<(string Kind, Guid Id)> failedResources,
        IDictionary<Guid, Exception> failures)
    {
        failedResources.Add((kind, id));
        failures.TryAdd(id, new InvalidOperationException(message));
    }

    private static string DetermineDesiredState(IReadOnlyCollection<string> states) =>
        states.Count > 0 && states.All(state => state == "Running") ? "Running"
        : states.Count > 0 && states.All(state => state == "Stopped") ? "Stopped"
        : "Unknown";

    private static string DetermineOperationalStatus(IReadOnlyCollection<string> states) =>
        states.Any(state => state is "Failed" or "Unreachable") ? "Failed"
        : states.Count > 0 && states.All(state => state == "Running") ? "Running"
        : states.Count > 0 && states.All(state => state == "Stopped") ? "Stopped"
        : "Unknown";

    private sealed record ResourceChain(Guid InstanceId, Guid WorkloadId, Guid BlockId, Guid EcosystemId);
}

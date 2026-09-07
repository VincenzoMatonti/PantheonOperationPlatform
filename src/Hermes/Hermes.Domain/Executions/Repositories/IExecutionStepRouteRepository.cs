using Hermes.Domain.Executions.Entities;

namespace Hermes.Domain.Executions.Repositories;

public interface IExecutionStepRouteRepository
{
    Task<ExecutionStepRoute?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ExecutionStepRoute?> GetAsync(Guid executionStepId, Guid routeId, CancellationToken cancellationToken = default);

    Task<List<ExecutionStepRoute>> GetByExecutionStepIdAsync(Guid executionStepId, CancellationToken cancellationToken = default);

    Task<List<ExecutionStepRoute>> GetByRouteIdAsync(Guid routeId, CancellationToken cancellationToken = default);

    Task AddAsync(ExecutionStepRoute executionStepRoute, CancellationToken cancellationToken = default);

    void Update(ExecutionStepRoute executionStepRoute);
}
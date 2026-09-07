using Hermes.Domain.Executions.Entities;
using Hermes.Domain.Executions.ValueObjects;

namespace Hermes.Domain.Executions.Repositories;

public interface IExecutionStepRepository
{
    Task<ExecutionStep?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<ExecutionStep>> GetByExecutionIdAsync(Guid executionId, CancellationToken cancellationToken = default);

    Task<List<Execution?>> GetByStatusAsync(ExecutionStepStatus status, CancellationToken cancellationToken = default);

    Task AddAsync(ExecutionStep executionStep, CancellationToken cancellationToken = default);

    void Update(ExecutionStep executionStep);
}
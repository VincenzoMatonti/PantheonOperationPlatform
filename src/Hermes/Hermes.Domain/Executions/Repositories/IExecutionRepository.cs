using Hermes.Domain.Executions.Entities;
using Hermes.Domain.Executions.ValueObjects;

namespace Hermes.Domain.Executions.Repositories;

public interface IExecutionRepository
{
    Task<Execution?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Execution?>> GetByStatusAsync(ExecutionStatus status, CancellationToken cancellationToken = default);

    Task AddAsync(Execution execution, CancellationToken cancellationToken = default);

    void Update(Execution execution);
}
using Hermes.Domain.Executions.Entities;

namespace Hermes.Domain.Executions.Repositories;

public interface IExecutionStepTypeRepository
{
    Task<ExecutionStepType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ExecutionStepType?> GetAsync(Guid executionStepId, Guid executionTypeId, CancellationToken cancellationToken = default);

    Task<List<ExecutionStepType>> GetByExecutionStepIdAsync(Guid executionStepId, CancellationToken cancellationToken = default);

    Task<List<ExecutionStepType>> GetByExecutionTypeIdAsync(Guid executionTypeId, CancellationToken cancellationToken = default);

    Task AddAsync(ExecutionStepType executionStepType, CancellationToken cancellationToken = default);

    void Update(ExecutionStepType executionStepType);
}
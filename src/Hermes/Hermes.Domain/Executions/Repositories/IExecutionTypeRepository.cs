using Hermes.Domain.Executions.Entities;
using Hermes.Domain.Executions.ValueObjects;

namespace Hermes.Domain.Executions.Repositories;

public interface IExecutionTypeRepository
{
    Task<ExecutionType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ExecutionType?> GetByCodeAsync(ExecutionTypeCode code, CancellationToken cancellationToken = default);

    Task<List<ExecutionType>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(ExecutionType executionType, CancellationToken cancellationToken = default);

    void Update(ExecutionType executionType);
}
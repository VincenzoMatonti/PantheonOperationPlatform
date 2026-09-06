using Hermes.Domain.Operations.Entities;

namespace Hermes.Domain.Operations.Repositories;

public interface IOperationTypeRepository
{
    Task<OperationType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<OperationType?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task<List<OperationType>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(OperationType operationType, CancellationToken cancellationToken = default);

    void Update(OperationType operationType);
}
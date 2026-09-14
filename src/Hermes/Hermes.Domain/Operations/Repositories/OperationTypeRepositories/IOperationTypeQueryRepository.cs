using Hermes.Domain.Operations.Entities;

namespace Hermes.Domain.Operations.Repositories.OperationTypeRepositories;

public interface IOperationTypeQueryRepository
{
    Task<OperationType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OperationType?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<List<OperationType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<OperationType>> GetAllActive(CancellationToken cancellationToken = default);
    Task<List<OperationType>> GetAllNonActive(CancellationToken cancellationToken = default);
    Task<List<OperationType>> GetAllDeleted(CancellationToken cancellationToken = default);
}
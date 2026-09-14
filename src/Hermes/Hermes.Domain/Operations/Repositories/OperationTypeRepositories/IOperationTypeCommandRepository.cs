using Hermes.Domain.Operations.Entities;

namespace Hermes.Domain.Operations.Repositories.OperationTypeRepositories;

public interface IOperationTypeCommandRepository
{
    Task AddAsync(OperationType operationType, CancellationToken cancellationToken = default);
    void Update(OperationType operationType);
}
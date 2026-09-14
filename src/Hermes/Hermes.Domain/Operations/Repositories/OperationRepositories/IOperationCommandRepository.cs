using Hermes.Domain.Operations.Entities;

namespace Hermes.Domain.Operations.Repositories.OperationRepositories;

public interface IOperationCommandRepository
{
    Task AddAsync(Operation operation, CancellationToken cancellationToken = default);
    void Update(Operation operation);
}
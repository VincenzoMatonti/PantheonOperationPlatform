using Hermes.Domain.Operations.Entities;
using Hermes.Domain.Operations.Repositories.OperationTypeRepositories;
using Hermes.Infrastructure.Persistence;

namespace Hermes.Infrastructure.Repositories.Operations.Commands;

public class OperationTypeCommandRepository(HermesDbContext context) : IOperationTypeCommandRepository
{
    public async Task AddAsync(OperationType operationType, CancellationToken cancellationToken = default)
    {
        await context.OperationTypes.AddAsync(operationType, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public void Update(OperationType operationType)
    {
        context.OperationTypes.Update(operationType);
        context.SaveChanges();
    }
}
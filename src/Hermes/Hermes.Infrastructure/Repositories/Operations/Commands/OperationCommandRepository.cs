using Hermes.Domain.Operations.Entities;
using Hermes.Domain.Operations.Repositories.OperationRepositories;
using Hermes.Infrastructure.Persistence;

namespace Hermes.Infrastructure.Repositories.Operations.Commands;

public class OperationCommandRepository(HermesDbContext context) : IOperationCommandRepository
{
    public async Task AddAsync(Operation operation, CancellationToken cancellationToken = default)
    {
        await context.Operations.AddAsync(operation, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public void Update(Operation operation)
    {
        context.Operations.Update(operation);
        context.SaveChanges();
    }
}
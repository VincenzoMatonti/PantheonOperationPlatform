using Hermes.Domain.Clients.Entities;
using Hermes.Infrastructure.Persistence;
using Hermes.Domain.Clients.Repositories.ClientOperationRepositories;


namespace Hermes.Infrastructure.Repositories.Clients.Commands;

public class ClientOperationCommandRepository(HermesDbContext context) : IClientOperationCommandRepository
{
    public async Task AddAsync(ClientOperation clientOperation, CancellationToken cancellationToken = default)
    {
        await context.ClientOperations.AddAsync(clientOperation, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async void Update(ClientOperation clientOperation)
    {
        context.ClientOperations.Update(clientOperation);
        await context.SaveChangesAsync();
    }
}
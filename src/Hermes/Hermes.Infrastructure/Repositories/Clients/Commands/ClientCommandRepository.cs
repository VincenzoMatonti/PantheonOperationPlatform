using Hermes.Domain.Clients.Entities;
using Hermes.Domain.Clients.Repositories.ClientRepositories;
using Hermes.Infrastructure.Persistence;

namespace Hermes.Infrastructure.Repositories.Clients.Commands;

public class ClientCommandRepository(HermesDbContext context) : IClientCommandRepository
{
    public async Task AddAsync(Client client, CancellationToken cancellationToken = default)
    {
        await context.Clients.AddAsync(client, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public void Update(Client client)
    {
        context.Clients.Update(client);
        context.SaveChangesAsync();
    }
}
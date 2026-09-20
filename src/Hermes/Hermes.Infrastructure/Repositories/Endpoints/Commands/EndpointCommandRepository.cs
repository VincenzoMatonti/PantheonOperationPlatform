using Hermes.Domain.Endpoints.Entities;
using Hermes.Domain.Endpoints.Repositories.EndpointRepositories;
using Hermes.Infrastructure.Persistence;

namespace Hermes.Infrastructure.Repositories.Endpoints.Commands;

public class EndpointCommandRepository(HermesDbContext context) : IEndpointCommandRepository
{
    public async Task AddAsync(Endpoint endpoint, CancellationToken cancellationToken = default)
    {
        await context.Endpoints.AddAsync(endpoint, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public void Update(Endpoint endpoint)
    {
        context.Endpoints.Update(endpoint);
        context.SaveChangesAsync();
    }
}
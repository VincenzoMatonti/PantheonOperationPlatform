using Hermes.Domain.Endpoints.Entities;
using Hermes.Domain.Endpoints.Repositories.EndpointOperationRepositories;
using Hermes.Infrastructure.Persistence;

namespace Hermes.Infrastructure.Repositories.Endpoints.Commands;

public class EndpointOperationCommandRepository(HermesDbContext context) : IEndpointOperationCommandRepository
{
    public async Task AddAsync(EndpointOperation endpointOperation, CancellationToken cancellationToken = default)
    {
        await context.EndpointOperations.AddAsync(endpointOperation, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public void Update(EndpointOperation endpointOperation)
    {
        context.EndpointOperations.Update(endpointOperation);
        context.SaveChangesAsync();
    }
}
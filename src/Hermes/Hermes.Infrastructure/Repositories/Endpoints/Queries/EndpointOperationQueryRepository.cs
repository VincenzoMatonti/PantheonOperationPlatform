using Hermes.Domain.Endpoints.Entities;
using Hermes.Domain.Endpoints.Repositories.EndpointOperationRepositories;
using Hermes.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Infrastructure.Repositories.Endpoints.Queries;

public class EndpointOperationQueryRepository(HermesDbContext context) : IEndpointOperationQueryRepository
{
    public async Task<EndpointOperation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.EndpointOperations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<EndpointOperation?> GetAsync(Guid endpointId, Guid operationTypeId, CancellationToken cancellationToken = default)
    {
        return await context.EndpointOperations.AsNoTracking().FirstOrDefaultAsync(x => x.EndpointId == endpointId &&
                                                                                        x.OperationTypeId == operationTypeId,
                                                                                        cancellationToken);
    }

    public async Task<List<EndpointOperation>> GetByEndpointIdAsync(Guid endpointId, CancellationToken cancellationToken = default)
    {
        return await context.EndpointOperations.AsNoTracking().Where(x => x.EndpointId == endpointId).ToListAsync(cancellationToken);
    }

    public async Task<List<EndpointOperation>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.EndpointOperations.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<List<EndpointOperation>> GetActiveEndpointOperation(CancellationToken cancellationToken = default)
    {
        return await context.EndpointOperations.AsNoTracking().Where(x => x.IsEnabled && !x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<EndpointOperation>> GetNonActiveEndpointOperation(CancellationToken cancellationToken = default)
    {
        return await context.EndpointOperations.AsNoTracking().Where(x => !x.IsEnabled && !x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<EndpointOperation>> GetDeletedEndpointOperation(CancellationToken cancellationToken = default)
    {
        return await context.EndpointOperations.AsNoTracking().Where(x => x.IsDeleted).ToListAsync(cancellationToken);
    }
}
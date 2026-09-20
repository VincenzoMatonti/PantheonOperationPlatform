using Microsoft.EntityFrameworkCore;
using Hermes.Domain.Clients.Entities;
using Hermes.Infrastructure.Persistence;
using Hermes.Domain.Clients.Repositories.ClientOperationRepositories;

namespace Hermes.Infrastructure.Repositories.Clients.Queries;

public class ClientOperationQueryRepository(HermesDbContext context) : IClientOperationQueryRepository
{
    public async Task<ClientOperation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.ClientOperations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<ClientOperation?> GetAsync(Guid clientId, Guid operationTypeId, CancellationToken cancellationToken = default)
    {
        return await context.ClientOperations.AsNoTracking().FirstOrDefaultAsync(x => x.ClientId == clientId && x.OperationTypeId == operationTypeId, cancellationToken);
    }

    public async Task<List<ClientOperation>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        return await context.ClientOperations
            .AsNoTracking().Where(x => x.ClientId == clientId).ToListAsync(cancellationToken);
    }

    public async Task<List<ClientOperation>> GetByOperationIdAsync(Guid operationTypeId, CancellationToken cancellationToken = default)
    {
        return await context.ClientOperations.AsNoTracking().Where(x => x.OperationTypeId == operationTypeId).ToListAsync(cancellationToken);
    }

    public async Task<List<ClientOperation>> GetActiveClientOperation(CancellationToken cancellationToken = default)
    {
        return await context.ClientOperations.AsNoTracking().Where(x => x.IsEnabled && !x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<ClientOperation>> GetNonActiveClientOperation(CancellationToken cancellationToken = default)
    {
        return await context.ClientOperations.AsNoTracking().Where(x => !x.IsEnabled && !x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<ClientOperation>> GetDeletedClientOperation(CancellationToken cancellationToken = default)
    {
        return await context.ClientOperations.AsNoTracking().Where(x => x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<ClientOperation>> GetByOperationTypeIdAsync(Guid operationTypeId, CancellationToken cancellationToken = default)
    {
        return await context.ClientOperations.AsNoTracking().Where(x => x.OperationTypeId == operationTypeId).ToListAsync(cancellationToken: cancellationToken);
        
    }
}
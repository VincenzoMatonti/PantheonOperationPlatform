using Hermes.Domain.Clients.Entities;
using Hermes.Domain.Clients.Repositories.ClientRepositories;
using Hermes.Domain.Clients.ValueObjects;
using Hermes.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Infrastructure.Repositories.Clients.Queries;

public class ClientQueryRepository(HermesDbContext context) : IClientQueryRepository
{
    public async Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Clients.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Client?> GetByCodeAsync(ClientCode code, CancellationToken cancellationToken = default)
    {
        return await context.Clients.AsNoTracking().FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
    }

    public async Task<List<ClientCode>> GetAllClientCodeAsync(CancellationToken cancellationToken = default)
    {
        return await context.Clients.AsNoTracking().Select(x => x.Code).ToListAsync(cancellationToken);
    }

    public async Task<ClientCode?> GetClientCodeByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Clients.AsNoTracking().Where(x => x.Id == id).Select(x => x.Code).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Client>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Clients.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<List<Client>> GetActiveClientAsync(CancellationToken cancellationToken = default)
    {
        return await context.Clients.AsNoTracking().Where(x => x.IsActive && !x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<Client>> GetNonActiveClientAsync(CancellationToken cancellationToken = default)
    {
        return await context.Clients.AsNoTracking().Where(x => !x.IsActive && !x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<Client>> GetDeletedClientAsync(CancellationToken cancellationToken = default)
    {
        return await context.Clients.AsNoTracking().Where(x => x.IsDeleted).ToListAsync(cancellationToken);
    }
}
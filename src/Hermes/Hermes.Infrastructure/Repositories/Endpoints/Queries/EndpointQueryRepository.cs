using Hermes.Domain.Endpoints.Entities;
using Hermes.Domain.Endpoints.Repositories.EndpointRepositories;
using Hermes.Domain.Endpoints.ValueObjects;
using Hermes.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Infrastructure.Repositories.Endpoints.Queries;

public class EndpointQueryRepository(HermesDbContext context) : IEndpointRepository
{
    public async Task<Endpoint?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Endpoints.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Endpoint?> GetByCodeAsync(EndpointCode code, CancellationToken cancellationToken = default)
    {
        return await context.Endpoints.AsNoTracking().FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
    }

    public async Task<Endpoint?> GetByTypeAsync(EndpointType type, CancellationToken cancellationToken = default)
    {
        return await context.Endpoints.AsNoTracking().FirstOrDefaultAsync(x => x.Type == type, cancellationToken);
    }

    public async Task<List<Endpoint>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Endpoints.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<List<EndpointCode>> GetAllEndpointCodeAsync(CancellationToken cancellationToken = default)
    {
        return await context.Endpoints.AsNoTracking().Select(x => x.Code).ToListAsync(cancellationToken);
    }

    public async Task<EndpointCode?> GetEndpointCodeByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Endpoints.AsNoTracking().Where(x => x.Id == id).Select(x => x.Code).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Endpoint>> GetActiveEndpointAsync(CancellationToken cancellationToken = default)
    {
        return await context.Endpoints.AsNoTracking().Where(x => x.IsActive && !x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<Endpoint>> GetNonActiveEndpointAsync(CancellationToken cancellationToken = default)
    {
        return await context.Endpoints.AsNoTracking().Where(x => !x.IsActive && !x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<Endpoint>> GetDeletedEndpointAsync(CancellationToken cancellationToken = default)
    {
        return await context.Endpoints.AsNoTracking().Where(x => x.IsDeleted).ToListAsync(cancellationToken);
    }
}
using Hermes.Domain.Routes.Entities;
using Hermes.Domain.Routes.Repositories;
using Hermes.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Infrastructure.Repositories.Routes;

public class RouteRepository(HermesDbContext context) : IRouteRepository
{
    public async Task<List<Route>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().Where(x => x.ClientId == clientId).ToListAsync(cancellationToken);
    }

    public async Task<List<Route>> GetByOperationTypeIdAsync(Guid operationTypeId, CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().Where(x => x.OperationTypeId == operationTypeId).ToListAsync(cancellationToken);
    }

    public async Task<List<Route>> GetByEndpointIdAsync(Guid endpointId, CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().Where(x => x.EndpointId == endpointId).ToListAsync(cancellationToken);
    }

    public async Task<List<Route>> GetByClientIdAndOperationTypeIdAsync(Guid clientId, Guid operationTypeId, CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().Where(x => x.ClientId == clientId && x.OperationTypeId == operationTypeId).ToListAsync(cancellationToken);
    }

    public async Task<List<Route>> GetByClientIdAndEndpointIdAsync(Guid clientId, Guid endpointId, CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().Where(x => x.ClientId == clientId && x.EndpointId == endpointId).ToListAsync(cancellationToken);
    }

    public async Task<List<Route>> GetByOperationTypeIdAndEndpointIdAsync(Guid operationTypeId, Guid endpointId, CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().Where(x => x.OperationTypeId == operationTypeId && x.EndpointId == endpointId).ToListAsync(cancellationToken);
    }

    public async Task<Route?> GetAsync(Guid clientId, Guid operationTypeId, Guid endpointId, CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().FirstOrDefaultAsync(x => x.ClientId == clientId && x.OperationTypeId == operationTypeId && x.EndpointId == endpointId, cancellationToken);
    }

    public async Task<Route?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<Route>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<List<Route>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().Where(x => x.IsActive && !x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<Route>> GetNonActiveAsync(CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().Where(x => !x.IsActive && !x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<Route>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().Where(x => x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<Route>> GetActiveByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().Where(x => x.ClientId == clientId && x.IsActive && !x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<Route>> GetNonActiveByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().Where(x => x.ClientId == clientId && !x.IsActive && !x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<Route>> GetDeletedByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().Where(x => x.ClientId == clientId && x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<Route>> GetActiveByOperationTypeIdAsync(Guid operationTypeId, CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().Where(x => x.OperationTypeId == operationTypeId && x.IsActive && !x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<Route>> GetNonActiveByOperationTypeIdAsync(
        Guid operationTypeId,
        CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().Where(x => x.OperationTypeId == operationTypeId && !x.IsActive && !x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<Route>> GetDeletedByOperationTypeIdAsync(Guid operationTypeId, CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().Where(x => x.OperationTypeId == operationTypeId && x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<Route>> GetActiveByEndpointIdAsync(Guid endpointId, CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().Where(x => x.EndpointId == endpointId && x.IsActive && !x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<Route>> GetNonActiveByEndpointIdAsync(Guid endpointId, CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().Where(x => x.EndpointId == endpointId && !x.IsActive && !x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<Route>> GetDeletedByEndpointIdAsync(Guid endpointId, CancellationToken cancellationToken = default)
    {
        return await context.Routes.AsNoTracking().Where(x => x.EndpointId == endpointId && x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Route route, CancellationToken cancellationToken = default)
    {
        await context.Routes.AddAsync(route, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public void Update(Route route)
    {
        context.Routes.Update(route);
        context.SaveChangesAsync();
    }
}
using Hermes.Domain.Operations.Entities;
using Hermes.Domain.Operations.Repositories.OperationTypeRepositories;
using Hermes.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Infrastructure.Repositories.Operations.Queries;

public class OperationTypeQueryRepository(HermesDbContext context) : IOperationTypeQueryRepository
{
    public async Task<OperationType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.OperationTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<OperationType?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await context.OperationTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Code.Value == code, cancellationToken);
    }

    public async Task<List<OperationType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.OperationTypes.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<List<OperationType>> GetAllActive(CancellationToken cancellationToken = default)
    {
        return await context.OperationTypes.AsNoTracking().Where(x => x.IsActive && !x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<OperationType>> GetAllNonActive(CancellationToken cancellationToken = default)
    {
        return await context.OperationTypes.AsNoTracking().Where(x => !x.IsActive && !x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<OperationType>> GetAllDeleted(CancellationToken cancellationToken = default)
    {
        return await context.OperationTypes.AsNoTracking().Where(x => x.IsDeleted).ToListAsync(cancellationToken);
    }
}
using Hermes.Domain.Operations.Entities;
using Hermes.Domain.Operations.Repositories.OperationRepositories;
using Hermes.Domain.Operations.ValueObjects;
using Hermes.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Infrastructure.Repositories.Operations.Queries;

public class OperationQueryRepository(HermesDbContext context) : IOperationQueryRepository
{
    public async Task<List<Operation>> GetAll(CancellationToken cancellationToken = default)
    {
        return await context.Operations.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<List<Operation>> GetAllDeleted(CancellationToken cancellationToken = default)
    {
        return await context.Operations.AsNoTracking().Where(x => x.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<OperationTypeCode>> GetAllCode(CancellationToken cancellationToken = default)
    {
        return await context.OperationTypes.Select(x => x.Code).ToListAsync(cancellationToken);
    }

    public async Task<Operation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Operations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Operation?> GetByCode(OperationTypeCode code, CancellationToken cancellationToken = default)
    {
        // TODO: implementare in base al modello Operation corrente
        throw new NotImplementedException();
    }

    public async Task<Operation?> GetByExternalIdAsync(ExternalOperationId externalId, CancellationToken cancellationToken = default)
    {
        return await context.Operations.AsNoTracking().FirstOrDefaultAsync(x => x.ExternalId == externalId, cancellationToken);
    }

    public async Task<Operation?> GetByCorrelationIdAsync(CorrelationId correlationId, CancellationToken cancellationToken = default)
    {
        return await context.Operations.AsNoTracking().FirstOrDefaultAsync(x => x.CorrelationId == correlationId, cancellationToken);
    }

    public async Task<List<Operation>> GetByStatusAsync(OperationStatus status, CancellationToken cancellationToken = default)
    {
        return await context.Operations.AsNoTracking().Where(x => x.Status == status).ToListAsync(cancellationToken);
    }

    public async Task<List<Operation>> GetByExecutionIdAsync(Guid executionId, CancellationToken cancellationToken = default)
    {
        return await context.Operations.AsNoTracking().Where(x => x.ExecutionId == executionId).ToListAsync(cancellationToken);
    }
}
using Athena.Domain.System.Registry.Entities;
using Athena.Domain.System.Registry.Repositories;
using Athena.Domain.System.Registry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.Infrastructure.Repositories;

public sealed class SystemInstanceOperationRepository(AthenaDbContext context) : ISystemInstanceOperationRepository
{
    public async Task AddAsync(SystemInstanceOperation operation, CancellationToken cancellationToken = default)
    {
        var systemInstanceDatabaseId = await context.SystemInstances
            .Where(instance => instance.Id == operation.SystemInstanceId)
            .Select(instance => (int?)EF.Property<int>(instance, "id"))
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"SystemInstance '{operation.SystemInstanceId.Value}' does not exist.");

        await context.SystemInstanceOperations.AddAsync(operation, cancellationToken);
        context.Entry(operation).Property<int>("system_instance_database_id").CurrentValue = systemInstanceDatabaseId;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(SystemInstanceOperation operation, CancellationToken cancellationToken = default)
    {
        var entry = context.Entry(operation);
        if (entry.State == EntityState.Detached) context.SystemInstanceOperations.Update(operation);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SystemInstanceOperation>> GetBySystemInstanceIdAsync(
        SystemInstanceId systemInstanceId,
        CancellationToken cancellationToken = default) =>
        await context.SystemInstanceOperations
            .AsNoTracking()
            .Where(operation => operation.SystemInstanceId == systemInstanceId)
            .OrderByDescending(operation => operation.CreatedAtUtc)
            .ToListAsync(cancellationToken);
}

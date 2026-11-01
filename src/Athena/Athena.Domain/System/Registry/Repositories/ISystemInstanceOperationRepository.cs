using Athena.Domain.System.Registry.Entities;
using Athena.Domain.System.Registry.ValueObjects;

namespace Athena.Domain.System.Registry.Repositories;

public interface ISystemInstanceOperationRepository
{
    Task AddAsync(SystemInstanceOperation operation, CancellationToken cancellationToken = default);
    Task UpdateAsync(SystemInstanceOperation operation, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemInstanceOperation>> GetBySystemInstanceIdAsync(SystemInstanceId systemInstanceId, CancellationToken cancellationToken = default);
}

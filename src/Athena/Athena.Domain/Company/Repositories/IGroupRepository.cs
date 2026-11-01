using Athena.Domain.Company.Enums;
using Athena.Domain.Company.Entities;
using Athena.Domain.Company.ValueObjects;

namespace Athena.Domain.Company.Repositories;

public interface IGroupRepository
{
    // Retrieval
    Task<Group?> GetByIdAsync(GroupId id, CancellationToken cancellationToken = default);
    Task<Group?> GetByNameAsync(GroupName name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Group>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Group>> GetByRegistrationStatusAsync(GroupRegistrationStatus status, CancellationToken cancellationToken = default);

    // Soft deletion
    Task<IReadOnlyList<Group>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default);

    // Existence checks
    Task<bool> ExistsByIdAsync(GroupId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(GroupName name, CancellationToken cancellationToken = default);

    // Persistence
    Task AddAsync(Group group, CancellationToken cancellationToken = default);
    Task UpdateAsync(Group group, CancellationToken cancellationToken = default);
}
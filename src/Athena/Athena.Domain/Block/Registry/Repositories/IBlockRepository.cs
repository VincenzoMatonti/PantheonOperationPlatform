using BlockEntity = Athena.Domain.Block.Registry.Entities.Block;
using Athena.Domain.Block.Registry.Enums;
using Athena.Domain.Block.Registry.ValueObjects;
using Athena.Domain.Ecosystem.Registry.ValueObjects;

namespace Athena.Domain.Block.Registry.Repositories;

public interface IBlockRepository
{
    // Retrieval
    Task<BlockEntity?> GetByIdAsync(BlockId id, CancellationToken cancellationToken = default);
    Task<BlockEntity?> GetByNameAsync(BlockName name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BlockEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BlockEntity>> GetByEcosystemIdAsync(EcosystemId ecosystemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BlockEntity>> GetByRegistrationStatusAsync(BlockRegistrationStatus status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BlockEntity>> GetByAvailabilityStatusAsync(BlockAvailabilityStatus status, CancellationToken cancellationToken = default);

    // Soft deletion
    Task<IReadOnlyList<BlockEntity>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default);

    // Existence checks
    Task<bool> ExistsByIdAsync(BlockId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(BlockName name, CancellationToken cancellationToken = default);

    // Persistence
    Task AddAsync(BlockEntity block, CancellationToken cancellationToken = default);
    Task UpdateAsync(BlockEntity block, CancellationToken cancellationToken = default);
}
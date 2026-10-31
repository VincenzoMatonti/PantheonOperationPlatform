using Athena.Domain.Block.Registry.Entities;
using Athena.Domain.Block.Registry.Enums;
using Athena.Domain.Block.Registry.Repositories;
using Athena.Domain.Block.Registry.ValueObjects;
using Athena.Domain.Ecosystem.Registry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.Infrastructure.Repositories;

public sealed class BlockRepository(AthenaDbContext context) : IBlockRepository
{
    private IQueryable<Block> ActiveBlocks => context.Blocks.Where(block => !block.IsDeleted);

    // Retrieval
    public async Task<Block?> GetByIdAsync(BlockId id, CancellationToken cancellationToken = default) => await ActiveBlocks.FirstOrDefaultAsync(block => block.Id == id, cancellationToken);

    public async Task<Block?> GetByNameAsync(BlockName name, CancellationToken cancellationToken = default) => await ActiveBlocks.FirstOrDefaultAsync(block => block.Name == name, cancellationToken);

    public async Task<IReadOnlyList<Block>> GetAllAsync(CancellationToken cancellationToken = default) => await ActiveBlocks.ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Block>> GetByEcosystemIdAsync(EcosystemId ecosystemId, CancellationToken cancellationToken = default) =>
            await ActiveBlocks.Where(block => block.EcosystemId == ecosystemId).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Block>> GetByRegistrationStatusAsync(BlockRegistrationStatus status, CancellationToken cancellationToken = default) =>
            await ActiveBlocks.Where(block => block.RegistrationStatus == status).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Block>> GetByAvailabilityStatusAsync(BlockAvailabilityStatus status, CancellationToken cancellationToken = default) =>
            await ActiveBlocks.Where(block => block.AvailabilityStatus == status).ToListAsync(cancellationToken);

    // Soft deletion
    public async Task<IReadOnlyList<Block>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default) =>
            await context.Blocks.Where(block => block.IsDeleted == isDeleted).ToListAsync(cancellationToken);

    // Existence checks
    public async Task<bool> ExistsByIdAsync(BlockId id, CancellationToken cancellationToken = default) => await context.Blocks.AnyAsync(block => block.Id == id, cancellationToken);
    public async Task<bool> ExistsByNameAsync(BlockName name, CancellationToken cancellationToken = default) => await context.Blocks.AnyAsync(block => block.Name == name, cancellationToken);

    // Persistence
    public async Task AddAsync(Block block, CancellationToken cancellationToken = default)
    {
        await context.Blocks.AddAsync(block, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Block block, CancellationToken cancellationToken = default)
    {
        context.Blocks.Update(block);
        await context.SaveChangesAsync(cancellationToken);
    }
}
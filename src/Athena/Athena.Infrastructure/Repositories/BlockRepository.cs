using Athena.Domain.Block.Registry.Entities;
using Athena.Domain.Block.Registry.Enums;
using Athena.Domain.Block.Registry.Repositories;
using Athena.Domain.Block.Registry.ValueObjects;
using Athena.Domain.Block.Management.Entities;
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
    public async Task AddAsync(Block block, BlockLifecycle lifecycle, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(block);
        ArgumentNullException.ThrowIfNull(lifecycle);
        if (lifecycle.Id != block.Id)
            throw new ArgumentException("The lifecycle must belong to the Block being created.", nameof(lifecycle));

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        var ecosystemDatabaseId = await context.Ecosystems
            .Where(ecosystem => ecosystem.Id == block.EcosystemId && !ecosystem.IsDeleted)
            .Select(ecosystem => (int?)EF.Property<int>(ecosystem, "id"))
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Ecosystem '{block.EcosystemId.Value}' does not exist.");
        context.Entry(block).Property<int>("ecosystem_database_id").CurrentValue = ecosystemDatabaseId;
        await context.Blocks.AddAsync(block, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var blockDatabaseId = await context.Blocks
            .Where(entity => entity.Id == block.Id)
            .Select(entity => EF.Property<int>(entity, "id"))
            .SingleAsync(cancellationToken);
        context.Entry(lifecycle).Property<int>("block_database_id").CurrentValue = blockDatabaseId;
        await context.BlockLifecycles.AddAsync(lifecycle, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task UpdateAsync(Block block, CancellationToken cancellationToken = default)
    {
        var blockEntry = context.Entry(block);
        if (blockEntry.State == EntityState.Detached) context.Blocks.Update(block);
        if (block.IsDeleted || block.RegistrationStatus != BlockRegistrationStatus.Registered)
        {
            var blockDatabaseId = await context.Blocks
                .Where(entity => entity.Id == block.Id)
                .Select(entity => (int?)EF.Property<int>(entity, "id"))
                .SingleOrDefaultAsync(cancellationToken);
            if (blockDatabaseId is not null)
            {
                var lifecycle = await context.BlockLifecycles.SingleOrDefaultAsync(
                    entity => EF.Property<int>(entity, "block_database_id") == blockDatabaseId,
                    cancellationToken);
                if (lifecycle is not null && !lifecycle.IsDeleted)
                {
                    if (block.IsDeleted) lifecycle.Delete();
                    else lifecycle.SetOperationalStatus(Athena.Domain.Block.Management.Enums.BlockOperationalStatus.Unknown);
                }
            }
        }
        await context.SaveChangesAsync(cancellationToken);
    }
}
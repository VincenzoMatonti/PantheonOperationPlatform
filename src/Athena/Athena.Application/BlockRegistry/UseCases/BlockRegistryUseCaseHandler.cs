using Athena.Application.BlockRegistry.Commands;
using Athena.Application.BlockRegistry.Contracts;
using Athena.Application.BlockRegistry.Exceptions;
using Athena.Application.BlockRegistry.Queries;
using Athena.Application.Common.Services;
using Athena.Application.EcosystemRegistry.Exceptions;
using Athena.Domain.Ecosystem.Registry.Repositories;
using Athena.Domain.Ecosystem.Registry.ValueObjects;

namespace Athena.Application.BlockRegistry.UseCases;

public sealed class BlockRegistryUseCaseHandler(BlockRegistryCommandHandler commandHandler, BlockRegistryQueryHandler queryHandler, IEcosystemRepository ecosystemRepository, ApplicationExceptionService exceptionService)
{
    private readonly BlockRegistryCommandHandler _commandHandler = commandHandler;
    private readonly BlockRegistryQueryHandler _queryHandler = queryHandler;
    private readonly IEcosystemRepository _ecosystemRepository = ecosystemRepository;
    private readonly ApplicationExceptionService _exceptionService = exceptionService;

    // Creation
    public Task<BlockDto> CreateBlockAsync(CreateBlockCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        await EnsureEcosystemExistsAsync(command.EcosystemId, cancellationToken);
        if (await _queryHandler.NameExistsAsync(command.Name, cancellationToken)) throw new BlockNameAlreadyExistsException(command.Name);
        return await _commandHandler.CreateAsync(command, cancellationToken);
    });

    // Registration
    public Task<BlockDto> RegisterBlockAsync(RegisterBlockCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        var block = await _queryHandler.GetByIdAsync(new GetBlockByIdQuery { BlockId = command.BlockId }, cancellationToken) ?? throw new BlockNotFoundException(command.BlockId);
        return await _commandHandler.RegisterAsync(command, cancellationToken) ?? throw new BlockNotFoundException(block.Id);
    });

    public Task<BlockDto> UnregisterBlockAsync(UnregisterBlockCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.UnregisterAsync(command, cancellationToken) ?? throw new BlockNotFoundException(command.BlockId));

    // Naming
    public Task<BlockDto> RenameBlockAsync(RenameBlockCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        var current = await GetBlockOrThrowAsync(command.BlockId, cancellationToken);
        if (await _queryHandler.NameExistsAsync(command.NewName, cancellationToken))
        {
            var existing = await _queryHandler.GetByNameAsync(new GetBlockByNameQuery { Name = command.NewName }, cancellationToken);
            if (existing is null || existing.Id != current.Id) throw new BlockNameAlreadyExistsException(command.NewName);
        }

        return await _commandHandler.RenameAsync(command, cancellationToken) ?? throw new BlockNotFoundException(command.BlockId);
    });

    // Enable / Disable
    public Task<BlockDto> EnableBlockAsync(EnableBlockCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.EnableAsync(command, cancellationToken) ?? throw new BlockNotFoundException(command.BlockId));

    public Task<BlockDto> DisableBlockAsync(DisableBlockCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.DisableAsync(command, cancellationToken) ?? throw new BlockNotFoundException(command.BlockId));

    // Deletion
    public Task<BlockDto> DeleteBlockAsync(DeleteBlockCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.DeleteAsync(command, cancellationToken) ?? throw new BlockNotFoundException(command.BlockId));

    // Availability
    public Task<BlockDto> MarkBlockAvailableAsync(MarkBlockAvailableCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.MarkAvailableAsync(command, cancellationToken) ?? throw new BlockNotFoundException(command.BlockId));

    public Task<BlockDto> MarkBlockUnavailableAsync(MarkBlockUnavailableCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.MarkUnavailableAsync(command, cancellationToken) ?? throw new BlockNotFoundException(command.BlockId));

    public Task<BlockDto> MarkBlockAvailabilityUnknownAsync(MarkBlockAvailabilityUnknownCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.MarkAvailabilityUnknownAsync(command, cancellationToken) ?? throw new BlockNotFoundException(command.BlockId));

    public Task<BlockDto> MarkBlockErrorAsync(MarkBlockErrorCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.MarkErrorAsync(command, cancellationToken) ?? throw new BlockNotFoundException(command.BlockId));

    // Retrieval
    public Task<BlockDto> GetBlockByIdAsync(GetBlockByIdQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _queryHandler.GetByIdAsync(query, cancellationToken) ?? throw new BlockNotFoundException(query.BlockId));

    public Task<BlockDto> GetBlockByNameAsync(GetBlockByNameQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _queryHandler.GetByNameAsync(query, cancellationToken) ?? throw new BlockNotFoundByNameException(query.Name));

    public Task<List<BlockDto>> GetAllBlocksAsync(CancellationToken cancellationToken = default) => ExecuteAsync(async () => (await _queryHandler.GetAllAsync(cancellationToken)).ToList());

    public Task<List<BlockDto>> GetBlocksByEcosystemIdAsync(GetBlocksByEcosystemIdQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        await EnsureEcosystemExistsAsync(query.EcosystemId, cancellationToken);
        return (await _queryHandler.GetByEcosystemIdAsync(query, cancellationToken)).ToList();
    });

    public Task<List<BlockDto>> GetBlocksByRegistrationStatusAsync(GetBlocksByRegistrationStatusQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        if (!Enum.IsDefined(query.Status)) throw new ArgumentOutOfRangeException(nameof(query.Status), query.Status, "Invalid block registration status filter.");
        return (await _queryHandler.GetByRegistrationStatusAsync(query, cancellationToken)).ToList();
    });

    public Task<List<BlockDto>> GetBlocksByAvailabilityStatusAsync(GetBlocksByAvailabilityStatusQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        if (!Enum.IsDefined(query.Status)) throw new ArgumentOutOfRangeException(nameof(query.Status), query.Status, "Invalid block availability status filter.");
        return (await _queryHandler.GetByAvailabilityStatusAsync(query, cancellationToken)).ToList();
    });

    public Task<List<BlockDto>> GetBlocksByDeletionStatusAsync(GetBlocksByDeletionStatusQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => (await _queryHandler.GetByDeletionStatusAsync(query, cancellationToken)).ToList());

    public Task<List<BlockDto>> GetDeletedBlocksAsync(CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => (await _queryHandler.GetByDeletionStatusAsync(new GetBlocksByDeletionStatusQuery { IsDeleted = true }, cancellationToken)).ToList());

    // Internal validation
    private async Task<BlockDto> GetBlockOrThrowAsync(Guid blockId, CancellationToken cancellationToken) =>
        await _queryHandler.GetByIdAsync(new GetBlockByIdQuery { BlockId = blockId }, cancellationToken) ?? throw new BlockNotFoundException(blockId);

    private async Task EnsureEcosystemExistsAsync(Guid ecosystemId, CancellationToken cancellationToken)
    {
        var ecosystem = await _ecosystemRepository.GetByIdAsync(EcosystemId.Create(ecosystemId), cancellationToken);
        if (ecosystem is null || ecosystem.IsDeleted) throw new EcosystemNotFoundException(ecosystemId);
    }

    private Task<T> ExecuteAsync<T>(Func<Task<T>> operation) => _exceptionService.ExecuteAsync(operation);
}
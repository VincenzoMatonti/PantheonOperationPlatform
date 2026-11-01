using Athena.Application.WorkloadRegistry.Commands;
using Athena.Application.WorkloadRegistry.Contracts;
using Athena.Application.WorkloadRegistry.Enums;
using Athena.Application.WorkloadRegistry.Exceptions;
using Athena.Application.WorkloadRegistry.Queries;
using Athena.Application.Common.Services;
using Athena.Domain.Block.Registry.Repositories;
using Athena.Domain.Block.Registry.ValueObjects;
using Athena.Application.BlockRegistry.Exceptions;

namespace Athena.Application.WorkloadRegistry.UseCases;

public sealed class WorkloadRegistryUseCaseHandler(
    WorkloadRegistryCommandHandler commandHandler,
    WorkloadRegistryQueryHandler queryHandler,
    IBlockRepository blockRepository,
    ApplicationExceptionService exceptionService)
{
    private readonly WorkloadRegistryCommandHandler _commandHandler = commandHandler;
    private readonly WorkloadRegistryQueryHandler _queryHandler = queryHandler;
    private readonly IBlockRepository _blockRepository = blockRepository;
    private readonly ApplicationExceptionService _exceptionService = exceptionService;

    // Creation
    public Task<WorkloadDto> CreateWorkloadAsync(CreateWorkloadCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        await EnsureBlockExistsAsync(command.BlockId, cancellationToken);
        if (await _queryHandler.NameExistsAsync(command.BlockId, command.Name, cancellationToken)) throw new WorkloadNameAlreadyExistsException(command.BlockId, command.Name);
        return await _commandHandler.HandleAsync(command, cancellationToken);
    });

    // Registration
    public Task<WorkloadDto> RegisterWorkloadAsync(RegisterWorkloadCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.HandleAsync(command, cancellationToken) ?? throw new WorkloadNotFoundException(command.Id));

    public Task<WorkloadDto> UnregisterWorkloadAsync(UnregisterWorkloadCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.HandleAsync(command, cancellationToken) ?? throw new WorkloadNotFoundException(command.Id));

    // Naming
    public Task<WorkloadDto> RenameWorkloadAsync(RenameWorkloadCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        var current = await GetWorkloadOrThrowAsync(command.Id, cancellationToken);
        if (await _queryHandler.NameExistsAsync(current.BlockId, command.Name, cancellationToken))
        {
            var existing = await _queryHandler.HandleAsync(new GetWorkloadByNameQuery(current.BlockId, command.Name), cancellationToken);
            if (existing is null || existing.Id != current.Id) throw new WorkloadNameAlreadyExistsException(current.BlockId, command.Name);
        }

        return await _commandHandler.HandleAsync(command, cancellationToken) ?? throw new WorkloadNotFoundException(command.Id);
    });

    // Enable / Disable
    public Task<WorkloadDto> EnableWorkloadAsync(EnableWorkloadCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.HandleAsync(command, cancellationToken) ?? throw new WorkloadNotFoundException(command.Id));

    public Task<WorkloadDto> DisableWorkloadAsync(DisableWorkloadCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.HandleAsync(command, cancellationToken) ?? throw new WorkloadNotFoundException(command.Id));

    // Deletion
    public Task<WorkloadDto> DeleteWorkloadAsync(DeleteWorkloadCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.HandleAsync(command, cancellationToken) ?? throw new WorkloadNotFoundException(command.Id));

    // Availability
    public Task<WorkloadDto> MarkWorkloadAvailableAsync(MarkWorkloadAvailableCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.HandleAsync(command, cancellationToken) ?? throw new WorkloadNotFoundException(command.Id));

    public Task<WorkloadDto> MarkWorkloadUnavailableAsync(MarkWorkloadUnavailableCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.HandleAsync(command, cancellationToken) ?? throw new WorkloadNotFoundException(command.Id));

    public Task<WorkloadDto> MarkWorkloadAvailabilityUnknownAsync(MarkWorkloadAvailabilityUnknownCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.HandleAsync(command, cancellationToken) ?? throw new WorkloadNotFoundException(command.Id));

    public Task<WorkloadDto> MarkWorkloadErrorAsync(MarkWorkloadErrorCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.HandleAsync(command, cancellationToken) ?? throw new WorkloadNotFoundException(command.Id));

    // Retrieval
    public Task<WorkloadDto> GetWorkloadByIdAsync(GetWorkloadByIdQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _queryHandler.HandleAsync(query, cancellationToken) ?? throw new WorkloadNotFoundException(query.Id));

    public Task<WorkloadDto> GetWorkloadByNameAsync(GetWorkloadByNameQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _queryHandler.HandleAsync(query, cancellationToken) ?? throw new WorkloadNotFoundByNameException(query.BlockId, query.Name));

    public Task<List<WorkloadDto>> GetAllWorkloadsAsync(CancellationToken cancellationToken = default) => ExecuteAsync(async () => (await _queryHandler.HandleAsync(cancellationToken)).ToList());

    public Task<List<WorkloadDto>> GetWorkloadsByBlockIdAsync(GetWorkloadsByBlockIdQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        await EnsureBlockExistsAsync(query.BlockId, cancellationToken);
        return (await _queryHandler.HandleAsync(query, cancellationToken)).ToList();
    });

    public Task<List<WorkloadDto>> GetWorkloadsByRegistrationStatusAsync(GetWorkloadsByRegistrationStatusQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        if (!Enum.IsDefined((WorkloadRegistrationStatusFilter)query.Status)) throw new ArgumentOutOfRangeException(nameof(query.Status), query.Status, "Invalid workload registration status filter.");
        return (await _queryHandler.HandleAsync(query, cancellationToken)).ToList();
    });

    public Task<List<WorkloadDto>> GetWorkloadsByAvailabilityStatusAsync(GetWorkloadsByAvailabilityStatusQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        if (!Enum.IsDefined((WorkloadAvailabilityStatusFilter)query.Status)) throw new ArgumentOutOfRangeException(nameof(query.Status), query.Status, "Invalid workload availability status filter.");
        return (await _queryHandler.HandleAsync(query, cancellationToken)).ToList();
    });

    public Task<List<WorkloadDto>> GetWorkloadsByDeletionStatusAsync(GetWorkloadsByDeletionStatusQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => (await _queryHandler.HandleAsync(query, cancellationToken)).ToList());

    public Task<List<WorkloadDto>> GetDeletedWorkloadsAsync(CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => (await _queryHandler.HandleAsync(new GetWorkloadsByDeletionStatusQuery(true), cancellationToken)).ToList());

    // Internal validation
    private async Task<WorkloadDto> GetWorkloadOrThrowAsync(Guid workloadId, CancellationToken cancellationToken) =>
        await _queryHandler.HandleAsync(new GetWorkloadByIdQuery(workloadId), cancellationToken) ?? throw new WorkloadNotFoundException(workloadId);

    private async Task EnsureBlockExistsAsync(Guid blockId, CancellationToken cancellationToken)
    {
        var block = await _blockRepository.GetByIdAsync(BlockId.Create(blockId), cancellationToken);
        if (block is null || block.IsDeleted) throw new BlockNotFoundException(blockId);
    }

    private Task<T> ExecuteAsync<T>(Func<Task<T>> operation) => _exceptionService.ExecuteAsync(operation);
}
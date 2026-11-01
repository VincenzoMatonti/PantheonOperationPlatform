using Athena.Application.Common.Services;
using Athena.Application.SystemRegistry.Commands;
using Athena.Application.SystemRegistry.Dtos;
using Athena.Application.SystemRegistry.Exceptions;
using Athena.Application.SystemRegistry.Queries;

namespace Athena.Application.SystemRegistry.UseCases;

public sealed class SystemRegistryUseCaseHandler(SystemRegistryCommandHandler commandHandler, SystemRegistryQueryHandler queryHandler, ApplicationExceptionService exceptionService)
{
    private readonly SystemRegistryCommandHandler _commandHandler = commandHandler;
    private readonly SystemRegistryQueryHandler _queryHandler = queryHandler;
    private readonly ApplicationExceptionService _exceptionService = exceptionService;

    public Task<SystemDto> CreateSystemAsync(RegisterSystemCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        var existing = await _queryHandler.GetSystemByNameAsync(new GetSystemByNameQuery { Name = command.Name }, cancellationToken);
        if (existing is not null) throw new SystemNameAlreadyExistsException(command.Name);
        return await _commandHandler.CreateSystemAsync(command, cancellationToken);
    });

    public Task<SystemDto> RegisterSystemAsync(RegisterSystemCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        var system = await _queryHandler.GetSystemByNameAsync(new GetSystemByNameQuery { Name = command.Name }, cancellationToken) ?? throw new SystemNotFoundByNameException(command.Name);
        if (system.IsDeleted || !SystemRegistryQueryHandler.IsUnregistered(system)) throw new SystemNameAlreadyExistsException(command.Name);
        return await _commandHandler.RegisterExistingSystemAsync(system.Id, cancellationToken);
    });

    public Task<SystemDto> UnregisterSystemAsync(UnregisterSystemCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(() => _commandHandler.UnregisterSystemAsync(command, cancellationToken));

    public Task<SystemDto> RenameSystemAsync(RenameSystemCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        var current = await _queryHandler.GetSystemByIdAsync(new GetSystemByIdQuery { SystemId = command.SystemId }, cancellationToken) ?? throw new SystemNotFoundException(command.SystemId);
        var existing = await _queryHandler.GetSystemByNameAsync(new GetSystemByNameQuery { Name = command.NewName }, cancellationToken);
        if (existing is not null && existing.Id != current.Id) throw new SystemNameAlreadyExistsException(command.NewName);
        return await _commandHandler.RenameSystemAsync(command, cancellationToken);
    });

    public Task<SystemDto> EnableSystemAsync(EnableSystemCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(() => _commandHandler.EnableSystemAsync(command, cancellationToken));

    public Task<SystemDto> DisableSystemAsync(DisableSystemCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(() => _commandHandler.DisableSystemAsync(command, cancellationToken));

    public Task<SystemDto> MarkSystemAvailableAsync(MarkSystemAvailableCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(() => _commandHandler.MarkSystemAvailableAsync(command, cancellationToken));

    public Task<SystemDto> MarkSystemUnavailableAsync(MarkSystemUnavailableCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(() => _commandHandler.MarkSystemUnavailableAsync(command, cancellationToken));

    public Task<SystemDto> MarkSystemAvailabilityUnknownAsync(MarkSystemAvailabilityUnknownCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(() => _commandHandler.MarkSystemAvailabilityUnknownAsync(command, cancellationToken));

    public Task<SystemDto> AddSystemEndpointAsync(AddSystemEndpointCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(() => _commandHandler.AddSystemEndpointAsync(command, cancellationToken));

    public Task<SystemDto> RemoveSystemEndpointAsync(RemoveSystemEndpointCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(() => _commandHandler.RemoveSystemEndpointAsync(command, cancellationToken));

    public Task<SystemDto> AddSystemCapabilityAsync(AddSystemCapabilityCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(() => _commandHandler.AddSystemCapabilityAsync(command, cancellationToken));

    public Task<SystemDto> RemoveSystemCapabilityAsync(RemoveSystemCapabilityCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(() => _commandHandler.RemoveSystemCapabilityAsync(command, cancellationToken));

    public Task<SystemDto> DeleteSystemAsync(DeleteSystemCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(() => _commandHandler.DeleteSystemAsync(command, cancellationToken));

    public Task<SystemDto> GetSystemByIdAsync(GetSystemByIdQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _queryHandler.GetSystemByIdAsync(query, cancellationToken) ?? throw new SystemNotFoundException(query.SystemId));

    public Task<SystemDto> GetSystemByNameAsync(GetSystemByNameQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _queryHandler.GetSystemByNameAsync(query, cancellationToken) ?? throw new SystemNotFoundByNameException(query.Name));

    public Task<List<SystemDto>> GetAllSystemsAsync(CancellationToken cancellationToken = default) => ExecuteAsync(async () => (await _queryHandler.GetAllSystemsAsync(cancellationToken)).ToList());

    public Task<List<SystemDto>> GetAllSystemsAsync(GetAllSystemsQuery query, CancellationToken cancellationToken = default) => GetAllSystemsAsync(cancellationToken);

    public Task<List<SystemDto>> GetSystemsByRegistrationStatusAsync(GetSystemsByRegistrationStatusQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => (await _queryHandler.GetSystemsByRegistrationStatusAsync(query, cancellationToken)).ToList());

    public Task<List<SystemDto>> GetSystemsByAvailabilityStatusAsync(GetSystemsByAvailabilityStatusQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => (await _queryHandler.GetSystemsByAvailabilityStatusAsync(query, cancellationToken)).ToList());

    public Task<List<SystemDto>> GetSystemsByCapabilityAsync(GetSystemsByCapabilityQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => (await _queryHandler.GetSystemsByCapabilityAsync(query, cancellationToken)).ToList());

    public Task<List<SystemEndpointDto>> GetSystemEndpointsAsync(GetSystemEndpointsQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        _ = await _queryHandler.GetSystemByIdAsync(new GetSystemByIdQuery { SystemId = query.SystemId }, cancellationToken) ?? throw new SystemNotFoundException(query.SystemId);
        return (await _queryHandler.GetSystemEndpointsAsync(query, cancellationToken)).ToList();
    });

    public Task<List<SystemCapabilityDto>> GetSystemCapabilitiesAsync(GetSystemCapabilitiesQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        _ = await _queryHandler.GetSystemByIdAsync(new GetSystemByIdQuery { SystemId = query.SystemId }, cancellationToken) ?? throw new SystemNotFoundException(query.SystemId);
        return (await _queryHandler.GetSystemCapabilitiesAsync(query, cancellationToken)).ToList();
    });

    public Task<List<SystemDto>> GetSystemsByDeletionStatusAsync(GetSystemsByDeletionStatusQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => (await _queryHandler.GetSystemsByDeletionStatusAsync(query, cancellationToken)).ToList());

    public Task<List<SystemDto>> GetDeletedSystemsAsync(CancellationToken cancellationToken = default) => ExecuteAsync(async () => (await _queryHandler.GetDeletedSystemsAsync(cancellationToken)).ToList());

    public Task<List<SystemDto>> GetDeletedSystemsAsync(GetDeletedSystemsQuery query, CancellationToken cancellationToken = default) => GetDeletedSystemsAsync(cancellationToken);

    private Task<T> ExecuteAsync<T>(Func<Task<T>> operation) => _exceptionService.ExecuteAsync(operation);
}

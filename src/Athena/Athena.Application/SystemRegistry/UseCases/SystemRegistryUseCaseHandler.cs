using Athena.Application.SystemRegistry.Commands;
using Athena.Application.SystemRegistry.Dtos;
using Athena.Application.SystemRegistry.Exceptions;
using Athena.Application.SystemRegistry.Queries;

namespace Athena.Application.SystemRegistry.UseCases;

public class SystemRegistryUseCaseHandler(SystemRegistryCommandHandler systemCommandHandler, SystemRegistryQueryHandler systemQueryHandler)
{
    private readonly SystemRegistryCommandHandler _systemRegistryCommandHandler = systemCommandHandler;
    private readonly SystemRegistryQueryHandler _systemRegistryQueryHandler = systemQueryHandler;

    // =================================================================================================================
    // USE CASE COMMANDS    
    public async Task<SystemDto> CreateSystemAsync(RegisterSystemCommand command, CancellationToken cancellationToken = default)
    {
        var query = new GetSystemByNameQuery { Name = command.Name };
        var existingSystem = await _systemRegistryQueryHandler.GetSystemByNameAsync(query, cancellationToken);
        if (existingSystem is not null) throw new SystemNameAlreadyExistsException(command.Name);
        var system = await _systemRegistryCommandHandler.CreateSystemAsync(command, cancellationToken);
        return SystemRegistryCommandHandler.ConvertSystemEntitiesToDto(system);
    }

    public async Task<SystemDto> RegisterSystemAsync(RegisterSystemCommand command, CancellationToken cancellationToken = default)
    {
        var query = new GetSystemByNameQuery { Name = command.Name };
        var system = await _systemRegistryQueryHandler.GetSystemByNameAsync(query, cancellationToken) ?? throw new SystemNotFoundByNameException(command.Name);
        if (system.IsDeleted || !SystemRegistryQueryHandler.IsUnregistered(SystemRegistryQueryHandler.ConvertSystemEntitiesToDto(system)))
            throw new SystemNameAlreadyExistsException(command.Name);
        await _systemRegistryCommandHandler.RegisterExistingSystemAsync(system, cancellationToken);
        return SystemRegistryCommandHandler.ConvertSystemEntitiesToDto(system);
    }

    public async Task<SystemDto> UnregisterSystemAsync(UnregisterSystemCommand command, CancellationToken cancellationToken = default)
    {
        var query = new GetSystemByIdQuery { SystemId = command.SystemId };
        var system = await _systemRegistryQueryHandler.GetSystemByIdAsync(query, cancellationToken) ?? throw new SystemNotFoundException(command.SystemId);
        await _systemRegistryCommandHandler.UnregisterSystemAsync(system, cancellationToken);
        return SystemRegistryCommandHandler.ConvertSystemEntitiesToDto(system);
    }

    public async Task<SystemDto> RenameSystemAsync(RenameSystemCommand command, CancellationToken cancellationToken = default)
    {
        var query = new GetSystemByIdQuery { SystemId = command.SystemId };
        var system = await _systemRegistryQueryHandler.GetSystemByIdAsync(query, cancellationToken) ?? throw new SystemNotFoundException(command.SystemId);
        var existingSystem = await _systemRegistryQueryHandler.GetSystemByNameAsync(new GetSystemByNameQuery { Name = command.NewName }, cancellationToken);
        if (existingSystem is not null && existingSystem.Id.Value != system.Id.Value) throw new SystemNameAlreadyExistsException(command.NewName);
        await _systemRegistryCommandHandler.RenameSystemAsync(system, command.NewName, cancellationToken);
        return SystemRegistryCommandHandler.ConvertSystemEntitiesToDto(system);
    }

    public async Task<SystemDto> EnableSystemAsync(EnableSystemCommand command, CancellationToken cancellationToken = default)
    {
        var query = new GetSystemByIdQuery { SystemId = command.SystemId };
        var system = await _systemRegistryQueryHandler.GetSystemByIdAsync(query, cancellationToken) ?? throw new SystemNotFoundException(command.SystemId);
        await _systemRegistryCommandHandler.EnableSystemAsync(system, cancellationToken);
        return SystemRegistryCommandHandler.ConvertSystemEntitiesToDto(system);
    }

    public async Task<SystemDto> DisableSystemAsync(DisableSystemCommand command, CancellationToken cancellationToken = default)
    {
        var query = new GetSystemByIdQuery { SystemId = command.SystemId };
        var system = await _systemRegistryQueryHandler.GetSystemByIdAsync(query, cancellationToken) ?? throw new SystemNotFoundException(command.SystemId);
        await _systemRegistryCommandHandler.DisableSystemAsync(system, cancellationToken);
        return SystemRegistryCommandHandler.ConvertSystemEntitiesToDto(system);
    }

    public async Task<SystemDto> MarkSystemAvailableAsync(MarkSystemAvailableCommand command, CancellationToken cancellationToken = default)
    {
        var query = new GetSystemByIdQuery { SystemId = command.SystemId };
        var system = await _systemRegistryQueryHandler.GetSystemByIdAsync(query, cancellationToken) ?? throw new SystemNotFoundException(command.SystemId);
        await _systemRegistryCommandHandler.MarkSystemAvailableAsync(system, cancellationToken);
        return SystemRegistryCommandHandler.ConvertSystemEntitiesToDto(system);
    }

    public async Task<SystemDto> MarkSystemUnavailableAsync(MarkSystemUnavailableCommand command, CancellationToken cancellationToken = default)
    {
        var query = new GetSystemByIdQuery { SystemId = command.SystemId };
        var system = await _systemRegistryQueryHandler.GetSystemByIdAsync(query, cancellationToken) ?? throw new SystemNotFoundException(command.SystemId);
        await _systemRegistryCommandHandler.MarkSystemUnavailableAsync(system, cancellationToken);
        return SystemRegistryCommandHandler.ConvertSystemEntitiesToDto(system);
    }

    public async Task<SystemDto> MarkSystemAvailabilityUnknownAsync(MarkSystemAvailabilityUnknownCommand command, CancellationToken cancellationToken = default)
    {
        var query = new GetSystemByIdQuery { SystemId = command.SystemId };
        var system = await _systemRegistryQueryHandler.GetSystemByIdAsync(query, cancellationToken) ?? throw new SystemNotFoundException(command.SystemId);
        await _systemRegistryCommandHandler.MarkSystemAvailabilityUnknownAsync(system, cancellationToken);
        return SystemRegistryCommandHandler.ConvertSystemEntitiesToDto(system);
    }

    public async Task<SystemDto> AddSystemEndpointAsync(AddSystemEndpointCommand command, CancellationToken cancellationToken = default)
    {
        var query = new GetSystemByIdQuery { SystemId = command.SystemId };
        var system = await _systemRegistryQueryHandler.GetSystemByIdAsync(query, cancellationToken) ?? throw new SystemNotFoundException(command.SystemId);
        await _systemRegistryCommandHandler.AddSystemEndpointAsync(system, command.Name, command.BaseAddress, cancellationToken);
        return SystemRegistryCommandHandler.ConvertSystemEntitiesToDto(system);
    }

    public async Task<SystemDto> RemoveSystemEndpointAsync(RemoveSystemEndpointCommand command, CancellationToken cancellationToken = default)
    {
        var query = new GetSystemByIdQuery { SystemId = command.SystemId };
        var system = await _systemRegistryQueryHandler.GetSystemByIdAsync(query, cancellationToken) ?? throw new SystemNotFoundException(command.SystemId);
        await _systemRegistryCommandHandler.RemoveSystemEndpointAsync(system, command.EndpointName, cancellationToken);
        return SystemRegistryCommandHandler.ConvertSystemEntitiesToDto(system);
    }

    public async Task<SystemDto> AddSystemCapabilityAsync(AddSystemCapabilityCommand command, CancellationToken cancellationToken = default)
    {
        var query = new GetSystemByIdQuery { SystemId = command.SystemId };
        var system = await _systemRegistryQueryHandler.GetSystemByIdAsync(query, cancellationToken) ?? throw new SystemNotFoundException(command.SystemId);
        await _systemRegistryCommandHandler.AddSystemCapabilityAsync(system, command.Name, command.Description, cancellationToken);
        return SystemRegistryCommandHandler.ConvertSystemEntitiesToDto(system);
    }

    public async Task<SystemDto> RemoveSystemCapabilityAsync(RemoveSystemCapabilityCommand command, CancellationToken cancellationToken = default)
    {
        var query = new GetSystemByIdQuery { SystemId = command.SystemId };
        var system = await _systemRegistryQueryHandler.GetSystemByIdAsync(query, cancellationToken) ?? throw new SystemNotFoundException(command.SystemId);
        await _systemRegistryCommandHandler.RemoveSystemCapabilityAsync(system, command.CapabilityName, cancellationToken);
        return SystemRegistryCommandHandler.ConvertSystemEntitiesToDto(system);
    }

    public async Task<SystemDto> DeleteSystemAsync(DeleteSystemCommand command, CancellationToken cancellationToken = default)
    {
        var query = new GetSystemByIdQuery { SystemId = command.SystemId };
        var system = await _systemRegistryQueryHandler.GetSystemByIdAsync(query, cancellationToken) ?? throw new SystemNotFoundException(command.SystemId);
        await _systemRegistryCommandHandler.DeleteSystemAsync(system, cancellationToken);
        return SystemRegistryCommandHandler.ConvertSystemEntitiesToDto(system);
    }

    // =================================================================================================================
    // USE CASE QUERIES

    public async Task<SystemDto> GetSystemByIdAsync(GetSystemByIdQuery query, CancellationToken cancellationToken = default)
    {
        var system = await _systemRegistryQueryHandler.GetSystemByIdAsync(query, cancellationToken) ?? throw new SystemNotFoundException(query.SystemId);
        return SystemRegistryQueryHandler.ConvertSystemEntitiesToDto(system);
    }

    public async Task<SystemDto> GetSystemByNameAsync(GetSystemByNameQuery query, CancellationToken cancellationToken = default)
    {
        var system = await _systemRegistryQueryHandler.GetSystemByNameAsync(query, cancellationToken) ?? throw new SystemNotFoundByNameException(query.Name);
        return SystemRegistryQueryHandler.ConvertSystemEntitiesToDto(system);
    }

    public async Task<List<SystemDto>> GetAllSystemsAsync(CancellationToken cancellationToken = default)
    {
        var systems = await _systemRegistryQueryHandler.GetAllSystemsAsync(cancellationToken);
        return SystemRegistryQueryHandler.ConvertSystemEntitiesToDto(systems);
    }

    public async Task<List<SystemDto>> GetSystemsByRegistrationStatusAsync(GetSystemsByRegistrationStatusQuery query, CancellationToken cancellationToken = default)
    {
        var systems = await _systemRegistryQueryHandler.GetSystemsByRegistrationStatusAsync(query, cancellationToken);
        return SystemRegistryQueryHandler.ConvertSystemEntitiesToDto(systems);
    }

    public async Task<List<SystemDto>> GetSystemsByAvailabilityStatusAsync(GetSystemsByAvailabilityStatusQuery query, CancellationToken cancellationToken = default)
    {
        var systems = await _systemRegistryQueryHandler.GetSystemsByAvailabilityStatusAsync(query, cancellationToken);
        return SystemRegistryQueryHandler.ConvertSystemEntitiesToDto(systems);
    }

    public async Task<List<SystemDto>> GetSystemsByCapabilityAsync(GetSystemsByCapabilityQuery query, CancellationToken cancellationToken = default)
    {
        var systems = await _systemRegistryQueryHandler.GetSystemsByCapabilityAsync(query, cancellationToken);
        return SystemRegistryQueryHandler.ConvertSystemEntitiesToDto(systems);
    }

    public async Task<List<SystemEndpointDto>> GetSystemEndpointsAsync(GetSystemEndpointsQuery query, CancellationToken cancellationToken = default)
    {
        var systemQuery = new GetSystemByIdQuery { SystemId = query.SystemId };
        _ = await _systemRegistryQueryHandler.GetSystemByIdAsync(systemQuery, cancellationToken) ?? throw new SystemNotFoundException(query.SystemId);
        var endpoints = await _systemRegistryQueryHandler.GetSystemEndpointsAsync(query, cancellationToken);
        return SystemRegistryQueryHandler.ConvertSystemEndpointEntitiesToDto(endpoints);
    }

    public async Task<List<SystemCapabilityDto>> GetSystemCapabilitiesAsync(GetSystemCapabilitiesQuery query, CancellationToken cancellationToken = default)
    {
        var systemQuery = new GetSystemByIdQuery { SystemId = query.SystemId };
        _ = await _systemRegistryQueryHandler.GetSystemByIdAsync(systemQuery, cancellationToken) ?? throw new SystemNotFoundException(query.SystemId);
        var capabilities = await _systemRegistryQueryHandler.GetSystemCapabilitiesAsync(query, cancellationToken);
        return SystemRegistryQueryHandler.ConvertSystemCapabilityEntitiesToDto(capabilities);
    }

    public async Task<List<SystemDto>> GetSystemsByDeletionStatusAsync(GetSystemsByDeletionStatusQuery query, CancellationToken cancellationToken = default)
    {
        var systems = await _systemRegistryQueryHandler.GetSystemsByDeletionStatusAsync(query, cancellationToken);
        return SystemRegistryQueryHandler.ConvertSystemEntitiesToDto(systems);
    }

    public async Task<List<SystemDto>> GetDeletedSystemsAsync(CancellationToken cancellationToken = default)
    {
        var systems = await _systemRegistryQueryHandler.GetDeletedSystemsAsync(cancellationToken);
        return SystemRegistryQueryHandler.ConvertSystemEntitiesToDto(systems);
    }
}
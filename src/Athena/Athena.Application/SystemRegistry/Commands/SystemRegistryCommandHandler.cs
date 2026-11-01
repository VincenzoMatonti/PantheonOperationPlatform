using Athena.Application.SystemRegistry.Dtos;
using Athena.Application.SystemRegistry.Exceptions;
using Athena.Domain.System.Registry.Repositories;
using Athena.Domain.System.Registry.ValueObjects;
using RegisteredSystem = Athena.Domain.System.Registry.Entities.System;

namespace Athena.Application.SystemRegistry.Commands;

public sealed class SystemRegistryCommandHandler(ISystemRepository systemRepository)
{
    private readonly ISystemRepository _systemRepository = systemRepository;

    public async Task<SystemDto> CreateSystemAsync(RegisterSystemCommand command, CancellationToken cancellationToken = default)
    {
        var system = RegisteredSystem.Create(SystemId.New(), SystemName.Create(command.Name));
        await _systemRepository.AddAsync(system, cancellationToken);
        return SystemRegistryDtoMapper.ToDto(system);
    }

    public Task<SystemDto> RegisterExistingSystemAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        MutateAsync(systemId, system => system.Register(), cancellationToken);

    public Task<SystemDto> UnregisterSystemAsync(UnregisterSystemCommand command, CancellationToken cancellationToken = default) =>
        MutateAsync(command.SystemId, system => system.Unregister(), cancellationToken);

    public async Task<SystemDto> RenameSystemAsync(RenameSystemCommand command, CancellationToken cancellationToken = default)
    {
        var system = await GetRequiredAsync(command.SystemId, cancellationToken);
        system.Rename(SystemName.Create(command.NewName));
        await _systemRepository.UpdateAsync(system, cancellationToken);
        return SystemRegistryDtoMapper.ToDto(system);
    }

    public Task<SystemDto> EnableSystemAsync(EnableSystemCommand command, CancellationToken cancellationToken = default) =>
        MutateAsync(command.SystemId, system => system.Enable(), cancellationToken);

    public Task<SystemDto> DisableSystemAsync(DisableSystemCommand command, CancellationToken cancellationToken = default) =>
        MutateAsync(command.SystemId, system => system.Disable(), cancellationToken);

    public Task<SystemDto> MarkSystemAvailableAsync(MarkSystemAvailableCommand command, CancellationToken cancellationToken = default) =>
        MutateAsync(command.SystemId, system => system.MarkAvailable(), cancellationToken);

    public Task<SystemDto> MarkSystemUnavailableAsync(MarkSystemUnavailableCommand command, CancellationToken cancellationToken = default) =>
        MutateAsync(command.SystemId, system => system.MarkUnavailable(), cancellationToken);

    public Task<SystemDto> MarkSystemAvailabilityUnknownAsync(
        MarkSystemAvailabilityUnknownCommand command,
        CancellationToken cancellationToken = default) =>
        MutateAsync(command.SystemId, system => system.MarkAvailabilityUnknown(), cancellationToken);

    public async Task<SystemDto> AddSystemEndpointAsync(AddSystemEndpointCommand command, CancellationToken cancellationToken = default)
    {
        var system = await GetRequiredAsync(command.SystemId, cancellationToken);
        system.AddEndpoint(SystemEndpoint.Create(command.Name, command.BaseAddress));
        await _systemRepository.UpdateAsync(system, cancellationToken);
        return SystemRegistryDtoMapper.ToDto(system);
    }

    public async Task<SystemDto> RemoveSystemEndpointAsync(RemoveSystemEndpointCommand command, CancellationToken cancellationToken = default)
    {
        var system = await GetRequiredAsync(command.SystemId, cancellationToken);
        system.RemoveEndpoint(command.EndpointName);
        await _systemRepository.UpdateAsync(system, cancellationToken);
        return SystemRegistryDtoMapper.ToDto(system);
    }

    public async Task<SystemDto> AddSystemCapabilityAsync(AddSystemCapabilityCommand command, CancellationToken cancellationToken = default)
    {
        var system = await GetRequiredAsync(command.SystemId, cancellationToken);
        system.AddCapability(SystemCapability.Create(command.Name, command.Description));
        await _systemRepository.UpdateAsync(system, cancellationToken);
        return SystemRegistryDtoMapper.ToDto(system);
    }

    public async Task<SystemDto> RemoveSystemCapabilityAsync(RemoveSystemCapabilityCommand command, CancellationToken cancellationToken = default)
    {
        var system = await GetRequiredAsync(command.SystemId, cancellationToken);
        system.RemoveCapability(command.CapabilityName);
        await _systemRepository.UpdateAsync(system, cancellationToken);
        return SystemRegistryDtoMapper.ToDto(system);
    }

    public Task<SystemDto> DeleteSystemAsync(DeleteSystemCommand command, CancellationToken cancellationToken = default) =>
        MutateAsync(command.SystemId, system => system.Delete(), cancellationToken);

    private async Task<SystemDto> MutateAsync(
        Guid systemId,
        Action<RegisteredSystem> mutation,
        CancellationToken cancellationToken)
    {
        var system = await GetRequiredAsync(systemId, cancellationToken);
        mutation(system);
        await _systemRepository.UpdateAsync(system, cancellationToken);
        return SystemRegistryDtoMapper.ToDto(system);
    }

    private async Task<RegisteredSystem> GetRequiredAsync(Guid systemId, CancellationToken cancellationToken)
    {
        var id = SystemId.Create(systemId);
        return await _systemRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new SystemNotFoundException(systemId);
    }
}

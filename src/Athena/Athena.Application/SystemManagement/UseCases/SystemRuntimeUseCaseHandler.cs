using Athena.Application.Common.Dtos;
using Athena.Application.Common.Interfaces;
using Athena.Application.Common.Services;
using Athena.Application.SystemManagement.Dtos;

namespace Athena.Application.SystemManagement.UseCases;

/// <summary>
/// Coordinates system runtime-instance use cases.
/// Refreshes discovered runtime instances before accessing the registry,
/// keeping Athena's runtime-instance records synchronized with providers.
/// Does not modify system lifecycle state.
/// </summary>
public class SystemRuntimeUseCaseHandler(
    ApplicationExceptionService exceptionService,
    RuntimeApplicationService runtimeApplicationService,
    IRuntimeDiscoveryService runtimeDiscoveryService)
{
    private readonly ApplicationExceptionService _exceptionService = exceptionService;
    private readonly RuntimeApplicationService _runtimeApplicationService = runtimeApplicationService;
    private readonly IRuntimeDiscoveryService _runtimeDiscoveryService = runtimeDiscoveryService;

    public Task<IReadOnlyList<SystemRuntimeInstanceDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _exceptionService.ExecuteAsync(async () =>
        {
            await RefreshDiscoveryAsync(cancellationToken);
            return await _runtimeApplicationService.GetAllAsync(cancellationToken);
        });
    }

    public Task<SystemRuntimeInstanceDto?> GetByIdAsync(GetSystemRuntimeInstanceByIdQuery query, CancellationToken cancellationToken = default)
    {
        return _exceptionService.ExecuteAsync(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            await RefreshDiscoveryAsync(cancellationToken);
            return await _runtimeApplicationService.GetByIdAsync(query.Id, cancellationToken);
        });
    }

    public Task<IReadOnlyList<SystemRuntimeInstanceDto>> GetBySystemIdAsync(GetSystemRuntimeInstancesBySystemIdQuery query, CancellationToken cancellationToken = default)
    {
        return _exceptionService.ExecuteAsync(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            await RefreshDiscoveryAsync(cancellationToken);
            return await _runtimeApplicationService.GetBySystemIdAsync(query.SystemId, cancellationToken);
        });
    }

    public Task<IReadOnlyList<SystemRuntimeInstanceDto>> GetBySystemIdAndEnvironmentAsync(
        GetSystemRuntimeInstancesBySystemIdAndEnvironmentQuery query, CancellationToken cancellationToken = default)
    {
        return _exceptionService.ExecuteAsync(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            await RefreshDiscoveryAsync(cancellationToken);
            return await _runtimeApplicationService.GetBySystemIdAndEnvironmentAsync(query.SystemId, query.Environment, cancellationToken);
        });
    }

    public Task<IReadOnlyList<SystemRuntimeInstanceDto>> GetByRuntimeResourceNameAsync(
        GetSystemRuntimeInstancesByRuntimeResourceNameQuery query, CancellationToken cancellationToken = default)
    {
        return _exceptionService.ExecuteAsync(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            await RefreshDiscoveryAsync(cancellationToken);
            return await _runtimeApplicationService.GetByRuntimeResourceNameAsync(query.RuntimeResourceName, cancellationToken);
        });
    }

    public Task<SystemRuntimeInstanceDto> RegisterAsync(RegisterSystemRuntimeInstanceCommand command, CancellationToken cancellationToken = default)
    {
        return _exceptionService.ExecuteAsync(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested(); await RefreshDiscoveryAsync(cancellationToken);
            return await _runtimeApplicationService.RegisterAsync(
                command.SystemId, command.RuntimeProvider, command.RuntimeResourceId,
                command.RuntimeResourceName, command.Environment, cancellationToken);
        });
    }

    public Task<bool> UnregisterAsync(UnregisterSystemRuntimeInstanceCommand command, CancellationToken cancellationToken = default)
    {
        return _exceptionService.ExecuteAsync(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            await RefreshDiscoveryAsync(cancellationToken);
            return await _runtimeApplicationService.UnregisterAsync(command.Id, cancellationToken);
        });
    }

    private Task RefreshDiscoveryAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return _runtimeDiscoveryService.DiscoverAndSynchronizeAsync(cancellationToken);
    }
}

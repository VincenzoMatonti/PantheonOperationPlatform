using Athena.Application.EcosystemManagement.Contracts;
using Athena.Application.EcosystemManagement.Exceptions;
using Athena.Domain.Ecosystem.Management.Entities;
using Athena.Domain.Ecosystem.Management.Enums;
using Athena.Domain.Ecosystem.Management.Repositories;
using Athena.Domain.Ecosystem.Registry.Exceptions;
using Athena.Domain.Ecosystem.Registry.Repositories;
using Athena.Domain.Ecosystem.Registry.ValueObjects;

namespace Athena.Application.EcosystemManagement.Commands;

public sealed class EcosystemManagementCommandHandler(
    IEcosystemRepository ecosystemRepository,
    IEcosystemLifecycleRepository lifecycleRepository,
    IEcosystemOperationRepository operationRepository)
{
    public async Task<EcosystemLifecycleDto> CreateLifecycleAsync(CreateEcosystemLifecycleCommand command, CancellationToken token)
    {
        var id = EcosystemId.Create(command.EcosystemId);
        _ = await GetEcosystemAsync(id, token);
        if (await lifecycleRepository.GetByIdAsync(id, token) is not null)
            throw Error(EcosystemManagementErrorCode.EcosystemLifecycleAlreadyExists, $"Lifecycle for ecosystem '{id.Value}' already exists.");
        var lifecycle = EcosystemLifecycle.Create(id);
        await lifecycleRepository.AddAsync(lifecycle, token);
        return EcosystemManagementDtoMapper.ToDto(lifecycle);
    }

    public async Task<EcosystemLifecycleDto> SetDesiredStateAsync(SetEcosystemDesiredStateCommand command, CancellationToken token)
    {
        if (!Enum.TryParse<EcosystemDesiredState>(command.DesiredState, true, out var desired) || !Enum.IsDefined(desired))
            throw Error(EcosystemManagementErrorCode.InvalidDesiredState, $"Desired state '{command.DesiredState}' is invalid.");
        var id = EcosystemId.Create(command.EcosystemId);
        EnsureRegistered(await GetEcosystemAsync(id, token));
        var lifecycle = await GetRequiredLifecycleAsync(id, token);
        lifecycle.SetDesiredState(desired);
        lifecycle.SetOperationalStatus(EcosystemOperationalStatus.Unknown);
        await lifecycleRepository.UpdateAsync(lifecycle, token);
        return EcosystemManagementDtoMapper.ToDto(lifecycle);
    }

    public async Task<EcosystemOperationDto> ExecuteOperationAsync(ExecuteEcosystemOperationCommand command, CancellationToken token)
    {
        if (!Enum.TryParse<EcosystemOperationType>(command.OperationType, true, out var type) || !Enum.IsDefined(type))
            throw Error(EcosystemManagementErrorCode.InvalidOperationType, $"Operation type '{command.OperationType}' is invalid.");
        var id = EcosystemId.Create(command.EcosystemId);
        EnsureRegistered(await GetEcosystemAsync(id, token));
        var lifecycle = await GetRequiredLifecycleAsync(id, token);
        var operation = EcosystemOperation.Create(Guid.NewGuid(), id, type);
        lifecycle.SetDesiredState(type switch
        {
            EcosystemOperationType.Start or EcosystemOperationType.Restart => EcosystemDesiredState.Running,
            EcosystemOperationType.Stop => EcosystemDesiredState.Stopped,
            _ => throw Error(EcosystemManagementErrorCode.InvalidOperationType, $"Operation type '{command.OperationType}' is invalid.")
        });
        lifecycle.SetOperationalStatus(EcosystemOperationalStatus.Unknown);
        operation.Start();
        operation.Complete();
        await operationRepository.SaveIntentAsync(operation, lifecycle, token);
        return EcosystemManagementDtoMapper.ToDto(operation);
    }

    private async Task<Athena.Domain.Ecosystem.Registry.Entities.Ecosystem> GetEcosystemAsync(EcosystemId id, CancellationToken token) =>
        await ecosystemRepository.GetByIdAsync(id, token) ?? throw new EcosystemNotFoundException(id.Value);

    private static void EnsureRegistered(Athena.Domain.Ecosystem.Registry.Entities.Ecosystem ecosystem)
    {
        if (ecosystem.RegistrationStatus == Athena.Domain.Ecosystem.Registry.Enums.EcosystemRegistrationStatus.Disabled)
            throw new EcosystemAlreadyDisabledException(ecosystem.Id.Value);
        if (ecosystem.RegistrationStatus == Athena.Domain.Ecosystem.Registry.Enums.EcosystemRegistrationStatus.Unregistered)
            throw new EcosystemNotRegisteredException(ecosystem.Id.Value);
    }

    private async Task<EcosystemLifecycle> GetRequiredLifecycleAsync(EcosystemId id, CancellationToken token) =>
        await lifecycleRepository.GetByIdAsync(id, token)
        ?? throw Error(EcosystemManagementErrorCode.EcosystemLifecycleNotFound, $"Lifecycle for ecosystem '{id.Value}' was not found.");

    private static EcosystemManagementException Error(EcosystemManagementErrorCode code, string message) => new(code, message);

}

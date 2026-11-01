using Athena.Application.Common.Services;
using Athena.Application.EcosystemManagement.Contracts;
using Athena.Application.EcosystemManagement.Exceptions;
using Athena.Domain.Ecosystem.Management.Entities;
using Athena.Domain.Ecosystem.Management.Enums;
using Athena.Domain.Ecosystem.Management.Repositories;
using Athena.Domain.Ecosystem.Registry.Exceptions;
using Athena.Domain.Ecosystem.Registry.Repositories;
using Athena.Domain.Ecosystem.Registry.ValueObjects;

namespace Athena.Application.EcosystemManagement.UseCases;

public sealed class EcosystemManagementUseCaseHandler(
    IEcosystemRepository ecosystemRepository,
    IEcosystemLifecycleRepository lifecycleRepository,
    IEcosystemOperationRepository operationRepository,
    ApplicationExceptionService exceptionService) : IEcosystemManagement
{
    public Task<EcosystemLifecycleDto> CreateLifecycleAsync(Guid id, CancellationToken token = default) =>
        RunAsync(async () =>
        {
            var ecosystemId = EcosystemId.Create(id);
            await GetEcosystemAsync(ecosystemId, token);
            if (await lifecycleRepository.GetByIdAsync(ecosystemId, token) is not null)
                throw Error(EcosystemManagementErrorCode.EcosystemLifecycleAlreadyExists, $"Lifecycle for ecosystem '{id}' already exists.");
            var lifecycle = EcosystemLifecycle.Create(ecosystemId);
            await lifecycleRepository.AddAsync(lifecycle, token);
            return ToDto(lifecycle);
        });
    public Task<EcosystemLifecycleDto?> GetLifecycleAsync(Guid id, CancellationToken token = default) =>
        RunAsync(async () =>
        {
            var ecosystemId = EcosystemId.Create(id);
            await GetEcosystemAsync(ecosystemId, token);
            var lifecycle = await lifecycleRepository.GetByIdAsync(ecosystemId, token);
            return lifecycle is null ? null : ToDto(lifecycle);
        });
    public Task<IReadOnlyList<EcosystemLifecycleDto>> GetLifecyclesAsync(CancellationToken token = default) =>
        RunAsync<IReadOnlyList<EcosystemLifecycleDto>>(async () => (await lifecycleRepository.GetAllAsync(token)).Select(ToDto).ToArray());
    public Task<EcosystemLifecycleDto> SetDesiredStateAsync(Guid id, string state, CancellationToken token = default) =>
        RunAsync(async () =>
        {
            if (!Enum.TryParse<EcosystemDesiredState>(state, true, out var desired) || !Enum.IsDefined(desired))
                throw Error(EcosystemManagementErrorCode.InvalidDesiredState, $"Desired state '{state}' is invalid.");
            var ecosystemId = EcosystemId.Create(id);
            EnsureRegistered(await GetEcosystemAsync(ecosystemId, token));
            var lifecycle = await GetRequiredLifecycleAsync(ecosystemId, token);
            lifecycle.SetDesiredState(desired);
            lifecycle.SetOperationalStatus(EcosystemOperationalStatus.Unknown);
            await lifecycleRepository.UpdateAsync(lifecycle, token);
            return ToDto(lifecycle);
        });
    public Task<EcosystemOperationDto> ExecuteOperationAsync(Guid id, string type, CancellationToken token = default) =>
        RunAsync(async () =>
        {
            if (!Enum.TryParse<EcosystemOperationType>(type, true, out var operationType) || !Enum.IsDefined(operationType))
                throw Error(EcosystemManagementErrorCode.InvalidOperationType, $"Operation type '{type}' is invalid.");
            var ecosystemId = EcosystemId.Create(id);
            EnsureRegistered(await GetEcosystemAsync(ecosystemId, token));
            var lifecycle = await GetRequiredLifecycleAsync(ecosystemId, token);
            var operation = EcosystemOperation.Create(Guid.NewGuid(), ecosystemId, operationType);
            var desired = operationType switch
            {
                EcosystemOperationType.Start or EcosystemOperationType.Restart => EcosystemDesiredState.Running,
                EcosystemOperationType.Stop => EcosystemDesiredState.Stopped,
                _ => throw Error(EcosystemManagementErrorCode.InvalidOperationType, $"Operation type '{type}' is invalid.")
            };
            lifecycle.SetDesiredState(desired);
            lifecycle.SetOperationalStatus(EcosystemOperationalStatus.Unknown);
            operation.Start();
            operation.Complete();
            await operationRepository.SaveIntentAsync(operation, lifecycle, token);
            return ToDto(operation);
        });
    public Task<IReadOnlyList<EcosystemOperationDto>> GetOperationsAsync(Guid id, CancellationToken token = default) =>
        RunAsync<IReadOnlyList<EcosystemOperationDto>>(async () =>
        {
            var ecosystemId = EcosystemId.Create(id);
            await GetEcosystemAsync(ecosystemId, token);
            return (await operationRepository.GetByEcosystemIdAsync(ecosystemId, token)).Select(ToDto).ToArray();
        });
    private async Task<Athena.Domain.Ecosystem.Registry.Entities.Ecosystem> GetEcosystemAsync(EcosystemId id, CancellationToken token) =>
        await ecosystemRepository.GetByIdAsync(id, token) ?? throw new EcosystemNotFoundException(id.Value);
    private static void EnsureRegistered(Athena.Domain.Ecosystem.Registry.Entities.Ecosystem ecosystem)
    {
        if (ecosystem.RegistrationStatus == Athena.Domain.Ecosystem.Registry.Enums.EcosystemRegistrationStatus.Disabled)
            throw new Athena.Domain.Ecosystem.Registry.Exceptions.EcosystemAlreadyDisabledException(ecosystem.Id.Value);
        if (ecosystem.RegistrationStatus == Athena.Domain.Ecosystem.Registry.Enums.EcosystemRegistrationStatus.Unregistered)
            throw new EcosystemNotRegisteredException(ecosystem.Id.Value);
    }
    private async Task<EcosystemLifecycle> GetRequiredLifecycleAsync(EcosystemId id, CancellationToken token) =>
        await lifecycleRepository.GetByIdAsync(id, token) ?? throw Error(EcosystemManagementErrorCode.EcosystemLifecycleNotFound, $"Lifecycle for ecosystem '{id.Value}' was not found.");
    private Task<T> RunAsync<T>(Func<Task<T>> operation) => exceptionService.ExecuteAsync(operation);
    private static EcosystemManagementException Error(EcosystemManagementErrorCode code, string message) => new(code, message);
    private static EcosystemLifecycleDto ToDto(EcosystemLifecycle lifecycle) =>
        new(lifecycle.Id.Value, lifecycle.DesiredState.ToString(), lifecycle.OperationalStatus.ToString(), lifecycle.CreatedAtUtc, lifecycle.UpdatedAtUtc, lifecycle.IsDeleted);
    private static EcosystemOperationDto ToDto(EcosystemOperation operation) =>
        new(operation.Id, operation.EcosystemId.Value, operation.Type.ToString(), operation.Status.ToString(),
            operation.CreatedAtUtc, operation.StartedAtUtc, operation.CompletedAtUtc, operation.ErrorMessage, operation.IsDeleted);
}

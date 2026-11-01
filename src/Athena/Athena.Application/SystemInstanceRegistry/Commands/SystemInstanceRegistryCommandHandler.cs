using Athena.Application.SystemInstanceRegistry.Contracts;
using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Registry.Entities;
using Athena.Domain.System.Registry.Repositories;
using Athena.Domain.System.Registry.ValueObjects;
using Athena.Domain.Workload.Registry.ValueObjects;

namespace Athena.Application.SystemInstanceRegistry.Commands;

public sealed class SystemInstanceRegistryCommandHandler(ISystemInstanceRepository repository)
{
    private readonly ISystemInstanceRepository _repository = repository;

    public async Task<SystemInstanceRegistryDto> CreateAsync(CreateSystemInstanceCommand command, CancellationToken cancellationToken = default)
    {
        var id = SystemInstanceId.Create(command.SystemInstanceId);
        var systemId = SystemId.Create(command.SystemId);
        var workloadId = WorkloadId.Create(command.WorkloadId);
        var name = SystemInstanceName.Create(command.Name);
        var instance = SystemInstance.Create(id, systemId, workloadId, name);
        var lifecycle = SystemInstanceLifecycle.Create(id);
        await _repository.AddAsync(instance, lifecycle, cancellationToken);
        return ToDto(instance);
    }

    public async Task<SystemInstanceRegistryDto?> RenameAsync(RenameSystemInstanceCommand command, CancellationToken cancellationToken = default)
    {
        var id = SystemInstanceId.Create(command.SystemInstanceId);
        var instance = await _repository.GetByIdAsync(id, cancellationToken);
        if (instance is null) return null;
        var name = SystemInstanceName.Create(command.Name);
        instance.Rename(name);
        await _repository.UpdateAsync(instance, cancellationToken);
        return ToDto(instance);
    }

    public Task<SystemInstanceRegistryDto?> RegisterAsync(RegisterSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
    UpdateAsync(command.SystemInstanceId, instance => instance.Register(), cancellationToken);

    public Task<SystemInstanceRegistryDto?> UnregisterAsync(UnregisterSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
    UpdateAsync(command.SystemInstanceId, instance => instance.Unregister(), cancellationToken);

    public Task<SystemInstanceRegistryDto?> EnableAsync(EnableSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
    UpdateAsync(command.SystemInstanceId, instance => instance.Enable(), cancellationToken);

    public Task<SystemInstanceRegistryDto?> DisableAsync(DisableSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
    UpdateAsync(command.SystemInstanceId, instance => instance.Disable(), cancellationToken);

    public Task<SystemInstanceRegistryDto?> DeleteAsync(DeleteSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
    UpdateAsync(command.SystemInstanceId, instance => instance.Delete(), cancellationToken);

    private async Task<SystemInstanceRegistryDto?> UpdateAsync(Guid systemInstanceId, Action<SystemInstance> operation, CancellationToken cancellationToken)
    {
        var id = SystemInstanceId.Create(systemInstanceId);
        var instance = await _repository.GetByIdAsync(id, cancellationToken);
        if (instance is null) return null;
        operation(instance);
        await _repository.UpdateAsync(instance, cancellationToken);
        return ToDto(instance);
    }

    private static SystemInstanceRegistryDto ToDto(SystemInstance instance)
        => new(
            instance.Id.Value,
            instance.SystemId.Value,
            instance.WorkloadId.Value,
            instance.Name.Value,
            instance.RegistrationStatus.ToString(),
            instance.CreatedAtUtc,
            instance.UpdatedAtUtc,
            instance.IsDeleted);
}
using Athena.Application.SystemInstanceRegistry.Contracts;
using Athena.Domain.System.Registry.Entities;
using Athena.Domain.System.Registry.Repositories;
using Athena.Domain.System.Registry.ValueObjects;
using Athena.Domain.Workload.Registry.ValueObjects;

namespace Athena.Application.SystemInstanceRegistry.Queries;

public sealed class SystemInstanceRegistryQueryHandler(ISystemInstanceRepository repository)
{
    private readonly ISystemInstanceRepository _repository = repository;

    public async Task<IReadOnlyList<SystemInstanceRegistryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var instances = await _repository.GetAllAsync(cancellationToken);
        return instances.Select(ToDto).ToArray();
    }

    public async Task<SystemInstanceRegistryDto?> GetByIdAsync(GetSystemInstanceByIdQuery query, CancellationToken cancellationToken = default)
    {
        var id = SystemInstanceId.Create(query.SystemInstanceId);
        var instance = await _repository.GetByIdAsync(id, cancellationToken);
        return instance is null ? null : ToDto(instance);
    }

    public async Task<SystemInstanceRegistryDto?> GetByNameAsync(GetSystemInstanceByNameQuery query, CancellationToken cancellationToken = default)
    {
        var workloadId = WorkloadId.Create(query.WorkloadId);
        var name = SystemInstanceName.Create(query.Name);
        var instance = await _repository.GetByNameAsync(workloadId, name, cancellationToken);
        return instance is null ? null : ToDto(instance);
    }

    public async Task<IReadOnlyList<SystemInstanceRegistryDto>> GetBySystemIdAsync(GetSystemInstancesBySystemIdQuery query, CancellationToken cancellationToken = default)
    {
        var systemId = SystemId.Create(query.SystemId);
        var instances = await _repository.GetBySystemIdAsync(systemId, cancellationToken);
        return instances.Select(ToDto).ToArray();
    }

    public async Task<IReadOnlyList<SystemInstanceRegistryDto>> GetByWorkloadIdAsync(GetSystemInstancesByWorkloadIdQuery query, CancellationToken cancellationToken = default)
    {
        var workloadId = WorkloadId.Create(query.WorkloadId);
        var instances = await _repository.GetByWorkloadIdAsync(workloadId, cancellationToken);
        return instances.Select(ToDto).ToArray();
    }

    public async Task<IReadOnlyList<SystemInstanceRegistryDto>> GetByRegistrationStatusAsync(GetSystemInstancesByRegistrationStatusQuery query, CancellationToken cancellationToken = default)
    {
        var instances = await _repository.GetByRegistrationStatusAsync(query.Status, cancellationToken);
        return instances.Select(ToDto).ToArray();
    }

    public async Task<IReadOnlyList<SystemInstanceRegistryDto>> GetByDeletionStatusAsync(GetSystemInstancesByDeletionStatusQuery query, CancellationToken cancellationToken = default)
    {
        var instances = await _repository.GetByDeletionStatusAsync(query.IsDeleted, cancellationToken);
        return instances.Select(ToDto).ToArray();
    }

    public Task<bool> ExistsByIdAsync(SystemInstanceExistsByIdQuery query, CancellationToken cancellationToken = default) =>
        _repository.ExistsByIdAsync(SystemInstanceId.Create(query.SystemInstanceId), cancellationToken);

    public Task<bool> ExistsByNameAsync(SystemInstanceExistsByNameQuery query, CancellationToken cancellationToken = default) =>
        _repository.ExistsByNameAsync(WorkloadId.Create(query.WorkloadId), SystemInstanceName.Create(query.Name), cancellationToken);

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
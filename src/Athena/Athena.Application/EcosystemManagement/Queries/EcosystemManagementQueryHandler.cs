using Athena.Application.EcosystemManagement.Contracts;
using Athena.Domain.Ecosystem.Management.Repositories;
using Athena.Domain.Ecosystem.Registry.Exceptions;
using Athena.Domain.Ecosystem.Registry.Repositories;
using Athena.Domain.Ecosystem.Registry.ValueObjects;

namespace Athena.Application.EcosystemManagement.Queries;

public sealed class EcosystemManagementQueryHandler(
    IEcosystemRepository ecosystemRepository,
    IEcosystemLifecycleRepository lifecycleRepository,
    IEcosystemOperationRepository operationRepository)
{
    public async Task<EcosystemLifecycleDto?> GetLifecycleAsync(GetEcosystemLifecycleQuery query, CancellationToken token)
    {
        var id = EcosystemId.Create(query.EcosystemId);
        _ = await ecosystemRepository.GetByIdAsync(id, token) ?? throw new EcosystemNotFoundException(id.Value);
        var lifecycle = await lifecycleRepository.GetByIdAsync(id, token);
        return lifecycle is null ? null : EcosystemManagementDtoMapper.ToDto(lifecycle);
    }

    public async Task<IReadOnlyList<EcosystemLifecycleDto>> GetLifecyclesAsync(GetEcosystemLifecyclesQuery query, CancellationToken token) =>
        (await lifecycleRepository.GetAllAsync(token)).Select(EcosystemManagementDtoMapper.ToDto).ToArray();

    public async Task<IReadOnlyList<EcosystemOperationDto>> GetOperationsAsync(GetEcosystemOperationsQuery query, CancellationToken token)
    {
        var id = EcosystemId.Create(query.EcosystemId);
        _ = await ecosystemRepository.GetByIdAsync(id, token) ?? throw new EcosystemNotFoundException(id.Value);
        return (await operationRepository.GetByEcosystemIdAsync(id, token)).Select(EcosystemManagementDtoMapper.ToDto).ToArray();
    }
}

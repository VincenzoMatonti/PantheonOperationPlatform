using Athena.Application.EcosystemRegistry.Contracts;
using Athena.Domain.Company.ValueObjects;
using Athena.Domain.Ecosystem.Management.Entities;
using Athena.Domain.Ecosystem.Registry.Entities;
using Athena.Domain.Ecosystem.Registry.Repositories;
using Athena.Domain.Ecosystem.Registry.ValueObjects;

namespace Athena.Application.EcosystemRegistry.Commands;

public sealed class EcosystemRegistryCommandHandler(IEcosystemRepository ecosystemRepository)
{
    private readonly IEcosystemRepository _ecosystemRepository = ecosystemRepository;

    public async Task<EcosystemDto> CreateAsync(CreateEcosystemCommand command, CancellationToken cancellationToken = default)
    {
        var ecosystemId = EcosystemId.New();
        var ecosystem = Ecosystem.Create(ecosystemId, CompanyId.Create(command.CompanyId), EcosystemName.Create(command.Name));
        var lifecycle = EcosystemLifecycle.Create(ecosystemId);
        await _ecosystemRepository.AddAsync(ecosystem, lifecycle, cancellationToken);
        return EcosystemRegistryDtoMapper.ToDto(ecosystem);
    }

    public Task<EcosystemDto?> RegisterAsync(RegisterEcosystemCommand command, CancellationToken cancellationToken = default) =>
    MutateAsync(command.EcosystemId, ecosystem => ecosystem.Register(), cancellationToken);

    public Task<EcosystemDto?> UnregisterAsync(UnregisterEcosystemCommand command, CancellationToken cancellationToken = default) =>
    MutateAsync(command.EcosystemId, ecosystem => ecosystem.Unregister(), cancellationToken);

    public async Task<EcosystemDto?> RenameAsync(RenameEcosystemCommand command, CancellationToken cancellationToken = default)
    {
        var ecosystem = await GetEcosystemAsync(command.EcosystemId, cancellationToken);
        if (ecosystem is null) return null;
        ecosystem.Rename(EcosystemName.Create(command.NewName));
        await _ecosystemRepository.UpdateAsync(ecosystem, cancellationToken);
        return EcosystemRegistryDtoMapper.ToDto(ecosystem);
    }

    public Task<EcosystemDto?> EnableAsync(EnableEcosystemCommand command, CancellationToken cancellationToken = default) =>
    MutateAsync(command.EcosystemId, ecosystem => ecosystem.Enable(), cancellationToken);

    public Task<EcosystemDto?> DisableAsync(DisableEcosystemCommand command, CancellationToken cancellationToken = default) =>
    MutateAsync(command.EcosystemId, ecosystem => ecosystem.Disable(), cancellationToken);

    public Task<EcosystemDto?> DeleteAsync(DeleteEcosystemCommand command, CancellationToken cancellationToken = default) =>
    MutateAsync(command.EcosystemId, ecosystem => ecosystem.Delete(), cancellationToken);

    public Task<EcosystemDto?> MarkAvailableAsync(MarkEcosystemAvailableCommand command, CancellationToken cancellationToken = default) =>
    MutateAsync(command.EcosystemId, ecosystem => ecosystem.MarkAvailable(), cancellationToken);

    public Task<EcosystemDto?> MarkUnavailableAsync(MarkEcosystemUnavailableCommand command, CancellationToken cancellationToken = default) =>
    MutateAsync(command.EcosystemId, ecosystem => ecosystem.MarkUnavailable(), cancellationToken);

    public Task<EcosystemDto?> MarkAvailabilityUnknownAsync(MarkEcosystemAvailabilityUnknownCommand command, CancellationToken cancellationToken = default) =>
    MutateAsync(command.EcosystemId, ecosystem => ecosystem.MarkAvailabilityUnknown(), cancellationToken);

    public Task<EcosystemDto?> MarkErrorAsync(MarkEcosystemErrorCommand command, CancellationToken cancellationToken = default) =>
    MutateAsync(command.EcosystemId, ecosystem => ecosystem.MarkError(), cancellationToken);

    private async Task<EcosystemDto?> MutateAsync(Guid ecosystemId, Action<Ecosystem> mutation, CancellationToken cancellationToken)
    {
        var ecosystem = await GetEcosystemAsync(ecosystemId, cancellationToken);
        if (ecosystem is null) return null;
        mutation(ecosystem);
        await _ecosystemRepository.UpdateAsync(ecosystem, cancellationToken);
        return EcosystemRegistryDtoMapper.ToDto(ecosystem);
    }

    private Task<Ecosystem?> GetEcosystemAsync(Guid ecosystemId, CancellationToken cancellationToken)
    {
        var id = EcosystemId.Create(ecosystemId);
        return _ecosystemRepository.GetByIdAsync(id, cancellationToken);
    }
}
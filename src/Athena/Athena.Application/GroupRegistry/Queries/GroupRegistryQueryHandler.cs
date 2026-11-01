using Athena.Application.GroupRegistry.Contracts;
using Athena.Application.GroupRegistry.Enums;
using Athena.Domain.Company.Enums;
using Athena.Domain.Company.Repositories;
using Athena.Domain.Company.ValueObjects;

namespace Athena.Application.GroupRegistry.Queries;

public sealed class GroupRegistryQueryHandler(IGroupRepository groupRepository)
{
    private readonly IGroupRepository _groupRepository = groupRepository;

    public async Task<GroupDto?> GetGroupByIdAsync(GetGroupByIdQuery query, CancellationToken cancellationToken = default)
    {
        var group = await _groupRepository.GetByIdAsync(GroupId.Create(query.GroupId), cancellationToken);
        return group is null ? null : GroupRegistryDtoMapper.ToDto(group);
    }

    public async Task<GroupDto?> GetGroupByNameAsync(GetGroupByNameQuery query, CancellationToken cancellationToken = default)
    {
        var group = await _groupRepository.GetByNameAsync(GroupName.Create(query.Name), cancellationToken);
        return group is null ? null : GroupRegistryDtoMapper.ToDto(group);
    }

    public async Task<IReadOnlyList<GroupDto>> GetAllGroupsAsync(CancellationToken cancellationToken = default)
    {
        var groups = await _groupRepository.GetAllAsync(cancellationToken);
        return GroupRegistryDtoMapper.ToDtos(groups);
    }

    public async Task<IReadOnlyList<GroupDto>> GetGroupsByRegistrationStatusAsync(GetGroupsByRegistrationStatusQuery query, CancellationToken cancellationToken = default)
    {
        var status = query.Status switch
        {
            GroupRegistrationStatusFilter.Registered => GroupRegistrationStatus.Registered,
            GroupRegistrationStatusFilter.Unregistered => GroupRegistrationStatus.Unregistered,
            GroupRegistrationStatusFilter.Disabled => GroupRegistrationStatus.Disabled,
            _ => (GroupRegistrationStatus?)null
        };

        if (status is null) return [];
        var groups = await _groupRepository.GetByRegistrationStatusAsync(status.Value, cancellationToken);
        return GroupRegistryDtoMapper.ToDtos(groups);
    }

    public async Task<IReadOnlyList<GroupDto>> GetGroupsByDeletionStatusAsync(GetGroupsByDeletionStatusQuery query, CancellationToken cancellationToken = default)
    {
        var groups = await _groupRepository.GetByDeletionStatusAsync(query.IsDeleted, cancellationToken);
        return GroupRegistryDtoMapper.ToDtos(groups);
    }

    public async Task<IReadOnlyList<GroupDto>> GetDeletedGroupsAsync(CancellationToken cancellationToken = default)
    {
        var groups = await _groupRepository.GetByDeletionStatusAsync(true, cancellationToken);
        return GroupRegistryDtoMapper.ToDtos(groups);
    }

    public Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default) => _groupRepository.ExistsByNameAsync(GroupName.Create(name), cancellationToken);
}
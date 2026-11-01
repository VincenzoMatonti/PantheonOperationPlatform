using Athena.Application.GroupRegistry.Contracts;
using Athena.Domain.Company.Entities;
using Athena.Domain.Company.Repositories;
using Athena.Domain.Company.ValueObjects;

namespace Athena.Application.GroupRegistry.Commands;

public sealed class GroupRegistryCommandHandler(IGroupRepository groupRepository)
{
    private readonly IGroupRepository _groupRepository = groupRepository;

    public async Task<GroupDto> CreateGroupAsync(CreateGroupCommand command, CancellationToken cancellationToken = default)
    {
        var group = Group.Create(GroupId.New(), GroupName.Create(command.Name));
        await _groupRepository.AddAsync(group, cancellationToken);
        return GroupRegistryDtoMapper.ToDto(group);
    }

    public Task<GroupDto?> RegisterExistingGroupAsync(Guid groupId, CancellationToken cancellationToken = default) => MutateAsync(groupId, group => group.Register(), cancellationToken);

    public Task<GroupDto?> UnregisterGroupAsync(UnregisterGroupCommand command, CancellationToken cancellationToken = default) => MutateAsync(command.GroupId, group => group.Unregister(), cancellationToken);

    public async Task<GroupDto?> RenameGroupAsync(RenameGroupCommand command, CancellationToken cancellationToken = default)
    {
        var group = await GetGroupAsync(command.GroupId, cancellationToken);
        if (group is null) return null;
        group.Rename(GroupName.Create(command.NewName));
        await _groupRepository.UpdateAsync(group, cancellationToken);
        return GroupRegistryDtoMapper.ToDto(group);
    }

    public Task<GroupDto?> EnableGroupAsync(EnableGroupCommand command, CancellationToken cancellationToken = default) => MutateAsync(command.GroupId, group => group.Enable(), cancellationToken);

    public Task<GroupDto?> DisableGroupAsync(DisableGroupCommand command, CancellationToken cancellationToken = default) => MutateAsync(command.GroupId, group => group.Disable(), cancellationToken);

    public Task<GroupDto?> DeleteGroupAsync(DeleteGroupCommand command, CancellationToken cancellationToken = default) => MutateAsync(command.GroupId, group => group.Delete(), cancellationToken);

    private async Task<GroupDto?> MutateAsync(Guid groupId, Action<Group> mutation, CancellationToken cancellationToken)
    {
        var group = await GetGroupAsync(groupId, cancellationToken);
        if (group is null) return null;
        mutation(group);
        await _groupRepository.UpdateAsync(group, cancellationToken);
        return GroupRegistryDtoMapper.ToDto(group);
    }

    private async Task<Group?> GetGroupAsync(Guid groupId, CancellationToken cancellationToken)
    {
        var id = GroupId.Create(groupId);
        return await _groupRepository.GetByIdAsync(id, cancellationToken);
    }
}
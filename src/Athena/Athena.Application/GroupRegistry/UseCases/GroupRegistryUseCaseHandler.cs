using Athena.Application.Common.Services;
using Athena.Application.GroupRegistry.Commands;
using Athena.Application.GroupRegistry.Contracts;
using Athena.Application.GroupRegistry.Exceptions;
using Athena.Application.GroupRegistry.Queries;

namespace Athena.Application.GroupRegistry.UseCases;

public sealed class GroupRegistryUseCaseHandler(GroupRegistryCommandHandler commandHandler, GroupRegistryQueryHandler queryHandler, ApplicationExceptionService exceptionService)
{
    private readonly GroupRegistryCommandHandler _commandHandler = commandHandler;
    private readonly GroupRegistryQueryHandler _queryHandler = queryHandler;
    private readonly ApplicationExceptionService _exceptionService = exceptionService;

    public Task<GroupDto> CreateGroupAsync(CreateGroupCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        var exists = await _queryHandler.GetGroupByNameAsync(new GetGroupByNameQuery { Name = command.Name }, cancellationToken);
        if (exists is not null || await NameExistsAsync(command.Name, cancellationToken)) throw new GroupNameAlreadyExistsException(command.Name);
        return await _commandHandler.CreateGroupAsync(command, cancellationToken);
    });

    public Task<GroupDto> RegisterGroupAsync(RegisterGroupCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        var group = await _queryHandler.GetGroupByNameAsync(new GetGroupByNameQuery { Name = command.Name }, cancellationToken) ?? throw new GroupNotFoundByNameException(command.Name);
        if (group.IsDeleted || group.RegistrationStatus != "Unregistered") throw new GroupNameAlreadyExistsException(command.Name);
        return await _commandHandler.RegisterExistingGroupAsync(group.Id, cancellationToken) ?? throw new GroupNotFoundException(group.Id);
    });

    public Task<GroupDto> UnregisterGroupAsync(UnregisterGroupCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.UnregisterGroupAsync(command, cancellationToken) ?? throw new GroupNotFoundException(command.GroupId));

    public Task<GroupDto> RenameGroupAsync(RenameGroupCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        var current = await _queryHandler.GetGroupByIdAsync(new GetGroupByIdQuery { GroupId = command.GroupId }, cancellationToken) ?? throw new GroupNotFoundException(command.GroupId);
        if (await NameExistsAsync(command.NewName, cancellationToken))
        {
            var existing = await _queryHandler.GetGroupByNameAsync(new GetGroupByNameQuery { Name = command.NewName }, cancellationToken);
            if (existing is null || existing.Id != current.Id) throw new GroupNameAlreadyExistsException(command.NewName);
        }

        return await _commandHandler.RenameGroupAsync(command, cancellationToken) ?? throw new GroupNotFoundException(command.GroupId);
    });

    public Task<GroupDto> EnableGroupAsync(EnableGroupCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.EnableGroupAsync(command, cancellationToken) ?? throw new GroupNotFoundException(command.GroupId));

    public Task<GroupDto> DisableGroupAsync(DisableGroupCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.DisableGroupAsync(command, cancellationToken) ?? throw new GroupNotFoundException(command.GroupId));

    public Task<GroupDto> DeleteGroupAsync(DeleteGroupCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.DeleteGroupAsync(command, cancellationToken) ?? throw new GroupNotFoundException(command.GroupId));

    public Task<GroupDto> GetGroupByIdAsync(GetGroupByIdQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _queryHandler.GetGroupByIdAsync(query, cancellationToken) ?? throw new GroupNotFoundException(query.GroupId));

    public Task<GroupDto> GetGroupByNameAsync(GetGroupByNameQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _queryHandler.GetGroupByNameAsync(query, cancellationToken) ?? throw new GroupNotFoundByNameException(query.Name));

    public Task<List<GroupDto>> GetAllGroupsAsync(CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => (await _queryHandler.GetAllGroupsAsync(cancellationToken)).ToList());

    public Task<List<GroupDto>> GetGroupsByRegistrationStatusAsync(GetGroupsByRegistrationStatusQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => (await _queryHandler.GetGroupsByRegistrationStatusAsync(query, cancellationToken)).ToList());

    public Task<List<GroupDto>> GetGroupsByDeletionStatusAsync(GetGroupsByDeletionStatusQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => (await _queryHandler.GetGroupsByDeletionStatusAsync(query, cancellationToken)).ToList());

    public Task<List<GroupDto>> GetDeletedGroupsAsync(CancellationToken cancellationToken = default) => ExecuteAsync(async () => (await _queryHandler.GetDeletedGroupsAsync(cancellationToken)).ToList());

    private Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken) => _queryHandler.NameExistsAsync(name, cancellationToken);

    private Task<T> ExecuteAsync<T>(Func<Task<T>> operation) => _exceptionService.ExecuteAsync(operation);
}
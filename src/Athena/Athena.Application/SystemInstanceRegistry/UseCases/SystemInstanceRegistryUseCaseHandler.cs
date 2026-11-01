using Athena.Application.SystemInstanceRegistry.Commands;
using Athena.Application.SystemInstanceRegistry.Contracts;
using Athena.Application.SystemInstanceRegistry.Exceptions;
using Athena.Application.SystemInstanceRegistry.Queries;
using Athena.Application.Common.Services;

namespace Athena.Application.SystemInstanceRegistry.UseCases;

public sealed class SystemInstanceRegistryUseCaseHandler(SystemInstanceRegistryCommandHandler commandHandler, SystemInstanceRegistryQueryHandler queryHandler, ApplicationExceptionService exceptionService)
{
    private readonly SystemInstanceRegistryCommandHandler _commandHandler = commandHandler;
    private readonly SystemInstanceRegistryQueryHandler _queryHandler = queryHandler;
    private readonly ApplicationExceptionService _exceptionService = exceptionService;

    // Creation

    public Task<SystemInstanceRegistryDto> CreateSystemInstanceAsync(CreateSystemInstanceCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        ValidateCreateRequest(command);
        if (await _queryHandler.ExistsByNameAsync(new SystemInstanceExistsByNameQuery(command.WorkloadId, command.Name), cancellationToken))
            throw new SystemInstanceNameAlreadyExistsException(command.WorkloadId, command.Name);
        return await _commandHandler.CreateAsync(command, cancellationToken);
    });

    // Naming
    public Task<SystemInstanceRegistryDto> RenameSystemInstanceAsync(RenameSystemInstanceCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        ValidateId(command.SystemInstanceId);
        if (string.IsNullOrWhiteSpace(command.Name)) throw new InvalidSystemInstanceRegistryRequestException("System instance name is required.");
        var current = await GetSystemInstanceOrThrowAsync(command.SystemInstanceId, cancellationToken);
        if (await _queryHandler.ExistsByNameAsync(new SystemInstanceExistsByNameQuery(current.WorkloadId, command.Name), cancellationToken))
        {
            var existing = await _queryHandler.GetByNameAsync(new GetSystemInstanceByNameQuery(current.WorkloadId, command.Name), cancellationToken);
            if (existing is null || existing.Id != current.Id) throw new SystemInstanceNameAlreadyExistsException(current.WorkloadId, command.Name);
        }

        return await _commandHandler.RenameAsync(command, cancellationToken) ?? throw new SystemInstanceNotFoundException(command.SystemInstanceId);
    });

    // Registration
    public Task<SystemInstanceRegistryDto> RegisterSystemInstanceAsync(RegisterSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.RegisterAsync(command, cancellationToken) ?? throw new SystemInstanceNotFoundException(command.SystemInstanceId));

    public Task<SystemInstanceRegistryDto> UnregisterSystemInstanceAsync(UnregisterSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.UnregisterAsync(command, cancellationToken) ?? throw new SystemInstanceNotFoundException(command.SystemInstanceId));

    // Enable / Disable
    public Task<SystemInstanceRegistryDto> EnableSystemInstanceAsync(EnableSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.EnableAsync(command, cancellationToken) ?? throw new SystemInstanceNotFoundException(command.SystemInstanceId));

    public Task<SystemInstanceRegistryDto> DisableSystemInstanceAsync(DisableSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.DisableAsync(command, cancellationToken) ?? throw new SystemInstanceNotFoundException(command.SystemInstanceId));

    // Deletion
    public Task<SystemInstanceRegistryDto> DeleteSystemInstanceAsync(DeleteSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.DeleteAsync(command, cancellationToken) ?? throw new SystemInstanceNotFoundException(command.SystemInstanceId));

    // Retrieval
    public Task<SystemInstanceRegistryDto> GetSystemInstanceByIdAsync(GetSystemInstanceByIdQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        ValidateId(query.SystemInstanceId);
        return await _queryHandler.GetByIdAsync(query, cancellationToken) ?? throw new SystemInstanceNotFoundException(query.SystemInstanceId);
    });

    public Task<SystemInstanceRegistryDto> GetSystemInstanceByNameAsync(GetSystemInstanceByNameQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        ValidateWorkloadId(query.WorkloadId);
        ValidateName(query.Name);
        return await _queryHandler.GetByNameAsync(query, cancellationToken) ?? throw new SystemInstanceNotFoundByNameException(query.WorkloadId, query.Name);
    });

    public Task<List<SystemInstanceRegistryDto>> GetAllSystemInstancesAsync(CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => (await _queryHandler.GetAllAsync(cancellationToken)).ToList());

    public Task<List<SystemInstanceRegistryDto>> GetSystemInstancesBySystemIdAsync(GetSystemInstancesBySystemIdQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        ValidateSystemId(query.SystemId);
        return (await _queryHandler.GetBySystemIdAsync(query, cancellationToken)).ToList();
    });

    public Task<List<SystemInstanceRegistryDto>> GetSystemInstancesByWorkloadIdAsync(GetSystemInstancesByWorkloadIdQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        ValidateWorkloadId(query.WorkloadId);
        return (await _queryHandler.GetByWorkloadIdAsync(query, cancellationToken)).ToList();
    });

    public Task<List<SystemInstanceRegistryDto>> GetSystemInstancesByRegistrationStatusAsync(GetSystemInstancesByRegistrationStatusQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        if (!Enum.IsDefined(query.Status)) throw new InvalidSystemInstanceRegistryRequestException($"Registration status '{query.Status}' is invalid.");
        return (await _queryHandler.GetByRegistrationStatusAsync(query, cancellationToken)).ToList();
    });

    public Task<List<SystemInstanceRegistryDto>> GetSystemInstancesByDeletionStatusAsync(GetSystemInstancesByDeletionStatusQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => (await _queryHandler.GetByDeletionStatusAsync(query, cancellationToken)).ToList());

    // Existence
    public Task<bool> SystemInstanceExistsByIdAsync(SystemInstanceExistsByIdQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        ValidateId(query.SystemInstanceId);
        return await _queryHandler.ExistsByIdAsync(query, cancellationToken);
    });

    public Task<bool> SystemInstanceExistsByNameAsync(SystemInstanceExistsByNameQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        ValidateWorkloadId(query.WorkloadId);
        ValidateName(query.Name);
        return await _queryHandler.ExistsByNameAsync(query, cancellationToken);
    });

    // Internal validation
    private async Task<SystemInstanceRegistryDto> GetSystemInstanceOrThrowAsync(Guid systemInstanceId, CancellationToken cancellationToken)
    {
        return await _queryHandler.GetByIdAsync(new GetSystemInstanceByIdQuery(systemInstanceId), cancellationToken) ?? throw new SystemInstanceNotFoundException(systemInstanceId);
    }

    private static void ValidateCreateRequest(CreateSystemInstanceCommand command)
    {
        ValidateId(command.SystemInstanceId);
        ValidateSystemId(command.SystemId);
        ValidateWorkloadId(command.WorkloadId);
        ValidateName(command.Name);
    }

    private static void ValidateId(Guid id)
    {
        if (id == Guid.Empty) throw new InvalidSystemInstanceRegistryRequestException("System instance ID is required.");
    }

    private static void ValidateSystemId(Guid id)
    {
        if (id == Guid.Empty) throw new InvalidSystemInstanceRegistryRequestException("System ID is required.");
    }

    private static void ValidateWorkloadId(Guid id)
    {
        if (id == Guid.Empty) throw new InvalidSystemInstanceRegistryRequestException("Workload ID is required.");
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new InvalidSystemInstanceRegistryRequestException("System instance name is required.");
    }

    private Task<T> ExecuteAsync<T>(Func<Task<T>> operation) => _exceptionService.ExecuteAsync(operation);
}
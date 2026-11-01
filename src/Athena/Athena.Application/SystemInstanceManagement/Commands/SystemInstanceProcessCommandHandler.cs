using Athena.Application.SystemInstanceManagement.Contracts;
using Athena.Application.SystemInstanceManagement.Services;

namespace Athena.Application.SystemInstanceManagement.Commands;

public sealed class SystemInstanceProcessCommandHandler(SystemInstanceProcessManagementService service)
{
    private readonly SystemInstanceProcessManagementService _service = service;

    public Task<SystemInstanceExecutionStatus> CreateAsync(CreateSystemInstanceCommand command, CancellationToken cancellationToken = default) => _service.CreateAsync(command.Request, cancellationToken);

    public Task<SystemInstanceOperationDto> CreateOperationAsync(CreateSystemInstanceCommand command, CancellationToken cancellationToken = default) => 
        _service.CreateOperationAsync(command.Request, cancellationToken);

    public Task<SystemInstanceExecutionStatus> RegisterAsync(RegisterSystemInstanceCommand command, CancellationToken cancellationToken = default) => _service.RegisterAsync(command.Request, cancellationToken);

    public Task<SystemInstanceOperationDto> RegisterOperationAsync(RegisterSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
        _service.RegisterOperationAsync(command.Request, cancellationToken);

    public Task<SystemInstanceExecutionStatus> StartAsync(StartSystemInstanceCommand command, CancellationToken cancellationToken = default) => _service.StartAsync(command.SystemInstanceId, cancellationToken);

    public Task<SystemInstanceExecutionStatus> StopAsync(StopSystemInstanceCommand command, CancellationToken cancellationToken = default) => _service.StopAsync(command.SystemInstanceId, cancellationToken);

    public Task<SystemInstanceExecutionStatus> RestartAsync(RestartSystemInstanceCommand command, CancellationToken cancellationToken = default) => _service.RestartAsync(command.SystemInstanceId, cancellationToken);

    public Task<SystemInstanceExecutionStatus> RemoveAsync(RemoveSystemInstanceCommand command, CancellationToken cancellationToken = default) => _service.RemoveAsync(command.SystemInstanceId, cancellationToken);

    public Task<SystemInstanceOperationDto> EnqueueOperationAsync(EnqueueSystemInstanceOperationCommand command, CancellationToken cancellationToken = default) =>
        _service.EnqueueOperationAsync(command.SystemInstanceId, command.Type, cancellationToken: cancellationToken);

    public Task<SystemInstanceOperationDto> RemoveOperationAsync(RemoveSystemInstanceOperationCommand command, CancellationToken cancellationToken = default) =>
        _service.RemoveOperationAsync(command.SystemInstanceId, cancellationToken);

    public Task<SystemInstanceOperationDto> UnregisterOperationAsync(UnregisterSystemInstanceOperationCommand command, CancellationToken cancellationToken = default) =>
        _service.UnregisterOperationAsync(command.SystemInstanceId, cancellationToken);

    public Task<SystemInstanceOperationDto> ConfigureOperationAsync(ConfigureSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
        _service.ConfigureOperationAsync(command.SystemInstanceId, command.ConfigurationReference, cancellationToken);

    public Task<SystemInstanceOperationDto?> ResolveOutcomeUnknownOperationAsync(ResolveSystemInstanceOperationCommand command, CancellationToken cancellationToken = default) =>
        _service.ResolveOutcomeUnknownOperationAsync(command.SystemInstanceId, command.OperationId, command.Reason, cancellationToken);
}
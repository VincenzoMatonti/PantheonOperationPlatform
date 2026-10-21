using Athena.Application.Common.Services;
using Athena.Application.OrchestrationRegistry.Commands;
using Athena.Application.OrchestrationRegistry.DTOs;
using Athena.Application.OrchestrationRegistry.Exceptions;
using Athena.Application.OrchestrationRegistry.Queries;

namespace Athena.Application.OrchestrationRegistry.UseCases;

public class OrchestrationRegistryUseCaseHandler(
    OrchestrationRegistryCommandHandler orchestrationCommandHandler,
    OrchestrationRegistryQueryHandler orchestrationQueryHandler,
    ApplicationExceptionService exceptionService)
{
    private readonly OrchestrationRegistryCommandHandler _orchestrationCommandHandler = orchestrationCommandHandler;
    private readonly OrchestrationRegistryQueryHandler _orchestrationQueryHandler = orchestrationQueryHandler;
    private readonly ApplicationExceptionService _exceptionService = exceptionService;

    private Task<T> ExecuteAsync<T>(Func<Task<T>> operation)
    {
        return _exceptionService.ExecuteAsync(operation);
    }

    // =================================================================================================================
    // USE CASE COMMANDS
    public Task<OrchestrationDto> CreateAsync(CreateOrchestrationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            GetOrchestrationByNameQuery query = new() { Name = command.Name };
            OrchestrationDto? existingOrchestration = await _orchestrationQueryHandler.GetByNameAsync(query, cancellationToken);
            if (existingOrchestration is not null && !existingOrchestration.IsDeleted) throw new OrchestrationNameAlreadyExistsException(command.Name);
            return await _orchestrationCommandHandler.CreateAsync(command, cancellationToken);
        });

    public Task<OrchestrationDto> RegisterAsync(RegisterOrchestrationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            OrchestrationDto orchestration = await _orchestrationCommandHandler.RegisterAsync(command, cancellationToken) ??
            throw new OrchestrationNotFoundException(command.OrchestrationId);
            return orchestration;
        });

    public Task<OrchestrationDto> UnregisterAsync(UnregisterOrchestrationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            OrchestrationDto orchestration = await _orchestrationCommandHandler.UnregisterAsync(command, cancellationToken) ??
            throw new OrchestrationNotFoundException(command.OrchestrationId);
            return orchestration;
        });

    public Task<OrchestrationDto> EnableAsync(EnableOrchestrationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            OrchestrationDto orchestration = await _orchestrationCommandHandler.EnableAsync(command, cancellationToken) ??
            throw new OrchestrationNotFoundException(command.OrchestrationId);
            return orchestration;
        });

    public Task<OrchestrationDto> DisableAsync(DisableOrchestrationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            OrchestrationDto orchestration = await _orchestrationCommandHandler.DisableAsync(command, cancellationToken) ??
            throw new OrchestrationNotFoundException(command.OrchestrationId);
            return orchestration;
        });

    public Task<OrchestrationDto> RenameAsync(RenameOrchestrationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            GetOrchestrationByIdQuery idQuery = new() { OrchestrationId = command.OrchestrationId };
            OrchestrationDto orchestration = await _orchestrationQueryHandler.GetByIdAsync(idQuery, cancellationToken) ??
            throw new OrchestrationNotFoundException(command.OrchestrationId);
            GetOrchestrationByNameQuery nameQuery = new() { Name = command.Name };
            OrchestrationDto? existingOrchestration = await _orchestrationQueryHandler.GetByNameAsync(nameQuery, cancellationToken);
            if (existingOrchestration is not null && !existingOrchestration.IsDeleted && existingOrchestration.OrchestrationId != orchestration.OrchestrationId)
                throw new OrchestrationNameAlreadyExistsException(command.Name);

            OrchestrationDto? updatedOrchestration = await _orchestrationCommandHandler.RenameAsync(command, cancellationToken);
            return updatedOrchestration ?? throw new OrchestrationNotFoundException(command.OrchestrationId);
        });

    public Task<OrchestrationDto> MarkAvailableAsync(MarkOrchestrationAvailableCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            OrchestrationDto orchestration = await _orchestrationCommandHandler.MarkAvailableAsync(command, cancellationToken) ??
            throw new OrchestrationNotFoundException(command.OrchestrationId);
            return orchestration;
        });

    public Task<OrchestrationDto> MarkUnavailableAsync(MarkOrchestrationUnavailableCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            OrchestrationDto orchestration = await _orchestrationCommandHandler.MarkUnavailableAsync(command, cancellationToken) ??
            throw new OrchestrationNotFoundException(command.OrchestrationId);
            return orchestration;
        });

    public Task<OrchestrationDto> MarkAvailabilityUnknownAsync(MarkOrchestrationAvailabilityUnknownCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            OrchestrationDto orchestration = await _orchestrationCommandHandler.MarkAvailabilityUnknownAsync(command, cancellationToken) ??
            throw new OrchestrationNotFoundException(command.OrchestrationId);
            return orchestration;
        });

    public Task<OrchestrationDto> DeleteAsync(DeleteOrchestrationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            OrchestrationDto orchestration = await _orchestrationCommandHandler.DeleteAsync(command, cancellationToken) ??
            throw new OrchestrationNotFoundException(command.OrchestrationId);
            return orchestration;
        });

    // =================================================================================================================
    // USE CASE QUERIES

    public Task<OrchestrationDto> GetByIdAsync(GetOrchestrationByIdQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            OrchestrationDto orchestration = await _orchestrationQueryHandler.GetByIdAsync(query, cancellationToken) ??
            throw new OrchestrationNotFoundException(query.OrchestrationId);
            return orchestration;
        });

    public Task<OrchestrationDto> GetByNameAsync(GetOrchestrationByNameQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            OrchestrationDto orchestration = await _orchestrationQueryHandler.GetByNameAsync(query, cancellationToken) ??
            throw new OrchestrationNotFoundByNameException(query.Name);
            return orchestration;
        });

    public Task<List<OrchestrationDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            return await _orchestrationQueryHandler.GetAllAsync(cancellationToken);
        });

    public Task<List<OrchestrationDto>> GetByRegistrationStatusAsync(GetOrchestrationsByRegistrationStatusQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            return await _orchestrationQueryHandler.GetByRegistrationStatusAsync(query, cancellationToken);
        });

    public Task<List<OrchestrationDto>> GetByAvailabilityStatusAsync(GetOrchestrationsByAvailabilityStatusQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            return await _orchestrationQueryHandler.GetByAvailabilityStatusAsync(query, cancellationToken);
        });

    public Task<List<OrchestrationDto>> GetDeletedAsync(CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            return await _orchestrationQueryHandler.GetDeletedAsync(cancellationToken);
        });
}

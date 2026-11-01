using Athena.Application.Common.Services;
using Athena.Application.CompanyRegistry.Exceptions;
using Athena.Application.EcosystemRegistry.Commands;
using Athena.Application.EcosystemRegistry.Contracts;
using Athena.Application.EcosystemRegistry.Exceptions;
using Athena.Application.EcosystemRegistry.Queries;
using Athena.Domain.Company.Repositories;
using Athena.Domain.Company.ValueObjects;

namespace Athena.Application.EcosystemRegistry.UseCases;

public sealed class EcosystemRegistryUseCaseHandler(
    EcosystemRegistryCommandHandler commandHandler,
    EcosystemRegistryQueryHandler queryHandler,
    ICompanyRepository companyRepository,
    ApplicationExceptionService exceptionService)
{
    private readonly EcosystemRegistryCommandHandler _commandHandler = commandHandler;
    private readonly EcosystemRegistryQueryHandler _queryHandler = queryHandler;
    private readonly ICompanyRepository _companyRepository = companyRepository;
    private readonly ApplicationExceptionService _exceptionService = exceptionService;

    public Task<EcosystemDto> CreateEcosystemAsync(CreateEcosystemCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        await EnsureCompanyExistsAsync(command.CompanyId, cancellationToken);
        if (await _queryHandler.NameExistsAsync(command.Name, cancellationToken)) throw new EcosystemNameAlreadyExistsException(command.Name);
        return await _commandHandler.CreateAsync(command, cancellationToken);
    });

    public Task<EcosystemDto> RegisterEcosystemAsync(RegisterEcosystemCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        await GetEcosystemOrThrowAsync(command.EcosystemId, cancellationToken);
        return await _commandHandler.RegisterAsync(command, cancellationToken) ?? throw new EcosystemNotFoundException(command.EcosystemId);
    });

    public Task<EcosystemDto> UnregisterEcosystemAsync(UnregisterEcosystemCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.UnregisterAsync(command, cancellationToken) ?? throw new EcosystemNotFoundException(command.EcosystemId));

    public Task<EcosystemDto> RenameEcosystemAsync(RenameEcosystemCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        var current = await GetEcosystemOrThrowAsync(command.EcosystemId, cancellationToken);
        if (await _queryHandler.NameExistsAsync(command.NewName, cancellationToken))
        {
            var existing = await _queryHandler.GetByNameAsync(new GetEcosystemByNameQuery { Name = command.NewName }, cancellationToken);
            if (existing is null || existing.Id != current.Id) throw new EcosystemNameAlreadyExistsException(command.NewName);
        }

        return await _commandHandler.RenameAsync(command, cancellationToken) ?? throw new EcosystemNotFoundException(command.EcosystemId);
    });

    public Task<EcosystemDto> EnableEcosystemAsync(EnableEcosystemCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.EnableAsync(command, cancellationToken) ?? throw new EcosystemNotFoundException(command.EcosystemId));

    public Task<EcosystemDto> DisableEcosystemAsync(DisableEcosystemCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.DisableAsync(command, cancellationToken) ?? throw new EcosystemNotFoundException(command.EcosystemId));

    public Task<EcosystemDto> DeleteEcosystemAsync(DeleteEcosystemCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.DeleteAsync(command, cancellationToken) ?? throw new EcosystemNotFoundException(command.EcosystemId));

    public Task<EcosystemDto> MarkEcosystemAvailableAsync(MarkEcosystemAvailableCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.MarkAvailableAsync(command, cancellationToken) ?? throw new EcosystemNotFoundException(command.EcosystemId));

    public Task<EcosystemDto> MarkEcosystemUnavailableAsync(MarkEcosystemUnavailableCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.MarkUnavailableAsync(command, cancellationToken) ?? throw new EcosystemNotFoundException(command.EcosystemId));

    public Task<EcosystemDto> MarkEcosystemAvailabilityUnknownAsync(MarkEcosystemAvailabilityUnknownCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.MarkAvailabilityUnknownAsync(command, cancellationToken) ?? throw new EcosystemNotFoundException(command.EcosystemId));

    public Task<EcosystemDto> MarkEcosystemErrorAsync(MarkEcosystemErrorCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.MarkErrorAsync(command, cancellationToken) ?? throw new EcosystemNotFoundException(command.EcosystemId));

    public Task<EcosystemDto> GetEcosystemByIdAsync(GetEcosystemByIdQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _queryHandler.GetByIdAsync(query, cancellationToken) ?? throw new EcosystemNotFoundException(query.EcosystemId));

    public Task<EcosystemDto> GetEcosystemByNameAsync(GetEcosystemByNameQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _queryHandler.GetByNameAsync(query, cancellationToken) ?? throw new EcosystemNotFoundByNameException(query.Name));

    public Task<List<EcosystemDto>> GetAllEcosystemsAsync(CancellationToken cancellationToken = default) => ExecuteAsync(async () => (await _queryHandler.GetAllAsync(cancellationToken)).ToList());

    public Task<List<EcosystemDto>> GetEcosystemsByCompanyIdAsync(GetEcosystemsByCompanyIdQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        await EnsureCompanyExistsAsync(query.CompanyId, cancellationToken);
        return (await _queryHandler.GetByCompanyIdAsync(query, cancellationToken)).ToList();
    });

    public Task<List<EcosystemDto>> GetEcosystemsByRegistrationStatusAsync(GetEcosystemsByRegistrationStatusQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        if (!Enum.IsDefined(query.Status)) throw new ArgumentOutOfRangeException(nameof(query.Status), query.Status, "Invalid ecosystem registration status filter.");
        return (await _queryHandler.GetByRegistrationStatusAsync(query, cancellationToken)).ToList();
    });

    public Task<List<EcosystemDto>> GetEcosystemsByAvailabilityStatusAsync(GetEcosystemsByAvailabilityStatusQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        if (!Enum.IsDefined(query.Status)) throw new ArgumentOutOfRangeException(nameof(query.Status), query.Status, "Invalid ecosystem availability status filter.");
        return (await _queryHandler.GetByAvailabilityStatusAsync(query, cancellationToken)).ToList();
    });

    public Task<List<EcosystemDto>> GetEcosystemsByDeletionStatusAsync(GetEcosystemsByDeletionStatusQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => (await _queryHandler.GetByDeletionStatusAsync(query, cancellationToken)).ToList());

    public Task<List<EcosystemDto>> GetDeletedEcosystemsAsync(CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => (await _queryHandler.GetByDeletionStatusAsync(new GetEcosystemsByDeletionStatusQuery { IsDeleted = true }, cancellationToken)).ToList());

    private async Task<EcosystemDto> GetEcosystemOrThrowAsync(Guid ecosystemId, CancellationToken cancellationToken) =>
        await _queryHandler.GetByIdAsync(new GetEcosystemByIdQuery { EcosystemId = ecosystemId }, cancellationToken) ?? throw new EcosystemNotFoundException(ecosystemId);

    private async Task EnsureCompanyExistsAsync(Guid companyId, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(CompanyId.Create(companyId), cancellationToken);
        if (company is null || company.IsDeleted) throw new CompanyNotFoundException(companyId);
    }

    private Task<T> ExecuteAsync<T>(Func<Task<T>> operation) => _exceptionService.ExecuteAsync(operation);
}
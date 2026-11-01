using Athena.Application.Common.Services;
using Athena.Application.CompanyRegistry.Commands;
using Athena.Application.CompanyRegistry.Contracts;
using Athena.Application.CompanyRegistry.Exceptions;
using Athena.Application.CompanyRegistry.Queries;
using Athena.Application.GroupRegistry.Exceptions;
using Athena.Domain.Company.Repositories;
using Athena.Domain.Company.ValueObjects;

namespace Athena.Application.CompanyRegistry.UseCases;

public sealed class CompanyRegistryUseCaseHandler(
    CompanyRegistryCommandHandler commandHandler,
    CompanyRegistryQueryHandler queryHandler,
    IGroupRepository groupRepository,
    ApplicationExceptionService exceptionService)
{
    private readonly CompanyRegistryCommandHandler _commandHandler = commandHandler;
    private readonly CompanyRegistryQueryHandler _queryHandler = queryHandler;
    private readonly IGroupRepository _groupRepository = groupRepository;
    private readonly ApplicationExceptionService _exceptionService = exceptionService;

    public Task<CompanyDto> CreateCompanyAsync(CreateCompanyCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        var existing = await _queryHandler.GetByNameAsync(new GetCompanyByNameQuery { Name = command.Name }, cancellationToken);
        if (existing is not null || await _queryHandler.NameExistsAsync(command.Name, cancellationToken)) throw new CompanyNameAlreadyExistsException(command.Name);
        if (!string.IsNullOrWhiteSpace(command.TaxCode) && await _queryHandler.TaxCodeExistsAsync(command.CountryCode, command.TaxCode, cancellationToken))
            throw new CompanyTaxCodeAlreadyExistsException(command.TaxCode);

        if (!string.IsNullOrWhiteSpace(command.VatNumber) && await _queryHandler.VatNumberExistsAsync(command.CountryCode, command.VatNumber, cancellationToken))
            throw new CompanyVatNumberAlreadyExistsException(command.VatNumber);

        await EnsureGroupExistsAsync(command.GroupId, cancellationToken);
        return await _commandHandler.CreateAsync(command, cancellationToken);
    });

    public Task<CompanyDto> RegisterCompanyAsync(RegisterCompanyCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        var company = await _queryHandler.GetByNameAsync(new GetCompanyByNameQuery { Name = command.Name }, cancellationToken) ?? throw new CompanyNotFoundByNameException(command.Name);
        return await _commandHandler.RegisterAsync(command, cancellationToken) ?? throw new CompanyNotFoundException(company.Id);
    });

    public Task<CompanyDto> UnregisterCompanyAsync(UnregisterCompanyCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.UnregisterAsync(command, cancellationToken) ?? throw new CompanyNotFoundException(command.CompanyId));

    public Task<CompanyDto> RenameCompanyAsync(RenameCompanyCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        var current = await GetCompanyOrThrowAsync(command.CompanyId, cancellationToken);
        if (await _queryHandler.NameExistsAsync(command.NewName, cancellationToken))
        {
            var existing = await _queryHandler.GetByNameAsync(new GetCompanyByNameQuery { Name = command.NewName }, cancellationToken);
            if (existing is null || existing.Id != current.Id) throw new CompanyNameAlreadyExistsException(command.NewName);
        }

        return await _commandHandler.RenameAsync(command, cancellationToken) ?? throw new CompanyNotFoundException(command.CompanyId);
    });

    public Task<CompanyDto> ChangeCompanyLegalNameAsync(ChangeCompanyLegalNameCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.ChangeLegalNameAsync(command, cancellationToken) ?? throw new CompanyNotFoundException(command.CompanyId));

    public Task<CompanyDto> AssignCompanyToGroupAsync(AssignCompanyToGroupCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        var group = await _groupRepository.GetByIdAsync(GroupId.Create(command.GroupId), cancellationToken);
        if (group is null || group.IsDeleted) throw new GroupNotFoundException(command.GroupId);
        return await _commandHandler.AssignToGroupAsync(command, cancellationToken) ?? throw new CompanyNotFoundException(command.CompanyId);
    });

    public Task<CompanyDto> RemoveCompanyFromGroupAsync(RemoveCompanyFromGroupCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.RemoveFromGroupAsync(command, cancellationToken) ?? throw new CompanyNotFoundException(command.CompanyId));

    public Task<CompanyDto> UpdateCompanyFiscalInformationAsync(UpdateCompanyFiscalInformationCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        var current = await GetCompanyOrThrowAsync(command.CompanyId, cancellationToken);
        if (!string.IsNullOrWhiteSpace(command.TaxCode) && await _queryHandler.TaxCodeExistsAsync(command.CountryCode, command.TaxCode, cancellationToken))
        {
            var existing = await _queryHandler.GetByTaxCodeAsync(new GetCompanyByTaxCodeQuery { CountryCode = command.CountryCode, TaxCode = command.TaxCode }, cancellationToken);
            if (existing is null || existing.Id != current.Id) throw new CompanyTaxCodeAlreadyExistsException(command.TaxCode);
        }

        if (!string.IsNullOrWhiteSpace(command.VatNumber) && await _queryHandler.VatNumberExistsAsync(command.CountryCode, command.VatNumber, cancellationToken))
        {
            var existing = await _queryHandler.GetByVatNumberAsync(new GetCompanyByVatNumberQuery { CountryCode = command.CountryCode, VatNumber = command.VatNumber }, cancellationToken);
            if (existing is null || existing.Id != current.Id) throw new CompanyVatNumberAlreadyExistsException(command.VatNumber);
        }

        return await _commandHandler.UpdateFiscalInformationAsync(command, cancellationToken) ?? throw new CompanyNotFoundException(command.CompanyId);
    });

    public Task<CompanyDto> UpdateCompanyContactInformationAsync(UpdateCompanyContactInformationCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.UpdateContactInformationAsync(command, cancellationToken) ?? throw new CompanyNotFoundException(command.CompanyId));

    public Task<CompanyDto> UpdateCompanyRegisteredAddressAsync(UpdateCompanyRegisteredAddressCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.UpdateRegisteredAddressAsync(command, cancellationToken) ?? throw new CompanyNotFoundException(command.CompanyId));

    public Task<CompanyDto> EnableCompanyAsync(EnableCompanyCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.EnableAsync(command, cancellationToken) ?? throw new CompanyNotFoundException(command.CompanyId));

    public Task<CompanyDto> DisableCompanyAsync(DisableCompanyCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.DisableAsync(command, cancellationToken) ?? throw new CompanyNotFoundException(command.CompanyId));

    public Task<CompanyDto> DeleteCompanyAsync(DeleteCompanyCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.DeleteAsync(command, cancellationToken) ?? throw new CompanyNotFoundException(command.CompanyId));

    public Task<CompanyDto> GetCompanyByIdAsync(GetCompanyByIdQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _queryHandler.GetByIdAsync(query, cancellationToken) ?? throw new CompanyNotFoundException(query.CompanyId));

    public Task<CompanyDto> GetCompanyByNameAsync(GetCompanyByNameQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _queryHandler.GetByNameAsync(query, cancellationToken) ?? throw new CompanyNotFoundByNameException(query.Name));

    public Task<List<CompanyDto>> GetAllCompaniesAsync(CancellationToken cancellationToken = default) => ExecuteAsync(async () => (await _queryHandler.GetAllAsync(cancellationToken)).ToList());

    public Task<List<CompanyDto>> GetCompaniesByGroupIdAsync(GetCompaniesByGroupIdQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        var group = await _groupRepository.GetByIdAsync(GroupId.Create(query.GroupId), cancellationToken);
        if (group is null || group.IsDeleted) throw new GroupNotFoundException(query.GroupId);
        return (await _queryHandler.GetByGroupIdAsync(query, cancellationToken)).ToList();
    });

    public Task<List<CompanyDto>> GetCompaniesByRegistrationStatusAsync(GetCompaniesByRegistrationStatusQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        if (!Enum.IsDefined(query.Status)) throw new ArgumentOutOfRangeException(nameof(query.Status), query.Status, "Invalid company registration status filter.");
        return (await _queryHandler.GetByRegistrationStatusAsync(query, cancellationToken)).ToList();
    });

    public Task<List<CompanyDto>> GetCompaniesByCountryCodeAsync(GetCompaniesByCountryCodeQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => (await _queryHandler.GetByCountryCodeAsync(query, cancellationToken)).ToList());

    public Task<CompanyDto> GetCompanyByTaxCodeAsync(GetCompanyByTaxCodeQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _queryHandler.GetByTaxCodeAsync(query, cancellationToken) ?? throw new CompanyNotFoundByNameException(query.TaxCode));

    public Task<CompanyDto> GetCompanyByVatNumberAsync(GetCompanyByVatNumberQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _queryHandler.GetByVatNumberAsync(query, cancellationToken) ?? throw new CompanyNotFoundByNameException(query.VatNumber));

    public Task<List<CompanyDto>> GetCompaniesByDeletionStatusAsync(GetCompaniesByDeletionStatusQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => (await _queryHandler.GetByDeletionStatusAsync(query, cancellationToken)).ToList());

    public Task<List<CompanyDto>> GetDeletedCompaniesAsync(CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => (await _queryHandler.GetByDeletionStatusAsync(new GetCompaniesByDeletionStatusQuery { IsDeleted = true }, cancellationToken)).ToList());

    private async Task<CompanyDto> GetCompanyOrThrowAsync(Guid companyId, CancellationToken cancellationToken) =>
    await _queryHandler.GetByIdAsync(new GetCompanyByIdQuery { CompanyId = companyId }, cancellationToken) ?? throw new CompanyNotFoundException(companyId);

    private async Task EnsureGroupExistsAsync(Guid? groupId, CancellationToken cancellationToken)
    {
        if (groupId is null) return;
        var group = await _groupRepository.GetByIdAsync(GroupId.Create(groupId.Value), cancellationToken);
        if (group is null || group.IsDeleted) throw new GroupNotFoundException(groupId.Value);
    }

    private Task<T> ExecuteAsync<T>(Func<Task<T>> operation) => _exceptionService.ExecuteAsync(operation);
}
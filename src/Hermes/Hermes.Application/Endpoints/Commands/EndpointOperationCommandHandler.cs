using Hermes.Application.Endpoints.DTOs.EndpointOperationDtos;
using Hermes.Domain.Endpoints.Entities;
using Hermes.Domain.Endpoints.Repositories.EndpointOperationRepositories;

namespace Hermes.Application.Endpoints.Commands;

public class EndpointOperationCommandHandler(IEndpointOperationCommandRepository endpointOperationCommandRepository)
{
    private readonly IEndpointOperationCommandRepository _endpointOperationCommandRepository = endpointOperationCommandRepository;

    public static CreateEndpointOperationDto ConvertEndpointOperationToCreateDto(EndpointOperation endpointOperation)
    {
        return new CreateEndpointOperationDto
        {
            Id = endpointOperation.Id,
            EndpointId = endpointOperation.EndpointId,
            OperationTypeId = endpointOperation.OperationTypeId,
            CreatedAt = endpointOperation.CreatedAt
        };
    }

    public static EnableEndpointOperationDto ConvertEndpointOperationToEnableDto(EndpointOperation endpointOperation)
    {
        return new EnableEndpointOperationDto
        {
            Id = endpointOperation.Id,
            EndpointId = endpointOperation.EndpointId,
            OperationTypeId = endpointOperation.OperationTypeId,
            IsEnabled = endpointOperation.IsEnabled,
            UpdatedAt = endpointOperation.UpdatedAt
        };
    }

    public static DisableEndpointOperationDto ConvertEndpointOperationToDisableDto(EndpointOperation endpointOperation)
    {
        return new DisableEndpointOperationDto
        {
            Id = endpointOperation.Id,
            EndpointId = endpointOperation.EndpointId,
            OperationTypeId = endpointOperation.OperationTypeId,
            IsEnabled = endpointOperation.IsEnabled,
            UpdatedAt = endpointOperation.UpdatedAt
        };
    }

    public static DeleteEndpointOperationDto ConvertEndpointOperationToDeleteDto(EndpointOperation endpointOperation)
    {
        return new DeleteEndpointOperationDto
        {
            Id = endpointOperation.Id,
            EndpointId = endpointOperation.EndpointId,
            OperationTypeId = endpointOperation.OperationTypeId,
            IsDeleted = endpointOperation.IsDeleted,
            UpdatedAt = endpointOperation.UpdatedAt
        };
    }

    public static RestoreEndpointOperationDto ConvertEndpointOperationToRestoreDto(EndpointOperation endpointOperation)
    {
        return new RestoreEndpointOperationDto
        {
            Id = endpointOperation.Id,
            EndpointId = endpointOperation.EndpointId,
            OperationTypeId = endpointOperation.OperationTypeId,
            IsDeleted = endpointOperation.IsDeleted,
            UpdatedAt = endpointOperation.UpdatedAt
        };
    }

    public async Task<EndpointOperation> CreateEndpointOperationAsync(CreateEndpointOperationCommand command, CancellationToken cancellationToken = default)
    {
        var endpointOperation = EndpointOperation.Create(command.EndpointId, command.OperationTypeId);
        await _endpointOperationCommandRepository.AddAsync(endpointOperation, cancellationToken);
        return endpointOperation;
    }

    public async Task EnableEndpointOperationAsync(EndpointOperation endpointOperation)
    {
        endpointOperation.Enable();
        _endpointOperationCommandRepository.Update(endpointOperation);
        await Task.CompletedTask;
    }

    public async Task DisableEndpointOperationAsync(EndpointOperation endpointOperation)
    {
        endpointOperation.Disable();
        _endpointOperationCommandRepository.Update(endpointOperation);
        await Task.CompletedTask;
    }

    public async Task DeleteEndpointOperationAsync(EndpointOperation endpointOperation)
    {
        endpointOperation.MarkAsDeleted();
        _endpointOperationCommandRepository.Update(endpointOperation);
        await Task.CompletedTask;
    }

    public async Task RestoreEndpointOperationAsync(EndpointOperation endpointOperation)
    {
        endpointOperation.Restore();
        _endpointOperationCommandRepository.Update(endpointOperation);
        await Task.CompletedTask;
    }
}


using Hermes.Application.Endpoints.DTOs.EndpointDtos;
using Hermes.Domain.Endpoints.Entities;
using Hermes.Domain.Endpoints.Repositories.Endpoints;
using Hermes.Domain.Endpoints.ValueObjects;

namespace Hermes.Application.Endpoints.Commands;

public class EndpointCommandHandler(IEndpointCommandRepository endpointCommandRepository)
{
    private readonly IEndpointCommandRepository _endpointCommandRepository = endpointCommandRepository;

    public async Task<Endpoint> CreateEndpointAsync(CreateEndpointCommand command, CancellationToken cancellationToken = default)
    {
        var code = EndpointCode.Create(command.Code);
        var type = EndpointType.Create(command.Type);
        var endpoint = Endpoint.Create(code, type);
        await _endpointCommandRepository.AddAsync(endpoint, cancellationToken);
        return endpoint;
    }

    public async Task ChangeEndpointTypeAsync(Endpoint endpoint, string newType, CancellationToken cancellationToken = default)
    {
        var type = EndpointType.Create(newType);
        endpoint.ChangeType(type);
        _endpointCommandRepository.Update(endpoint);
        await Task.CompletedTask;
    }
    public async Task RenameCodeEndpointAsync(Endpoint endpoint, string newCode, CancellationToken cancellationToken = default)
    {
        var code = EndpointCode.Create(newCode);
        endpoint.RenameCode(code);
        _endpointCommandRepository.Update(endpoint);
        await Task.CompletedTask;
    }

    public async Task ActivateEndpointAsync(Endpoint endpoint)
    {
        endpoint.Activate();
        _endpointCommandRepository.Update(endpoint);
        await Task.CompletedTask;
    }

    public async Task DeactivateEndpointAsync(Endpoint endpoint)
    {
        endpoint.Deactivate();
        _endpointCommandRepository.Update(endpoint);
        await Task.CompletedTask;
    }

    public async Task DeleteEndpointAsync(Endpoint endpoint)
    {
        endpoint.MarkAsDeleted();
        _endpointCommandRepository.Update(endpoint);
        await Task.CompletedTask;
    }

    public async Task RestoreEndpointAsync(Endpoint endpoint)
    {
        endpoint.Restore();
        _endpointCommandRepository.Update(endpoint);
        await Task.CompletedTask;
    }

    public static CreateEndpointDto ConvertEndpointToCreateDto(Endpoint endpoint)
    {
        return new CreateEndpointDto
        {
            Id = endpoint.Id,
            Code = endpoint.Code.Value,
            Type = endpoint.Type.Value,
            IsActive = endpoint.IsActive,
            CreatedAt = endpoint.CreatedAt
        };
    }

    public static ChangeEndpointTypeDto ConvertEndpointToRenameDto(Endpoint endpoint)
    {
        return new ChangeEndpointTypeDto
        {
            Id = endpoint.Id,
            Code = endpoint.Code.Value,
            Type = endpoint.Type.Value,
            UpdatedAt = endpoint.UpdatedAt
        };
    }

    public static RenameCodeEndpointDto ConvertEndpointToRenameCodeDto(Endpoint endpoint)
    {
        return new RenameCodeEndpointDto
        {
            Id = endpoint.Id,
            Code = endpoint.Code.Value,
            Type = endpoint.Type.Value,
            UpdatedAt = endpoint.UpdatedAt
        };
    }

    public static ActivateEndpointDto ConvertEndpointToActivateDto(Endpoint endpoint)
    {
        return new ActivateEndpointDto
        {
            Id = endpoint.Id,
            Code = endpoint.Code.Value,
            Type = endpoint.Type.Value,
            IsActive = endpoint.IsActive,
            UpdatedAt = endpoint.UpdatedAt
        };
    }

    public static DeactivateEndpointDto ConvertEndpointToDeactivateDto(Endpoint endpoint)
    {
        return new DeactivateEndpointDto
        {
            Id = endpoint.Id,
            Code = endpoint.Code.Value,
            Type = endpoint.Type.Value,
            IsActive = endpoint.IsActive,
            UpdatedAt = endpoint.UpdatedAt
        };
    }

    public static DeleteEndpointDto ConvertEndpointToDeleteDto(Endpoint endpoint)
    {
        return new DeleteEndpointDto
        {
            Id = endpoint.Id,
            Code = endpoint.Code.Value,
            Type = endpoint.Type.Value,
            IsDeleted = endpoint.IsDeleted,
            UpdatedAt = endpoint.UpdatedAt
        };
    }

    public static RestoreEndpointDto ConvertEndpointToRestoreDto(Endpoint endpoint)
    {
        return new RestoreEndpointDto
        {
            Id = endpoint.Id,
            Code = endpoint.Code.Value,
            Type = endpoint.Type.Value,
            IsDeleted = endpoint.IsDeleted,
            UpdatedAt = endpoint.UpdatedAt
        };
    }
}
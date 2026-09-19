using Hermes.Application.Endpoints.DTOs.EndpointDtos;
using Hermes.Domain.Endpoints.Entities;
using Hermes.Domain.Endpoints.Repositories.EndpointRepositories;
using Hermes.Domain.Endpoints.ValueObjects;

namespace Hermes.Application.Endpoints.Queries;

public class EndpointQueryHandler(IEndpointRepository endpointRepository)
{
    private readonly IEndpointRepository _endpointRepository = endpointRepository;

    public static EndpointDto ConvertEndpointEntitesToDto(Endpoint endpoint)
    {
        return new EndpointDto
        {
            Id = endpoint.Id,
            Code = endpoint.Code.Value,
            Type = endpoint.Type.Value
        };
    }

    public static List<EndpointDto> ConvertEndpointEntitesToDto(List<Endpoint> endpoints)
    {
        return [.. endpoints.Select(ConvertEndpointEntitesToDto)];
    }

    public static EndpointCodeDto ConvertEndpointCodeEntitesToDto(EndpointCode endpointCode)
    {
        return new EndpointCodeDto { Code = endpointCode.Value };
    }

    public static List<EndpointCodeDto> ConvertEndpointCodeEntitesToDto(List<EndpointCode> endpointCodes)
    {
        return [.. endpointCodes.Select(ConvertEndpointCodeEntitesToDto)];
    }

    public async Task<List<Endpoint>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _endpointRepository.GetAllAsync(cancellationToken);
    }

    public async Task<Endpoint?> GetEndpointByIdAsync(GetEndpointByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _endpointRepository.GetByIdAsync(query.EndpointId, cancellationToken);
    }
    public async Task<Endpoint?> GetEndpointByCodeAsync(GetEndpointByCodeQuery query, CancellationToken cancellationToken = default)
    {
        var code = EndpointCode.Create(query.Code);
        return await _endpointRepository.GetByCodeAsync(code, cancellationToken);
    }
    public async Task<Endpoint?> GetEndpointByTypeAsync(GetEndpointByTypeQuery query, CancellationToken cancellationToken = default)
    {
        var type = EndpointType.Create(query.Type);
        return await _endpointRepository.GetByTypeAsync(type, cancellationToken);
    }
    public async Task<EndpointCode?> GetEndpointCodeByIdAsync(GetEndpointCodeByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _endpointRepository.GetEndpointCodeByIdAsync(query.EndpointId, cancellationToken);
    }
    public async Task<List<EndpointCode>> GetAllEndpointCodesAsync(CancellationToken cancellationToken = default)
    {
        return await _endpointRepository.GetAllEndpointCodeAsync(cancellationToken);
    }
    public async Task<List<Endpoint>> GetActiveEndpointsAsync(CancellationToken cancellationToken = default)
    {
        return await _endpointRepository.GetActiveEndpointAsync(cancellationToken);
    }
    public async Task<List<Endpoint>> GetNonActiveEndpointsAsync(CancellationToken cancellationToken = default)
    {
        return await _endpointRepository.GetNonActiveEndpointAsync(cancellationToken);
    }
    public async Task<List<Endpoint>> GetDeletedEndpointsAsync(CancellationToken cancellationToken = default)
    {
        return await _endpointRepository.GetDeletedEndpointAsync(cancellationToken);
    }
}
using Hermes.Application.Endpoints.DTOs.EndpointOperationDtos;
using Hermes.Domain.Endpoints.Entities;
using Hermes.Domain.Endpoints.Repositories.EndpointOperationRepositories;

namespace Hermes.Application.Endpoints.Queries;

public class EndpointOperationQueryHandler(IEndpointOperationQueryRepository endpointOperationQueryRepository)
{
    private readonly IEndpointOperationQueryRepository _endpointOperationQueryRepository = endpointOperationQueryRepository;

    public static EndpointOperationDto ConvertEndpointOperationEntitesToDto(EndpointOperation endpointOperation)
    {
        return new EndpointOperationDto
        {
            Id = endpointOperation.Id,
            EndpointId = endpointOperation.EndpointId,
            OperationTypeId = endpointOperation.OperationTypeId,
            IsEnabled = endpointOperation.IsEnabled,
            IsDeleted = endpointOperation.IsDeleted
        };
    }

    public static List<EndpointOperationDto> ConvertEndpointOperationEntitesToDto(List<EndpointOperation> endpointOperations)
    {
        return [.. endpointOperations.Select(ConvertEndpointOperationEntitesToDto)];
    }

    public async Task<List<EndpointOperation>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _endpointOperationQueryRepository.GetAllAsync(cancellationToken);
    }

    public async Task<EndpointOperation?> GetEndpointOperationByIdAsync(GetEndpointOperationByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _endpointOperationQueryRepository.GetByIdAsync(query.EndpointOperationId, cancellationToken);
    }

    public async Task<EndpointOperation?> GetEndpointOperationAsync(GetEndpointOperationQuery query, CancellationToken cancellationToken = default)
    {
        return await _endpointOperationQueryRepository.GetAsync(query.EndpointId, query.OperationTypeId, cancellationToken);
    }

    public async Task<List<EndpointOperation>> GetEndpointOperationsByEndpointIdAsync(GetEndpointOperationsByEndpointIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _endpointOperationQueryRepository.GetByEndpointIdAsync(query.EndpointId, cancellationToken);
    }

    public async Task<List<EndpointOperation>> GetActiveEndpointOperationsAsync(CancellationToken cancellationToken = default)
    {
        return await _endpointOperationQueryRepository.GetActiveEndpointOperation(cancellationToken);
    }

    public async Task<List<EndpointOperation>> GetNonActiveEndpointOperationsAsync(CancellationToken cancellationToken = default)
    {
        return await _endpointOperationQueryRepository.GetNonActiveEndpointOperation(cancellationToken);
    }

    public async Task<List<EndpointOperation>> GetDeletedEndpointOperationsAsync(CancellationToken cancellationToken = default)
    {
        return await _endpointOperationQueryRepository.GetDeletedEndpointOperation(cancellationToken);
    }
}

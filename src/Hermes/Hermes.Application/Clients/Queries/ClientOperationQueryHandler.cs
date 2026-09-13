using Hermes.Domain.Clients.Entities;
using Hermes.Domain.Clients.Repositories;
using Hermes.Application.Clients.DTOs.ClientOperationDtos;

namespace Hermes.Application.Clients.Queries
{
    public class ClientOperationQueryHandler(IClientOperationQueryRepository clientOperationQueryRepository)
    {
        private readonly IClientOperationQueryRepository _clientOperationQueryRepository = clientOperationQueryRepository;

        public static ClientOperationDto ConvertClientOperationEntitesToDto(ClientOperation clientOperation)
        {
            return new ClientOperationDto
            {
                Id = clientOperation.Id,
                ClientId = clientOperation.ClientId,
                OperationTypeId = clientOperation.OperationTypeId,
            };
        }

        public static List<ClientOperationDto> ConvertClientOperationEntitesToDto(List<ClientOperation> clientOperations)
        {
            return [.. clientOperations.Select(ConvertClientOperationEntitesToDto)];
        }

        public async Task<ClientOperation?> GetClientOperationByIdAsync(GetClientOperationByIdQuery query, CancellationToken cancellationToken = default)
        {
            return await _clientOperationQueryRepository.GetByIdAsync(query.ClientOperationId, cancellationToken);
        }

        public async Task<ClientOperation?> GetClientOperationAsync(GetClientOperationQuery query, CancellationToken cancellationToken = default)
        {
            return await _clientOperationQueryRepository.GetAsync(query.ClientId, query.OperationTypeId, cancellationToken);
        }

        public async Task<List<ClientOperation>> GetClientOperationsByClientIdAsync(GetClientOperationsByClientIdQuery query, CancellationToken cancellationToken = default)
        {
            return await _clientOperationQueryRepository.GetByClientIdAsync(query.ClientId, cancellationToken);
        }

        public async Task<List<ClientOperation>> GetClientOperationsByOperationTypeIdAsync(GetClientOperationsByOperationTypeIdQuery query, CancellationToken cancellationToken = default)
        {
            return await _clientOperationQueryRepository.GetByOperationIdAsync(cancellationToken);
        }

        public async Task<List<ClientOperation>> GetActiveClientOperationsAsync(CancellationToken cancellationToken = default)
        {
            return await _clientOperationQueryRepository.GetActiveClientOperation(cancellationToken);
        }

        public async Task<List<ClientOperation>> GetNonActiveClientOperationsAsync(CancellationToken cancellationToken = default)
        {
            return await _clientOperationQueryRepository.GetNonActiveClientOperation(cancellationToken);
        }

        public async Task<List<ClientOperation>> GetDeletedClientOperationsAsync(CancellationToken cancellationToken = default)
        {
            return await _clientOperationQueryRepository.GetDeletedClientOperation(cancellationToken);
        }
    }
}
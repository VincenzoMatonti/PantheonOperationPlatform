using Hermes.Application.Clients.DTOs.ClientDTOs;
using Hermes.Domain.Clients.Entities;
using Hermes.Domain.Clients.ValueObjects;
using Hermes.Domain.Clients.Repositories.ClientRepositories;

namespace Hermes.Application.Clients.Queries
{
    public class ClientQueryHandler(IClientQueryRepository clientQueryRepository)
    {
        private readonly IClientQueryRepository _clientQueryRepository = clientQueryRepository;

        public static ClientDto ConvertClientEntitesToDto(Client client)
        {
            return new ClientDto
            {
                Id = client.Id,
                Code = client.Code.Value,
                Name = client.Name
            };
        }

        public static List<ClientDto> ConvertClientEntitesToDto(List<Client> clients)
        {
            return [.. clients.Select(ConvertClientEntitesToDto)];
        }

        public static ClientCodeDto ConvertClientCodeEntitesToDto(ClientCode clientCode)
        {
            return new ClientCodeDto
            {
                Code = clientCode.Value
            };
        }

        public static List<ClientCodeDto> ConvertClientCodeEntitesToDto(List<ClientCode> clientCodes)
        {
            return [.. clientCodes.Select(ConvertClientCodeEntitesToDto)];
        }

        public async Task<List<Client>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _clientQueryRepository.GetAllAsync(cancellationToken);
        }

        public async Task<Client?> GetClientByIdAsync(GetClientByIdQuery query, CancellationToken cancellationToken = default)
        {
            return await _clientQueryRepository.GetByIdAsync(query.ClientId, cancellationToken);
        }

        public async Task<Client?> GetClientByCodeAsync(GetClientByCodeQuery query, CancellationToken cancellationToken = default)
        {
            var code = ClientCode.Create(query.Code);
            return await _clientQueryRepository.GetByCodeAsync(code, cancellationToken);
        }

        public async Task<ClientCode?> GetClientCodeByIdAsync(GetClientCodeByIdQuery query, CancellationToken cancellationToken = default)
        {
            return await _clientQueryRepository.GetClientCodeByIdAsync(query.ClientId, cancellationToken);
        }

        public async Task<List<ClientCode>> GetAllClientCodesAsync(CancellationToken cancellationToken = default)
        {
            return await _clientQueryRepository.GetAllClientCodeAsync(cancellationToken);
        }

        public async Task<List<Client>> GetActiveClientsAsync(CancellationToken cancellationToken = default)
        {
            return await _clientQueryRepository.GetActiveClientAsync(cancellationToken);
        }

        public async Task<List<Client>> GetNonActiveClientsAsync(CancellationToken cancellationToken = default)
        {
            return await _clientQueryRepository.GetNonActiveClientAsync(cancellationToken);
        }

        public async Task<List<Client>> GetDeletedClientsAsync(CancellationToken cancellationToken = default)
        {
            return await _clientQueryRepository.GetDeletedClientAsync(cancellationToken);
        }
    }
}
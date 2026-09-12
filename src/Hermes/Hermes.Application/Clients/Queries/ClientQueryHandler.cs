using Hermes.Application.Clients.DTOs.ClientDTOs;
using Hermes.Domain.Clients.Entities;
using Hermes.Domain.Clients.ValueObjects;
using Hermes.Domain.Clients.Repositories.ClientRepositories;

namespace Hermes.Application.Clients.Queries
{
    public class ClientQueryHandler(IClientQueryRepository clientQueryRepository)
    {
        private readonly IClientQueryRepository _clientQueryRepository = clientQueryRepository;

        public static async Task<ClientDto> ConvertClientEntitesToDtoAsync(Client client)
        {
            return new ClientDto
            {
                Id = client.Id,
                Code = client.Code.Value,
                Name = client.Name,
            };
        }

        public static async Task<Client> ConvertClientDtoToEntitesAsync(ClientDto clientDto)
        {
            var code = ClientCode.Create(clientDto.Code);
            var client = Client.Create(code, clientDto.Name);
            return client;
        }

        public static async Task<ClientCodeDto> ConvertClientCodeEntitesToDtoAsync(ClientCode clientCode)
        {
            return new ClientCodeDto
            {
                Code = clientCode.Value,
            };
        }        

        public static async Task<ClientCode> ConvertClientCodeDtoToEntitesAsync(ClientCodeDto clientCodeDto)
        {
            var code = ClientCode.Create(clientCodeDto.Code);
            return code;
        }

        public async Task<List<ClientDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var clients = await _clientQueryRepository.GetAllAsync(cancellationToken);
            return [.. clients.Select(client => new ClientDto
            {
                Id = client.Id,
                Code = client.Code.Value,
                Name = client.Name,
            })];

        }

        public async Task<ClientDto?> GetClientByIdAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            var client = await _clientQueryRepository.GetByIdAsync(clientId, cancellationToken);
            if (client is null)
            {
                return null;
            }

            return new ClientDto
            {
                Id = client.Id,
                Code = client.Code.Value,       
                Name = client.Name,
            };
        }

        public async Task<ClientDto?> GetClientByCodeAsync(GetClientByCodeQuery query, CancellationToken cancellationToken = default)
        {
            var code = ClientCode.Create(query.Code);
            var client = await _clientQueryRepository.GetByCodeAsync(code, cancellationToken);
            if (client is null)
            {
                return null;
            }

            return new ClientDto
            {
                Id = client.Id,
                Code = client.Code.Value,
                Name = client.Name,
            };
        }

        public async Task<ClientCode?> GetClientCodeByIdAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
           return await _clientQueryRepository.GetClientCodeByIdAsync(clientId, cancellationToken);
        }

        public async Task<List<ClientCodeDto>> GetAllClientCodesAsync(CancellationToken cancellationToken = default)
        {
            var clientCodes = await _clientQueryRepository.GetAllClientCodeAsync(cancellationToken);
            return [.. clientCodes.Select(clientCode => new ClientCodeDto
            {
                Code = clientCode.Value,
            })];
        }
        
        public async Task<List<ClientDto>> GetActiveClientsAsync(CancellationToken cancellationToken = default)
        {
            var clients = await _clientQueryRepository.GetActiveClientAsync(cancellationToken);
            var activeClients = clients.Select(client => new ClientDto
            {
                Id = client.Id,
                Code = client.Code.Value,
                Name = client.Name
            })
            .ToList();

            return activeClients;
        }

        public async Task<bool> IsActiveClientAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            return await _clientQueryRepository.IsActiveClientAsync(clientId, cancellationToken);
        }

        public async Task<List<ClientDto>> GetNonActiveClientsAsync(CancellationToken cancellationToken = default)
        {
            var clients = await _clientQueryRepository.GetNonActiveClientAsync(cancellationToken);
            var nonActiveClients = clients.Select(client => new ClientDto
            {
                Id = client.Id,
                Code = client.Code.Value,
                Name = client.Name
            })
            .ToList();

            return nonActiveClients;
        }

        public async Task<bool> IsNonActiveClientAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            return await _clientQueryRepository.IsNonActiveClientAsync(clientId, cancellationToken);
        }

        public async Task<List<ClientDto>> GetDeletedClientsAsync(CancellationToken cancellationToken = default)
        {
            var clients = await _clientQueryRepository.GetDeletedClientAsync(cancellationToken);
            var deletedClients = clients.Select(client => new ClientDto
            {
                Id = client.Id,
                Code = client.Code.Value,
                Name = client.Name
            })
            .ToList();

            return deletedClients;
        }

        public async Task<bool> IsDeletedClientAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            return await _clientQueryRepository.IsDeletedClientAsync(clientId, cancellationToken);
        }
    }
}
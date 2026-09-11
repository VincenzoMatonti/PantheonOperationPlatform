using Hermes.Application.Clients.DTOs;
using Hermes.Domain.Clients.ValueObjects;
using Hermes.Domain.Clients.Repositories.ClientRepositories;

namespace Hermes.Application.Clients.Queries
{
    public class ClientQueryHandler
    {
        private readonly IClientQueryRepository _clientQueryRepository;

        public ClientQueryHandler(IClientQueryRepository clientQueryRepository)
        {
            _clientQueryRepository = clientQueryRepository;
        }

        public async Task<List<ClientDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var clients = await _clientQueryRepository.GetAllAsync(cancellationToken);
            return [.. clients.Select(client => new ClientDto
            {
                Id = client.Id,
                Code = client.Code.Value,
                Name = client.Name,
                IsActive = client.IsActive
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
                IsActive = client.IsActive
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
                IsActive = client.IsActive
            };
        }

        public async Task<List<ActiveClientDto>> GetActiveClientsAsync(CancellationToken cancellationToken = default)
        {
            var clients = await _clientQueryRepository.GetActiveClientAsync(cancellationToken);
            var activeClients = clients.Select(client => new ActiveClientDto
            {
                Id = client.Id,
                Code = client.Code.Value,
                Name = client.Name
            })
            .ToList();

            return activeClients;
        }

        public async Task<List<ActiveClientDto>> GetNonActiveClientsAsync(CancellationToken cancellationToken = default)
        {
            var clients = await _clientQueryRepository.GetNonActiveClientAsync(cancellationToken);
            var nonActiveClients = clients.Select(client => new ActiveClientDto
            {
                Id = client.Id,
                Code = client.Code.Value,
                Name = client.Name
            })
            .ToList();

            return nonActiveClients;
        }
    }
}
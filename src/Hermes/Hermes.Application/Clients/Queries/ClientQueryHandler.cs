using Hermes.Application.Clients.DTOs;
using Hermes.Domain.Clients.ValueObjects;
using Hermes.Domain.Clients.Repositories;

namespace Hermes.Application.Clients.Queries
{
    public class ClientQueryHandler
    {
        private readonly IClientRepository _clientRepository;

        public ClientQueryHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<List<ClientDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var clients = await _clientRepository.GetAllAsync(cancellationToken);
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
            var client = await _clientRepository.GetByIdAsync(clientId, cancellationToken);
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
            var client = await _clientRepository.GetByCodeAsync(code, cancellationToken);
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
            var clients = await _clientRepository.GetActiveClientAsync(cancellationToken);
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
            var clients = await _clientRepository.GetNonActiveClientAsync(cancellationToken);
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
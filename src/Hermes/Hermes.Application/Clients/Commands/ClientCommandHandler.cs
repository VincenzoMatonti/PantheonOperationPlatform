using Hermes.Domain.Clients.Repositories;
using Hermes.Domain.Clients.Entities;
using Hermes.Domain.Clients.ValueObjects;
using Hermes.Application.Clients.DTOs;


namespace Hermes.Application.Clients.Commands
{
    public class ClientCommandHandler
    {
        private readonly IClientRepository _clientRepository;

        public ClientCommandHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<ClientDto> CreateClientAsync(CreateClientCommand command, CancellationToken cancellationToken = default)
        {
            var code = ClientCode.Create(command.Code);
            var existingClient = await _clientRepository.GetByCodeAsync(code, cancellationToken);
            if (existingClient is not null)
            {
                throw new InvalidOperationException($"A client with code '{code.Value}' already exists.");
            }

            var client = Client.Create(code, command.Name);
            await _clientRepository.AddAsync(client, cancellationToken);
            return new ClientDto
            {
                Id = client.Id,
                Code = client.Code.Value,
                Name = client.Name,
                IsActive = client.IsActive
            };
        }

        public async Task RenameClientAsync(RenameClientCommand command, CancellationToken cancellationToken = default)
        {
            var client = await _clientRepository.GetByIdAsync(command.ClientId, cancellationToken) ??
                         throw new InvalidOperationException($"Client with ID '{command.ClientId}' was not found.");

            client.Rename(command.Name);
            _clientRepository.Update(client);
        }

        public async Task ActivateClientAsync(ActivateClientCommand command, CancellationToken cancellationToken = default)
        {
            var client = await _clientRepository.GetByIdAsync(command.ClientId, cancellationToken) ??
                         throw new InvalidOperationException($"Client with ID '{command.ClientId}' was not found.");

            client.Activate();
            _clientRepository.Update(client);
        }

        public async Task DeactivateClientAsync(DeactivateClientCommand command, CancellationToken cancellationToken = default)
        {
            var client = await _clientRepository.GetByIdAsync(command.ClientId, cancellationToken) ??
                         throw new InvalidOperationException($"Client with ID '{command.ClientId}' was not found.");

            client.Deactivate();
            _clientRepository.Update(client);

        }
    }
}

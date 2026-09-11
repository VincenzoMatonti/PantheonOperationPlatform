using Hermes.Domain.Clients.Repositories.ClientRepositories;
using Hermes.Domain.Clients.Entities;
using Hermes.Domain.Clients.ValueObjects;
using Hermes.Application.Clients.DTOs;


namespace Hermes.Application.Clients.Commands
{
    public class ClientCommandHandler
    {
        private readonly IClientCommandRepository _clientCommandRepository;
        private readonly IClientQueryRepository _clientQueryRepository;

        public ClientCommandHandler(IClientCommandRepository clientCommandRepository, IClientQueryRepository clientQueryRepository)
        {
            _clientCommandRepository = clientCommandRepository;
            _clientQueryRepository = clientQueryRepository;
        }

        public async Task<ClientDto> CreateClientAsync(CreateClientCommand command, CancellationToken cancellationToken = default)
        {
            var code = ClientCode.Create(command.Code);
            var existingClient = await _clientQueryRepository.GetByCodeAsync(code, cancellationToken);
            if (existingClient is not null)
            {
                throw new InvalidOperationException($"A client with code '{code.Value}' already exists.");
            }

            var client = Client.Create(code, command.Name);
            await _clientCommandRepository.AddAsync(client, cancellationToken);
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
            var client = await _clientQueryRepository.GetByIdAsync(command.ClientId, cancellationToken) ??
                         throw new InvalidOperationException($"Client with ID '{command.ClientId}' was not found.");

            client.Rename(command.Name);
            _clientCommandRepository.Update(client);
        }

        public async Task ActivateClientAsync(ActivateClientCommand command, CancellationToken cancellationToken = default)
        {
            var client = await _clientQueryRepository.GetByIdAsync(command.ClientId, cancellationToken) ??
                         throw new InvalidOperationException($"Client with ID '{command.ClientId}' was not found.");

            client.Activate();
            _clientCommandRepository.Update(client);
        }

        public async Task DeactivateClientAsync(DeactivateClientCommand command, CancellationToken cancellationToken = default)
        {
            var client = await _clientQueryRepository.GetByIdAsync(command.ClientId, cancellationToken) ??
                         throw new InvalidOperationException($"Client with ID '{command.ClientId}' was not found.");

            client.Deactivate();
            _clientCommandRepository.Update(client);

        }
    }
}

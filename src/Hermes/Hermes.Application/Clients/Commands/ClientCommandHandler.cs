using Hermes.Domain.Clients.Entities;
using Hermes.Domain.Clients.ValueObjects;
using Hermes.Application.Clients.DTOs.ClientDTOs;
using Hermes.Domain.Clients.Repositories.ClientRepositories;

namespace Hermes.Application.Clients.Commands
{
    public class ClientCommandHandler(IClientCommandRepository clientCommandRepository)
    {
        private readonly IClientCommandRepository _clientCommandRepository = clientCommandRepository;

        public static async Task<ClientDto> ConvertEntitesToDtoAsync(Client client)
        {
            return new ClientDto
            {
                Id = client.Id,
                Code = client.Code.Value,
                Name = client.Name,
            };
        }

        public static async Task<Client> ConvertDtoToEntitesAsync(ClientDto clientDto)
        {
            var code = ClientCode.Create(clientDto.Code);
            var client = Client.Create(code, clientDto.Name);
            return client;
        }

        public async Task<Client> CreateClientAsync(CreateClientCommand command, CancellationToken cancellationToken = default)
        {
            var code = ClientCode.Create(command.Code);
            var client = Client.Create(code, command.Name);
            await _clientCommandRepository.AddAsync(client, cancellationToken);
            return client;
        }

        public async Task RenameClientAsync(Client client)
        {
            client.Rename(client.Name);
            _clientCommandRepository.Update(client);
        }

        public async Task RenameCodeClientAsync(Client client, string newCode)
        {
            var code = ClientCode.Create(newCode);
            client.RenameCode(code);
            _clientCommandRepository.Update(client);
        }

        public async Task ActivateClientAsync(Client client)
        {
            client.Activate();
            _clientCommandRepository.Update(client);
        }

        public async Task DeactivateClientAsync(Client client)
        {
            client.Deactivate();
            _clientCommandRepository.Update(client);
        }

        public async Task DeleteClientAsync(Client client)
        {
            client.MarkAsDeleted();
            _clientCommandRepository.Update(client);
        }

        public async Task RestoreClientAsync(Client client)
        {
            client.Restore();
            _clientCommandRepository.Update(client);
        }
    }
}

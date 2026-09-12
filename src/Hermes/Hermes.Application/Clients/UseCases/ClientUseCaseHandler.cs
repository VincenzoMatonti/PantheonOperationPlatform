using Hermes.Application.Clients.Queries;
using Hermes.Application.Clients.Commands;
using Hermes.Application.Clients.DTOs.ClientDTOs;
using Hermes.Domain.Clients.ValueObjects;


namespace Hermes.Application.Clients.UseCases
{
    public class ClientUseCaseHandler(ClientQueryHandler clientQueryHandler, ClientCommandHandler clientCommandHandler)
    {
        private readonly ClientQueryHandler _clientQueryHandler = clientQueryHandler;
        private readonly ClientCommandHandler _clientCommandHandler = clientCommandHandler;

        public async Task<ClientDto> CreateClientAsync(CreateClientCommand command, CancellationToken cancellationToken = default)
        {
            var clientCode = new GetClientByCodeQuery { Code = command.Code };
            var existingClient = await _clientQueryHandler.GetClientByCodeAsync(clientCode, cancellationToken);
            if (existingClient != null) throw new InvalidOperationException($"Client with code '{clientCode.Code}' already exists.");
            var newClient = await _clientCommandHandler.CreateClientAsync(command, cancellationToken);
            var newClientDto = await ClientCommandHandler.ConvertEntitesToDtoAsync(newClient);
            return newClientDto;
        }

        public async Task RenameClientAsync(RenameClientCommand command, CancellationToken cancellationToken = default)
        {
            var client = await _clientQueryHandler.GetClientByIdAsync(command.ClientId, cancellationToken);
            if (client != null)
            {
                var clientEntity = await ClientCommandHandler.ConvertDtoToEntitesAsync(client);
                await _clientCommandHandler.RenameClientAsync(clientEntity);
            }
            else throw new InvalidOperationException($"Client with ID '{command.ClientId}' was not found.");
        }

        public async Task RenameCodeClientAsync(RenameCodeClientCommand command, CancellationToken cancellationToken = default)
        {
            var client = await _clientQueryHandler.GetClientByIdAsync(command.ClientId, cancellationToken);
            if (client != null)
            {
                var clientCode = new GetClientByCodeQuery { Code = command.NewCode };
                var existingClientWithNewCode = await _clientQueryHandler.GetClientByCodeAsync(clientCode, cancellationToken);
                if (existingClientWithNewCode != null) throw new InvalidOperationException($"Client with code '{command.NewCode}' already exists.");
                var clientEntity = await ClientCommandHandler.ConvertDtoToEntitesAsync(client);
                await _clientCommandHandler.RenameCodeClientAsync(clientEntity, command.NewCode);
            }
            else throw new InvalidOperationException($"Client with ID '{command.ClientId}' was not found.");
        }

        public async Task ActivateClientAsync(ActivateClientCommand command, CancellationToken cancellationToken = default)
        {
            var client = await _clientQueryHandler.GetClientByIdAsync(command.ClientId, cancellationToken);
            if (client != null)
            {
                var result = await _clientQueryHandler.IsActiveClientAsync(command.ClientId, cancellationToken);
                if (result) throw new InvalidOperationException($"Client with ID '{command.ClientId}' is already active.");
                var clientEntity = await ClientCommandHandler.ConvertDtoToEntitesAsync(client);
                await _clientCommandHandler.ActivateClientAsync(clientEntity);
            }
            else throw new InvalidOperationException($"Client with ID '{command.ClientId}' was not found.");
        }

        public async Task DeactivateClientAsync(DeactivateClientCommand command, CancellationToken cancellationToken = default)
        {
            var client = await _clientQueryHandler.GetClientByIdAsync(command.ClientId, cancellationToken);
            if (client != null)
            {
                var result = await _clientQueryHandler.IsNonActiveClientAsync(command.ClientId, cancellationToken);
                if (result) throw new InvalidOperationException($"Client with ID '{command.ClientId}' is already inactive.");
                var clientEntity = await ClientCommandHandler.ConvertDtoToEntitesAsync(client);
                await _clientCommandHandler.DeactivateClientAsync(clientEntity);
            }
            else throw new InvalidOperationException($"Client with ID '{command.ClientId}' was not found.");
        }

        public async Task DeletedClientAsync(DeletedClientCommand command, CancellationToken cancellationToken = default)
        {
            var client = await _clientQueryHandler.GetClientByIdAsync(command.ClientId, cancellationToken);
            if (client != null)
            {
                var result = await _clientQueryHandler.IsDeletedClientAsync(command.ClientId, cancellationToken);
                if (result) throw new InvalidOperationException($"Client with ID '{command.ClientId}' is already deleted.");
                var clientEntity = await ClientCommandHandler.ConvertDtoToEntitesAsync(client);
                await _clientCommandHandler.DeleteClientAsync(clientEntity);
            }
            else throw new InvalidOperationException($"Client with ID '{command.ClientId}' was not found.");
        }

        public async Task RestoreClientAsync(RestoreClientCommand command, CancellationToken cancellationToken = default)
        {
            var client = await _clientQueryHandler.GetClientByIdAsync(command.ClientId, cancellationToken);
            if (client != null)
            {
                var result = await _clientQueryHandler.IsDeletedClientAsync(command.ClientId, cancellationToken);
                if (!result) throw new InvalidOperationException($"Client with ID '{command.ClientId}' is not deleted.");
                var clientEntity = await ClientCommandHandler.ConvertDtoToEntitesAsync(client);
                await _clientCommandHandler.RestoreClientAsync(clientEntity);
            }
            else throw new InvalidOperationException($"Client with ID '{command.ClientId}' was not found.");
        }

        public async Task<ClientDto?> GetClientByIdAsync(GetClientByIdQuery query, CancellationToken cancellationToken = default)
        {
            var client = await _clientQueryHandler.GetClientByIdAsync(query.ClientId, cancellationToken);
            return client is null ? throw new InvalidOperationException($"Client with ID '{query.ClientId}' was not found.") : client;
        }

        public async Task<ClientDto?> GetClientByCodeAsync(GetClientByCodeQuery query, CancellationToken cancellationToken = default)
        {
            var client = await _clientQueryHandler.GetClientByCodeAsync(query, cancellationToken);
            return client is null ? throw new InvalidOperationException($"Client with code '{query.Code}' was not found.") : client;
        }

        public async Task<ClientCodeDto?> GetClientCodeByIdAsync(GetClientCodeByIdQuery query, CancellationToken cancellationToken = default)
        {
            var clientCode = await _clientQueryHandler.GetClientCodeByIdAsync(query.ClientId, cancellationToken);
            if (clientCode != null)
            {
                ClientCodeDto? clientCodeDto = await ClientQueryHandler.ConvertClientCodeEntitesToDtoAsync(clientCode);
                return clientCodeDto;
            }
            else throw new InvalidOperationException($"Client code with ID '{query.ClientId}' was not found.");          
        }

        public async Task<List<ClientDto>> GetAllClientsAsync(CancellationToken cancellationToken = default)
        {
            var clients = await _clientQueryHandler.GetAllAsync(cancellationToken);
            if (clients == null || clients.Count == 0) return [];
            return clients;
        }

        public async Task<List<ClientCodeDto>> GetAllClientCodesAsync(CancellationToken cancellationToken = default)
        {
            var clientCodes = await _clientQueryHandler.GetAllClientCodesAsync(cancellationToken);
            if (clientCodes == null || clientCodes.Count == 0) return [];
            return clientCodes;
        }

        public async Task<List<ClientDto>> GetActiveClientsAsync(CancellationToken cancellationToken = default)
        {
            var clients = await _clientQueryHandler.GetActiveClientsAsync(cancellationToken);
            if (clients == null || clients.Count == 0) return [];
            return clients;
        }

        public async Task<List<ClientDto>> GetNonActiveClientsAsync(CancellationToken cancellationToken = default)
        {
            var clients = await _clientQueryHandler.GetNonActiveClientsAsync(cancellationToken);
            if (clients == null || clients.Count == 0) return [];
            return clients;
        }

        public async Task<List<ClientDto>> GetDeletedClientsAsync(CancellationToken cancellationToken = default)
        {
            var clients = await _clientQueryHandler.GetDeletedClientsAsync(cancellationToken);
            if (clients == null || clients.Count == 0) return [];
            return clients;
        }
    }
}
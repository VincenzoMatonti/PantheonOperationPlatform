using Hermes.Application.Clients.Queries;
using Hermes.Application.Clients.Commands;
using Hermes.Application.Clients.DTOs.ClientDTOs;
using Hermes.Application.Clients.Exceptions;


namespace Hermes.Application.Clients.UseCases
{
    public class ClientUseCaseHandler(ClientQueryHandler clientQueryHandler, ClientCommandHandler clientCommandHandler)
    {
        private readonly ClientQueryHandler _clientQueryHandler = clientQueryHandler;
        private readonly ClientCommandHandler _clientCommandHandler = clientCommandHandler;

        //==================================================================================================================================================
        //USE CASE COMMAND
        public async Task<CreateClientDto> CreateClientAsync(CreateClientCommand command, CancellationToken cancellationToken = default)
        {
            var clientCode = new GetClientByCodeQuery { Code = command.Code };
            var existingClient = await _clientQueryHandler.GetClientByCodeAsync(clientCode, cancellationToken);
            if (existingClient != null) throw new ClientAlreadyExistsException(command.Code);
            var newClient = await _clientCommandHandler.CreateClientAsync(command, cancellationToken);
            var newClientDto = ClientCommandHandler.ConvertClientEntitesToCreateDto(newClient);
            return newClientDto;
        }

        public async Task<RenameClientDto> RenameClientAsync(RenameClientCommand command, CancellationToken cancellationToken = default)
        {
            var queryCommand = new GetClientByIdQuery { ClientId = command.ClientId };
            var client = await _clientQueryHandler.GetClientByIdAsync(queryCommand, cancellationToken);
            if (client != null)
            {
                await _clientCommandHandler.RenameClientAsync(client, command.NewName);
                return ClientCommandHandler.ConvertClientEntitesToRenameDto(client);
            }
            throw new ClientNotFoundException(command.ClientId);
        }

        public async Task<RenameClientCodeDto> RenameCodeClientAsync(RenameCodeClientCommand command, CancellationToken cancellationToken = default)
        {
            var queryCommand = new GetClientByIdQuery { ClientId = command.ClientId };
            var client = await _clientQueryHandler.GetClientByIdAsync(queryCommand, cancellationToken);
            if (client != null)
            {
                var clientCode = new GetClientByCodeQuery { Code = command.NewCode };
                var existingClientWithNewCode = await _clientQueryHandler.GetClientByCodeAsync(clientCode, cancellationToken);
                if (existingClientWithNewCode != null) throw new ClientAlreadyExistsException(command.NewCode);
                await _clientCommandHandler.RenameCodeClientAsync(client, command.NewCode);
                return ClientCommandHandler.ConvertClientEntitesToRenameCodeDto(client);
            }
            throw new ClientNotFoundException(command.ClientId);
        }

        public async Task<ActivateClientDto> ActivateClientAsync(ActivateClientCommand command, CancellationToken cancellationToken = default)
        {
            var queryCommand = new GetClientByIdQuery { ClientId = command.ClientId };
            var client = await _clientQueryHandler.GetClientByIdAsync(queryCommand, cancellationToken);
            if (client != null)
            {
                await _clientCommandHandler.ActivateClientAsync(client);
                return ClientCommandHandler.ConvertClientEntitesToActivateDto(client);
            }
            throw new ClientNotFoundException(command.ClientId);
        }

        public async Task<DeactivateClientDto> DeactivateClientAsync(DeactivateClientCommand command, CancellationToken cancellationToken = default)
        {
            var queryCommand = new GetClientByIdQuery { ClientId = command.ClientId };
            var client = await _clientQueryHandler.GetClientByIdAsync(queryCommand, cancellationToken);
            if (client != null)
            {
                await _clientCommandHandler.DeactivateClientAsync(client);
                return ClientCommandHandler.ConvertClientEntitesToDeactivateDto(client);
            }
            throw new ClientNotFoundException(command.ClientId);
        }

        public async Task<DeleteClientDto> DeletedClientAsync(DeletedClientCommand command, CancellationToken cancellationToken = default)
        {
            var queryCommand = new GetClientByIdQuery { ClientId = command.ClientId };
            var client = await _clientQueryHandler.GetClientByIdAsync(queryCommand, cancellationToken);
            if (client != null)
            {
                await _clientCommandHandler.DeleteClientAsync(client);
                return ClientCommandHandler.ConvertClientEntitesToDeleteDto(client);
            }
            throw new ClientNotFoundException(command.ClientId);
        }

        public async Task<RestoreClientDto> RestoreClientAsync(RestoreClientCommand command, CancellationToken cancellationToken = default)
        {
            var queryCommand = new GetClientByIdQuery { ClientId = command.ClientId };
            var client = await _clientQueryHandler.GetClientByIdAsync(queryCommand, cancellationToken);
            if (client != null)
            {
                await _clientCommandHandler.RestoreClientAsync(client);
                return ClientCommandHandler.ConvertClientEntitesToRestoreDto(client);
            }
            throw new ClientNotFoundException(command.ClientId);
        }

        //==================================================================================================================================================
        //USE CASE QUERY

        public async Task<ClientDto> GetClientByIdAsync(GetClientByIdQuery query, CancellationToken cancellationToken = default)
        {
            var client = await _clientQueryHandler.GetClientByIdAsync(query, cancellationToken);
            if (client != null) return ClientQueryHandler.ConvertClientEntitesToDto(client);
            throw new ClientNotFoundException(query.ClientId);
        }

        public async Task<ClientDto> GetClientByCodeAsync(GetClientByCodeQuery query, CancellationToken cancellationToken = default)
        {
            var client = await _clientQueryHandler.GetClientByCodeAsync(query, cancellationToken);
            if (client != null) return ClientQueryHandler.ConvertClientEntitesToDto(client);
            throw new ClientNotFoundByCodeException(query.Code);
        }

        public async Task<ClientCodeDto> GetClientCodeByIdAsync(GetClientCodeByIdQuery query, CancellationToken cancellationToken = default)
        {
            var clientCode = await _clientQueryHandler.GetClientCodeByIdAsync(query, cancellationToken);
            if (clientCode != null) return ClientQueryHandler.ConvertClientCodeEntitesToDto(clientCode);
            throw new ClientNotFoundException(query.ClientId);
        }

        public async Task<List<ClientDto>> GetAllClientsAsync(CancellationToken cancellationToken = default)
        {
            var clients = await _clientQueryHandler.GetAllAsync(cancellationToken);
            if (clients == null || clients.Count == 0) return [];
            return ClientQueryHandler.ConvertClientEntitesToDto(clients);
        }

        public async Task<List<ClientCodeDto>> GetAllClientCodesAsync(CancellationToken cancellationToken = default)
        {
            var clientCodes = await _clientQueryHandler.GetAllClientCodesAsync(cancellationToken);
            if (clientCodes == null || clientCodes.Count == 0) return [];
            return ClientQueryHandler.ConvertClientCodeEntitesToDto(clientCodes);
        }

        public async Task<List<ClientDto>> GetActiveClientsAsync(CancellationToken cancellationToken = default)
        {
            var clients = await _clientQueryHandler.GetActiveClientsAsync(cancellationToken);
            if (clients == null || clients.Count == 0) return [];
            return ClientQueryHandler.ConvertClientEntitesToDto(clients);
        }

        public async Task<List<ClientDto>> GetNonActiveClientsAsync(CancellationToken cancellationToken = default)
        {
            var clients = await _clientQueryHandler.GetNonActiveClientsAsync(cancellationToken);
            if (clients == null || clients.Count == 0) return [];
            return ClientQueryHandler.ConvertClientEntitesToDto(clients);
        }

        public async Task<List<ClientDto>> GetDeletedClientsAsync(CancellationToken cancellationToken = default)
        {
            var clients = await _clientQueryHandler.GetDeletedClientsAsync(cancellationToken);
            if (clients == null || clients.Count == 0) return [];
            return ClientQueryHandler.ConvertClientEntitesToDto(clients);
        }
    }
}
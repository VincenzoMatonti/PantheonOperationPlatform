using Hermes.Application.Clients.Queries;
using Hermes.Application.Clients.Commands;
using Hermes.Application.Clients.DTOs.ClientDTOs;
using Hermes.Application.Clients.DTOs.ClientOperationDtos;
using Hermes.Application.Clients.Exceptions;

namespace Hermes.Application.Clients.UseCases
{
    public class ClientOperationUseCaseHandler(
        ClientOperationQueryHandler clientOperationQueryHandler,
        ClientOperationCommandHandler clientOperationCommandHandler,
        ClientQueryHandler clientQueryHandler)
    {
        private readonly ClientOperationQueryHandler _clientOperationQueryHandler = clientOperationQueryHandler;
        private readonly ClientOperationCommandHandler _clientOperationCommandHandler = clientOperationCommandHandler;
        private readonly ClientQueryHandler _clientQueryHandler = clientQueryHandler;


        //==================================================================================================================================================
        //USE CASE COMMAND

        public async Task<CreateClientOperationDto> CreateClientOperationAsync(CreateClientOperationCommand command, CancellationToken cancellationToken = default)
        {
            var clientQuery = new GetClientByIdQuery { ClientId = command.ClientId };
            var client = await _clientQueryHandler.GetClientByIdAsync(clientQuery, cancellationToken);
            if (client != null)
            {
                var clientOperationQuery = new GetClientOperationQuery
                {
                    ClientId = command.ClientId,
                    OperationTypeId = command.OperationTypeId
                };
                var existingClientOperation = await _clientOperationQueryHandler.GetClientOperationAsync(clientOperationQuery, cancellationToken);
                if (existingClientOperation != null) throw new ClientOperationAlreadyExistsException(command.ClientId, command.OperationTypeId);
                var newClientOperation = await _clientOperationCommandHandler.CreateAsync(command, cancellationToken);
                var newClientOperationDto = ClientOperationCommandHandler.ConvertClientOperationEntitesToCreateDto(newClientOperation);
                return newClientOperationDto;
            }
            throw new ClientNotFoundException(command.ClientId);

        }


        public async Task<EnableClientOperationDto> EnableClientOperationAsync(EnableClientOperationCommand command, CancellationToken cancellationToken = default)
        {
            var queryCommand = new GetClientOperationByIdQuery { ClientOperationId = command.ClientOperationId };
            var clientOperation = await _clientOperationQueryHandler.GetClientOperationByIdAsync(queryCommand, cancellationToken);
            if (clientOperation != null)
            {
                await _clientOperationCommandHandler.Enable(clientOperation);
                return ClientOperationCommandHandler.ConvertClientOperationEntitesToEnableDto(clientOperation);
            }
            throw new ClientOperationNotFoundException(command.ClientOperationId);
        }



        public async Task<DisableClientOperationDto> DisableClientOperationAsync(DisableClientOperationCommand command, CancellationToken cancellationToken = default)
        {
            var queryCommand = new GetClientOperationByIdQuery { ClientOperationId = command.ClientOperationId };
            var clientOperation = await _clientOperationQueryHandler.GetClientOperationByIdAsync(queryCommand, cancellationToken);
            if (clientOperation != null)
            {
                await _clientOperationCommandHandler.Disable(clientOperation);
                return ClientOperationCommandHandler.ConvertClientOperationEntitesToDisableDto(clientOperation);
            }
            throw new ClientOperationNotFoundException(command.ClientOperationId);
        }


        public async Task<DeleteClientOperationDto> DeleteClientOperationAsync(DeleteClientOperationCommand command, CancellationToken cancellationToken = default)
        {
            var queryCommand = new GetClientOperationByIdQuery { ClientOperationId = command.ClientOperationId };
            var clientOperation = await _clientOperationQueryHandler.GetClientOperationByIdAsync(queryCommand, cancellationToken);
            if (clientOperation != null)
            {
                await _clientOperationCommandHandler.Delete(clientOperation);
                return ClientOperationCommandHandler.ConvertClientOperationEntitesToDeleteDto(clientOperation);
            }
            throw new ClientOperationNotFoundException(command.ClientOperationId);
        }


        public async Task<RestoreClientOperationDto> RestoreClientOperationAsync(RestoreClientOperationCommand command, CancellationToken cancellationToken = default)
        {
            var queryCommand = new GetClientOperationByIdQuery { ClientOperationId = command.ClientOperationId };
            var clientOperation = await _clientOperationQueryHandler.GetClientOperationByIdAsync(queryCommand, cancellationToken);
            if (clientOperation != null)
            {
                await _clientOperationCommandHandler.Restore(clientOperation);
                return ClientOperationCommandHandler.ConvertClientOperationEntitesToRestoreDto(clientOperation);
            }
            throw new ClientOperationNotFoundException(command.ClientOperationId);
        }


        //==================================================================================================================================================
        //USE CASE ONLY QUERY

        public async Task<ClientOperationDto> GetClientOperationByIdAsync(GetClientOperationByIdQuery query, CancellationToken cancellationToken = default)
        {
            var clientOperation = await _clientOperationQueryHandler.GetClientOperationByIdAsync(query, cancellationToken);
            if (clientOperation != null) return ClientOperationQueryHandler.ConvertClientOperationEntitesToDto(clientOperation);
            throw new ClientOperationNotFoundException(query.ClientOperationId);
        }


        public async Task<ClientOperationDto> GetClientOperationAsync(GetClientOperationQuery query, CancellationToken cancellationToken = default)
        {
            var clientOperation = await _clientOperationQueryHandler.GetClientOperationAsync(query, cancellationToken);
            if (clientOperation != null) return ClientOperationQueryHandler.ConvertClientOperationEntitesToDto(clientOperation);
            throw new ClientOperationNotFoundByClientAndOperationTypeException(query.ClientId, query.OperationTypeId);
        }


        public async Task<List<ClientOperationDto>> GetClientOperationsByClientIdAsync(GetClientOperationsByClientIdQuery query, CancellationToken cancellationToken = default)
        {
            var clientOperations = await _clientOperationQueryHandler.GetClientOperationsByClientIdAsync(query, cancellationToken);
            if (clientOperations == null || clientOperations.Count == 0) return [];
            return ClientOperationQueryHandler.ConvertClientOperationEntitesToDto(clientOperations);
        }


        public async Task<List<ClientOperationDto>> GetClientOperationsByOperationTypeIdAsync(GetClientOperationsByOperationTypeIdQuery query, CancellationToken cancellationToken = default)
        {
            var clientOperations = await _clientOperationQueryHandler.GetClientOperationsByOperationTypeIdAsync(query, cancellationToken);
            if (clientOperations == null || clientOperations.Count == 0) return [];
            return ClientOperationQueryHandler.ConvertClientOperationEntitesToDto(clientOperations);
        }


        public async Task<List<ClientOperationDto>> GetActiveClientOperationsAsync(CancellationToken cancellationToken = default)
        {
            var clientOperations = await _clientOperationQueryHandler.GetActiveClientOperationsAsync(cancellationToken);
            if (clientOperations == null || clientOperations.Count == 0) return [];
            return ClientOperationQueryHandler.ConvertClientOperationEntitesToDto(clientOperations);
        }


        public async Task<List<ClientOperationDto>> GetNonActiveClientOperationsAsync(CancellationToken cancellationToken = default)
        {
            var clientOperations = await _clientOperationQueryHandler.GetNonActiveClientOperationsAsync(cancellationToken);
            if (clientOperations == null || clientOperations.Count == 0) return [];
            return ClientOperationQueryHandler.ConvertClientOperationEntitesToDto(clientOperations);
        }


        public async Task<List<ClientOperationDto>> GetDeletedClientOperationsAsync(CancellationToken cancellationToken = default)
        {
            var clientOperations = await _clientOperationQueryHandler.GetDeletedClientOperationsAsync(cancellationToken);
            if (clientOperations == null || clientOperations.Count == 0) return [];
            return ClientOperationQueryHandler.ConvertClientOperationEntitesToDto(clientOperations);
        }
    }
}
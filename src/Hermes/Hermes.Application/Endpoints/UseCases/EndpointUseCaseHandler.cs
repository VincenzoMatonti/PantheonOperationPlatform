using Hermes.Application.Endpoints.Queries;
using Hermes.Application.Endpoints.Commands;
using Hermes.Application.Endpoints.DTOs.EndpointDtos;
using Hermes.Application.Endpoints.Exceptions;

namespace Hermes.Application.Endpoints.UseCases
{
    public class EndpointUseCaseHandler(EndpointQueryHandler endpointQueryHandler, EndpointCommandHandler endpointCommandHandler)
    {
        private readonly EndpointQueryHandler _endpointQueryHandler = endpointQueryHandler;
        private readonly EndpointCommandHandler _endpointCommandHandler = endpointCommandHandler;

        //==================================================================================================================================================
        //USE CASE COMMAND

        public async Task<CreateEndpointDto> CreateEndpointAsync(CreateEndpointCommand command, CancellationToken cancellationToken = default)
        {
            var endpointCode = new GetEndpointByCodeQuery { Code = command.Code };
            var existingEndpoint = await _endpointQueryHandler.GetEndpointByCodeAsync(endpointCode, cancellationToken);
            if (existingEndpoint != null) throw new EndpointAlreadyExistsByCodeException(command.Code);
            var newEndpoint = await _endpointCommandHandler.CreateEndpointAsync(command, cancellationToken);
            var newEndpointDto = EndpointCommandHandler.ConvertEndpointToCreateDto(newEndpoint);
            return newEndpointDto;
        }

        public async Task<RenameEndpointDto> RenameEndpointAsync(RenameEndpointCommand command, CancellationToken cancellationToken = default)
        {
            var queryCommand = new GetEndpointByIdQuery { EndpointId = command.EndpointId };
            var endpoint = await _endpointQueryHandler.GetEndpointByIdAsync(queryCommand, cancellationToken);
            if (endpoint != null)
            {
                var endpointType = new GetEndpointByTypeQuery { Type = command.NewType };
                var existingEndpointWithNewType = await _endpointQueryHandler.GetEndpointByTypeAsync(endpointType, cancellationToken);
                if (existingEndpointWithNewType != null) throw new EndpointAlreadyExistsByTypeException(command.NewType);
                await _endpointCommandHandler.RenameEndpointAsync(endpoint, command.NewType, cancellationToken);
                await _endpointCommandHandler.RenameEndpointAsync(endpoint, command.NewType, cancellationToken);
                return EndpointCommandHandler.ConvertEndpointToRenameDto(endpoint);
            }
            throw new EndpointNotFoundException(command.EndpointId);
        }

        public async Task<RenameCodeEndpointDto> RenameCodeEndpointAsync(RenameCodeEndpointCommand command, CancellationToken cancellationToken = default)
        {
            var queryCommand = new GetEndpointByIdQuery { EndpointId = command.EndpointId };
            var endpoint = await _endpointQueryHandler.GetEndpointByIdAsync(queryCommand, cancellationToken);
            if (endpoint != null)
            {
                var endpointCode = new GetEndpointByCodeQuery { Code = command.NewCode };
                var existingEndpointWithNewCode = await _endpointQueryHandler.GetEndpointByCodeAsync(endpointCode, cancellationToken);
                if (existingEndpointWithNewCode != null) throw new EndpointAlreadyExistsByCodeException(command.NewCode);
                await _endpointCommandHandler.RenameCodeEndpointAsync(endpoint, command.NewCode, cancellationToken);
                return EndpointCommandHandler.ConvertEndpointToRenameCodeDto(endpoint);
            }
            throw new EndpointNotFoundException(command.EndpointId);
        }

        public async Task<ActivateEndpointDto> ActivateEndpointAsync(ActivateEndpointCommand command, CancellationToken cancellationToken = default)
        {
            var queryCommand = new GetEndpointByIdQuery { EndpointId = command.EndpointId };
            var endpoint = await _endpointQueryHandler.GetEndpointByIdAsync(queryCommand, cancellationToken);
            if (endpoint != null)
            {
                await _endpointCommandHandler.ActivateEndpointAsync(endpoint);
                return EndpointCommandHandler.ConvertEndpointToActivateDto(endpoint);
            }
            throw new EndpointNotFoundException(command.EndpointId);
        }

        public async Task<DeactivateEndpointDto> DeactivateEndpointAsync(DeactivateEndpointCommand command, CancellationToken cancellationToken = default)
        {
            var queryCommand = new GetEndpointByIdQuery { EndpointId = command.EndpointId };
            var endpoint = await _endpointQueryHandler.GetEndpointByIdAsync(queryCommand, cancellationToken);
            if (endpoint != null)
            {
                await _endpointCommandHandler.DeactivateEndpointAsync(endpoint);
                return EndpointCommandHandler.ConvertEndpointToDeactivateDto(endpoint);
            }
            throw new EndpointNotFoundException(command.EndpointId);
        }

        public async Task<DeleteEndpointDto> DeletedEndpointAsync(DeleteEndpointCommand command, CancellationToken cancellationToken = default)
        {
            var queryCommand = new GetEndpointByIdQuery { EndpointId = command.EndpointId };
            var endpoint = await _endpointQueryHandler.GetEndpointByIdAsync(queryCommand, cancellationToken);
            if (endpoint != null)
            {
                await _endpointCommandHandler.DeleteEndpointAsync(endpoint);
                return EndpointCommandHandler.ConvertEndpointToDeleteDto(endpoint);
            }
            throw new EndpointNotFoundException(command.EndpointId);
        }

        public async Task<RestoreEndpointDto> RestoreEndpointAsync(RestoreEndpointCommand command, CancellationToken cancellationToken = default)
        {
            var queryCommand = new GetEndpointByIdQuery { EndpointId = command.EndpointId };
            var endpoint = await _endpointQueryHandler.GetEndpointByIdAsync(queryCommand, cancellationToken);
            if (endpoint != null)
            {
                await _endpointCommandHandler.RestoreEndpointAsync(endpoint);
                return EndpointCommandHandler.ConvertEndpointToRestoreDto(endpoint);
            }
            throw new EndpointNotFoundException(command.EndpointId);
        }

        //==================================================================================================================================================
        //USE CASE QUERY

        public async Task<EndpointDto> GetEndpointByIdAsync(GetEndpointByIdQuery query, CancellationToken cancellationToken = default)
        {
            var endpoint = await _endpointQueryHandler.GetEndpointByIdAsync(query, cancellationToken);
            if (endpoint != null) return EndpointQueryHandler.ConvertEndpointEntitesToDto(endpoint);
            throw new EndpointNotFoundException(query.EndpointId);
        }

        public async Task<EndpointDto> GetEndpointByCodeAsync(GetEndpointByCodeQuery query, CancellationToken cancellationToken = default)
        {
            var endpoint = await _endpointQueryHandler.GetEndpointByCodeAsync(query, cancellationToken);
            if (endpoint != null) return EndpointQueryHandler.ConvertEndpointEntitesToDto(endpoint);
            throw new EndpointNotFoundByCodeException(query.Code);
        }

        public async Task<EndpointDto> GetEndpointByTypeAsync(GetEndpointByTypeQuery query, CancellationToken cancellationToken = default)
        {
            var endpoint = await _endpointQueryHandler.GetEndpointByTypeAsync(query, cancellationToken);
            if (endpoint != null) return EndpointQueryHandler.ConvertEndpointEntitesToDto(endpoint);
            throw new EndpointNotFoundByTypeException(query.Type);
        }

        public async Task<EndpointCodeDto> GetEndpointCodeByIdAsync(GetEndpointCodeByIdQuery query, CancellationToken cancellationToken = default)
        {
            var endpointCode = await _endpointQueryHandler.GetEndpointCodeByIdAsync(query, cancellationToken);
            if (endpointCode != null) return EndpointQueryHandler.ConvertEndpointCodeEntitesToDto(endpointCode);
            throw new EndpointNotFoundException(query.EndpointId);
        }

        public async Task<List<EndpointDto>> GetAllEndpointsAsync(CancellationToken cancellationToken = default)
        {
            var endpoints = await _endpointQueryHandler.GetAllAsync(cancellationToken);
            if (endpoints == null || endpoints.Count == 0) return [];
            return EndpointQueryHandler.ConvertEndpointEntitesToDto(endpoints);
        }

        public async Task<List<EndpointCodeDto>> GetAllEndpointCodesAsync(CancellationToken cancellationToken = default)
        {
            var endpointCodes = await _endpointQueryHandler.GetAllEndpointCodesAsync(cancellationToken);
            if (endpointCodes == null || endpointCodes.Count == 0) return [];
            return EndpointQueryHandler.ConvertEndpointCodeEntitesToDto(endpointCodes);
        }

        public async Task<List<EndpointDto>> GetActiveEndpointsAsync(CancellationToken cancellationToken = default)
        {
            var endpoints = await _endpointQueryHandler.GetActiveEndpointsAsync(cancellationToken);
            if (endpoints == null || endpoints.Count == 0) return [];
            return EndpointQueryHandler.ConvertEndpointEntitesToDto(endpoints);
        }

        public async Task<List<EndpointDto>> GetNonActiveEndpointsAsync(CancellationToken cancellationToken = default)
        {
            var endpoints = await _endpointQueryHandler.GetNonActiveEndpointsAsync(cancellationToken);
            if (endpoints == null || endpoints.Count == 0) return [];
            return EndpointQueryHandler.ConvertEndpointEntitesToDto(endpoints);
        }

        public async Task<List<EndpointDto>> GetDeletedEndpointsAsync(CancellationToken cancellationToken = default)
        {
            var endpoints = await _endpointQueryHandler.GetDeletedEndpointsAsync(cancellationToken);
            if (endpoints == null || endpoints.Count == 0) return [];
            return EndpointQueryHandler.ConvertEndpointEntitesToDto(endpoints);
        }
    }
}


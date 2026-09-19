using Hermes.Application.Endpoints.Commands;
using Hermes.Application.Endpoints.DTOs.EndpointOperationDtos;
using Hermes.Application.Endpoints.Exceptions;
using Hermes.Application.Endpoints.Queries;

namespace Hermes.Application.Endpoints.UseCases;

public class EndpointOperationUseCaseHandler(
    EndpointOperationQueryHandler endpointOperationQueryHandler,
    EndpointOperationCommandHandler endpointOperationCommandHandler)
{
    private readonly EndpointOperationQueryHandler _endpointOperationQueryHandler = endpointOperationQueryHandler;
    private readonly EndpointOperationCommandHandler _endpointOperationCommandHandler = endpointOperationCommandHandler;

    //==================================================================================================================================================
    //USE CASE COMMAND

    public async Task<CreateEndpointOperationDto> CreateEndpointOperationAsync(CreateEndpointOperationCommand command, CancellationToken cancellationToken = default)
    {
        var queryCommand = new GetEndpointOperationQuery { EndpointId = command.EndpointId, OperationTypeId = command.OperationTypeId };
        var existingEndpointOperation = await _endpointOperationQueryHandler.GetEndpointOperationAsync(queryCommand, cancellationToken);
        if (existingEndpointOperation != null) throw new EndpointOperationAlreadyExistsByEndpointAndOperationTypeException(command.EndpointId, command.OperationTypeId);
        var newEndpointOperation = await _endpointOperationCommandHandler.CreateEndpointOperationAsync(command, cancellationToken);
        return EndpointOperationCommandHandler.ConvertEndpointOperationToCreateDto(newEndpointOperation);
    }

    public async Task<EnableEndpointOperationDto> EnableEndpointOperationAsync(EnableEndpointOperationCommand command, CancellationToken cancellationToken = default)
    {
        var queryCommand = new GetEndpointOperationByIdQuery { EndpointOperationId = command.EndpointOperationId };
        var endpointOperation = await _endpointOperationQueryHandler.GetEndpointOperationByIdAsync(queryCommand, cancellationToken);
        if (endpointOperation != null)
        {
            await _endpointOperationCommandHandler.EnableEndpointOperationAsync(endpointOperation);
            return EndpointOperationCommandHandler.ConvertEndpointOperationToEnableDto(endpointOperation);
        }
        throw new EndpointOperationNotFoundException(command.EndpointOperationId);
    }

    public async Task<DisableEndpointOperationDto> DisableEndpointOperationAsync(DisableEndpointOperationCommand command, CancellationToken cancellationToken = default)
    {
        var queryCommand = new GetEndpointOperationByIdQuery { EndpointOperationId = command.EndpointOperationId };
        var endpointOperation = await _endpointOperationQueryHandler.GetEndpointOperationByIdAsync(queryCommand, cancellationToken);
        if (endpointOperation != null)
        {
            await _endpointOperationCommandHandler.DisableEndpointOperationAsync(endpointOperation);
            return EndpointOperationCommandHandler.ConvertEndpointOperationToDisableDto(endpointOperation);
        }
        throw new EndpointOperationNotFoundException(command.EndpointOperationId);
    }

    public async Task<DeleteEndpointOperationDto> DeleteEndpointOperationAsync(DeleteEndpointOperationCommand command, CancellationToken cancellationToken = default)
    {
        var queryCommand = new GetEndpointOperationByIdQuery { EndpointOperationId = command.EndpointOperationId };
        var endpointOperation = await _endpointOperationQueryHandler.GetEndpointOperationByIdAsync(queryCommand, cancellationToken);
        if (endpointOperation != null)
        {
            await _endpointOperationCommandHandler.DeleteEndpointOperationAsync(endpointOperation);
            return EndpointOperationCommandHandler.ConvertEndpointOperationToDeleteDto(endpointOperation);
        }
        throw new EndpointOperationNotFoundException(command.EndpointOperationId);
    }

    public async Task<RestoreEndpointOperationDto> RestoreEndpointOperationAsync(RestoreEndpointOperationCommand command, CancellationToken cancellationToken = default)
    {
        var queryCommand = new GetEndpointOperationByIdQuery { EndpointOperationId = command.EndpointOperationId };
        var endpointOperation = await _endpointOperationQueryHandler.GetEndpointOperationByIdAsync(queryCommand, cancellationToken);
        if (endpointOperation != null)
        {
            await _endpointOperationCommandHandler.RestoreEndpointOperationAsync(endpointOperation);
            return EndpointOperationCommandHandler.ConvertEndpointOperationToRestoreDto(endpointOperation);
        }
        throw new EndpointOperationNotFoundException(command.EndpointOperationId);
    }

    //==================================================================================================================================================
    //USE CASE QUERY

    public async Task<EndpointOperationDto> GetEndpointOperationByIdAsync(GetEndpointOperationByIdQuery query, CancellationToken cancellationToken = default)
    {
        var endpointOperation = await _endpointOperationQueryHandler.GetEndpointOperationByIdAsync(query, cancellationToken);
        if (endpointOperation != null) return EndpointOperationQueryHandler.ConvertEndpointOperationEntitesToDto(endpointOperation);
        throw new EndpointOperationNotFoundException(query.EndpointOperationId);
    }

    public async Task<EndpointOperationDto> GetEndpointOperationAsync(GetEndpointOperationQuery query, CancellationToken cancellationToken = default)
    {
        var endpointOperation = await _endpointOperationQueryHandler.GetEndpointOperationAsync(query, cancellationToken);
        if (endpointOperation != null) return EndpointOperationQueryHandler.ConvertEndpointOperationEntitesToDto(endpointOperation);
        throw new EndpointOperationNotFoundByEndpointAndOperationTypeException(query.EndpointId, query.OperationTypeId);
    }

    public async Task<List<EndpointOperationDto>> GetEndpointOperationsByEndpointIdAsync(GetEndpointOperationsByEndpointIdQuery query, CancellationToken cancellationToken = default)
    {
        var endpointOperations = await _endpointOperationQueryHandler.GetEndpointOperationsByEndpointIdAsync(query, cancellationToken);
        if (endpointOperations == null || endpointOperations.Count == 0) return [];
        return EndpointOperationQueryHandler.ConvertEndpointOperationEntitesToDto(endpointOperations);
    }

    public async Task<List<EndpointOperationDto>> GetAllEndpointOperationsAsync(CancellationToken cancellationToken = default)
    {
        var endpointOperations = await _endpointOperationQueryHandler.GetAllAsync(cancellationToken);
        if (endpointOperations == null || endpointOperations.Count == 0) return [];
        return EndpointOperationQueryHandler.ConvertEndpointOperationEntitesToDto(endpointOperations);
    }

    public async Task<List<EndpointOperationDto>> GetActiveEndpointOperationsAsync(CancellationToken cancellationToken = default)
    {
        var endpointOperations = await _endpointOperationQueryHandler.GetActiveEndpointOperationsAsync(cancellationToken);
        if (endpointOperations == null || endpointOperations.Count == 0) return [];
        return EndpointOperationQueryHandler.ConvertEndpointOperationEntitesToDto(endpointOperations);
    }

    public async Task<List<EndpointOperationDto>> GetNonActiveEndpointOperationsAsync(CancellationToken cancellationToken = default)
    {
        var endpointOperations = await _endpointOperationQueryHandler.GetNonActiveEndpointOperationsAsync(cancellationToken);
        if (endpointOperations == null || endpointOperations.Count == 0) return [];
        return EndpointOperationQueryHandler.ConvertEndpointOperationEntitesToDto(endpointOperations);
    }

    public async Task<List<EndpointOperationDto>> GetDeletedEndpointOperationsAsync(CancellationToken cancellationToken = default)
    {
        var endpointOperations = await _endpointOperationQueryHandler.GetDeletedEndpointOperationsAsync(cancellationToken);
        if (endpointOperations == null || endpointOperations.Count == 0) return [];
        return EndpointOperationQueryHandler.ConvertEndpointOperationEntitesToDto(endpointOperations);
    }
}

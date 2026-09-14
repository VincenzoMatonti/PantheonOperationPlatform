using Hermes.Application.Operations.Commands;
using Hermes.Application.Operations.DTOs.OperationDtos;
using Hermes.Application.Operations.Exceptions;
using Hermes.Application.Operations.Queries;

namespace Hermes.Application.Operations.UseCases;

public class OperationUseCaseHandler(OperationQueryHandler operationQueryHandler, OperationCommandHandler operationCommandHandler)
{
    private readonly OperationQueryHandler _operationQueryHandler = operationQueryHandler;
    private readonly OperationCommandHandler _operationCommandHandler = operationCommandHandler;

    //==================================================================================================================================================
    //USE CASE COMMAND

    public async Task<CreateOperationDto> CreateOperationAsync(CreateOperationCommand command, CancellationToken cancellationToken = default)
    {
        var correlationIdQuery = new GetOperationByCorrelationIdQuery { CorrelationId = command.CorrelationId };
        var existingOperationByCorrelationId = await _operationQueryHandler.GetOperationByCorrelationIdAsync(correlationIdQuery, cancellationToken);
        if (existingOperationByCorrelationId != null) throw new OperationAlreadyExistsByCorrelationIdException(command.CorrelationId);
        var externalIdQuery = new GetOperationByExternalIdQuery { ExternalId = command.ExternalId };
        var existingOperationByExternalId = await _operationQueryHandler.GetOperationByExternalIdAsync(externalIdQuery, cancellationToken);
        if (existingOperationByExternalId != null) throw new OperationAlreadyExistsByExternalIdException(command.ExternalId);
        var newOperation = await _operationCommandHandler.CreateOperationAsync(command, cancellationToken);
        var newOperationDto = OperationCommandHandler.ConvertOperationEntitiesToCreateDto(newOperation);
        return newOperationDto;
    }

    public async Task<SendOperationDto> SendOperationAsync(SendOperationCommand command, CancellationToken cancellationToken = default)
    {
        var queryCommand = new GetOperationByIdQuery { OperationId = command.OperationId };
        var operation = await _operationQueryHandler.GetOperationByIdAsync(queryCommand, cancellationToken);
        if (operation != null)
        {
            await _operationCommandHandler.SendOperationAsync(operation);
            return OperationCommandHandler.ConvertOperationEntitiesToSendDto(operation);
        }
        throw new OperationNotFoundException(command.OperationId);
    }

    public async Task<DeleteOperationDto> DeleteOperationAsync(DeleteOperationCommand command, CancellationToken cancellationToken = default)
    {
        var queryCommand = new GetOperationByIdQuery { OperationId = command.OperationId };
        var operation = await _operationQueryHandler.GetOperationByIdAsync(queryCommand, cancellationToken);
        if (operation != null)
        {
            await _operationCommandHandler.DeleteOperationAsync(operation);
            return OperationCommandHandler.ConvertOperationEntitiesToDeleteDto(operation);
        }
        throw new OperationNotFoundException(command.OperationId);
    }

    public async Task<RestoreOperationDto> RestoreOperationAsync(RestoreOperationCommand command, CancellationToken cancellationToken = default)
    {
        var queryCommand = new GetOperationByIdQuery { OperationId = command.OperationId };
        var operation = await _operationQueryHandler.GetOperationByIdAsync(queryCommand, cancellationToken);
        if (operation != null)
        {
            await _operationCommandHandler.RestoreOperationAsync(operation);
            return OperationCommandHandler.ConvertOperationEntitiesToRestoreDto(operation);
        }
        throw new OperationNotFoundException(command.OperationId);
    }

    public async Task<ValidateOperationDto> ValidateOperationAsync(ValidateOperationCommand command, CancellationToken cancellationToken = default)
    {
        var queryCommand = new GetOperationByIdQuery { OperationId = command.OperationId };
        var operation = await _operationQueryHandler.GetOperationByIdAsync(queryCommand, cancellationToken);
        if (operation != null)
        {
            await _operationCommandHandler.ValidateOperationAsync(operation);
            return OperationCommandHandler.ConvertOperationEntitiesToValidateDto(operation);
        }
        throw new OperationNotFoundException(command.OperationId);
    }

    public async Task<AcceptOperationDto> AcceptOperationAsync(AcceptOperationCommand command, CancellationToken cancellationToken = default)
    {
        var queryCommand = new GetOperationByIdQuery { OperationId = command.OperationId };
        var operation = await _operationQueryHandler.GetOperationByIdAsync(queryCommand, cancellationToken);
        if (operation != null)
        {
            await _operationCommandHandler.AcceptOperationAsync(operation);
            return OperationCommandHandler.ConvertOperationEntitiesToAcceptDto(operation);
        }
        throw new OperationNotFoundException(command.OperationId);
    }

    public async Task<RejectOperationDto> RejectOperationAsync(RejectOperationCommand command, CancellationToken cancellationToken = default)
    {
        var queryCommand = new GetOperationByIdQuery { OperationId = command.OperationId };
        var operation = await _operationQueryHandler.GetOperationByIdAsync(queryCommand, cancellationToken);
        if (operation != null)
        {
            await _operationCommandHandler.RejectOperationAsync(operation);
            return OperationCommandHandler.ConvertOperationEntitiesToRejectDto(operation);
        }
        throw new OperationNotFoundException(command.OperationId);
    }

    //==================================================================================================================================================
    //USE CASE QUERY

    public async Task<OperationDto> GetOperationByIdAsync(GetOperationByIdQuery query, CancellationToken cancellationToken = default)
    {
        var operation = await _operationQueryHandler.GetOperationByIdAsync(query, cancellationToken);
        if (operation != null) return OperationQueryHandler.ConvertOperationEntitiesToDto(operation);
        throw new OperationNotFoundException(query.OperationId);
    }

    public async Task<OperationDto> GetOperationByExternalIdAsync(GetOperationByExternalIdQuery query, CancellationToken cancellationToken = default)
    {
        var operation = await _operationQueryHandler.GetOperationByExternalIdAsync(query, cancellationToken);
        if (operation != null) return OperationQueryHandler.ConvertOperationEntitiesToDto(operation);
        throw new OperationNotFoundByExternalIdException(query.ExternalId);
    }

    public async Task<OperationDto> GetOperationByCorrelationIdAsync(GetOperationByCorrelationIdQuery query, CancellationToken cancellationToken = default)
    {
        var operation = await _operationQueryHandler.GetOperationByCorrelationIdAsync(query, cancellationToken);
        if (operation != null) return OperationQueryHandler.ConvertOperationEntitiesToDto(operation);
        throw new OperationNotFoundByCorrelationIdException(query.CorrelationId);
    }

    public async Task<List<OperationDto>> GetAllOperationsAsync(CancellationToken cancellationToken = default)
    {
        var operations = await _operationQueryHandler.GetAllAsync(cancellationToken);
        if (operations == null || operations.Count == 0) return [];
        return OperationQueryHandler.ConvertOperationEntitiesToDto(operations);
    }

    public async Task<List<OperationDto>> GetDeletedOperationsAsync(CancellationToken cancellationToken = default)
    {
        var operations = await _operationQueryHandler.GetDeletedOperationsAsync(cancellationToken);
        if (operations == null || operations.Count == 0) return [];
        return OperationQueryHandler.ConvertOperationEntitiesToDto(operations);
    }

    public async Task<List<OperationDto>> GetOperationsByStatusAsync(GetOperationsByStatusQuery query, CancellationToken cancellationToken = default)
    {
        var operations = await _operationQueryHandler.GetOperationsByStatusAsync(query, cancellationToken);
        if (operations == null || operations.Count == 0) return [];
        return OperationQueryHandler.ConvertOperationEntitiesToDto(operations);
    }

    public async Task<List<OperationDto>> GetOperationsByExecutionIdAsync(GetOperationsByExecutionIdQuery query, CancellationToken cancellationToken = default)
    {
        var operations = await _operationQueryHandler.GetOperationsByExecutionIdAsync(query, cancellationToken);
        if (operations == null || operations.Count == 0) return [];
        return OperationQueryHandler.ConvertOperationEntitiesToDto(operations);
    }
}


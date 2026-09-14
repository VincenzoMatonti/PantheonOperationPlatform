using Hermes.Application.Operations.Exceptions;
using Hermes.Application.OperationTypes.Commands;
using Hermes.Application.OperationTypes.DTOs.OperationTypeDtos;
using Hermes.Application.OperationTypes.Queries;

namespace Hermes.Application.OperationTypes.UseCases;

public class OperationTypeUseCaseHandler(OperationTypeQueryHandler operationTypeQueryHandler, OperationTypeCommandHandler operationTypeCommandHandler)
{
    private readonly OperationTypeQueryHandler _operationTypeQueryHandler = operationTypeQueryHandler;
    private readonly OperationTypeCommandHandler _operationTypeCommandHandler = operationTypeCommandHandler;

    //==================================================================================================================================================
    //USE CASE COMMAND

    public async Task<CreateOperationTypeDto> CreateOperationTypeAsync(CreateOperationTypeCommand command, CancellationToken cancellationToken = default)
    {
        var codeQuery = new GetOperationTypeByCodeQuery { Code = command.Code };
        var existingOperationType = await _operationTypeQueryHandler.GetOperationTypeByCodeAsync(codeQuery, cancellationToken);
        if (existingOperationType != null) throw new OperationTypeAlreadyExistsException(command.Code);
        var newOperationType = await _operationTypeCommandHandler.CreateOperationTypeAsync(command, cancellationToken);
        var newOperationTypeDto = OperationTypeCommandHandler.ConvertOperationTypeEntitiesToCreateDto(newOperationType);
        return newOperationTypeDto;
    }

    public async Task<RenameOperationTypeDto> RenameOperationTypeAsync(RenameOperationTypeCommand command, CancellationToken cancellationToken = default)
    {
        var queryCommand = new GetOperationTypeByIdQuery { OperationTypeId = command.OperationTypeId };
        var operationType = await _operationTypeQueryHandler.GetOperationTypeByIdAsync(queryCommand, cancellationToken);
        if (operationType != null)
        {
            await _operationTypeCommandHandler.RenameOperationTypeAsync(operationType, command.Name);
            return OperationTypeCommandHandler.ConvertOperationTypeEntitiesToRenameDto(operationType);
        }
        throw new OperationTypeNotFoundException(command.OperationTypeId);
    }

    public async Task<RenameOperationTypeCodeDto> RenameOperationTypeCodeAsync(RenameOperationTypeCodeCommand command, CancellationToken cancellationToken = default)
    {
        var queryCommand = new GetOperationTypeByIdQuery { OperationTypeId = command.OperationTypeId };
        var operationType = await _operationTypeQueryHandler.GetOperationTypeByIdAsync(queryCommand, cancellationToken);
        if (operationType != null)
        {
            var codeQuery = new GetOperationTypeByCodeQuery { Code = command.Code };
            var existingOperationType = await _operationTypeQueryHandler.GetOperationTypeByCodeAsync(codeQuery, cancellationToken);
            if (existingOperationType != null && existingOperationType.Id != operationType.Id)
                throw new OperationTypeAlreadyExistsException(command.Code);

            await _operationTypeCommandHandler.RenameCodeOperationTypeAsync(operationType, command.Code);
            return OperationTypeCommandHandler.ConvertOperationTypeEntitiesToRenameCodeDto(operationType);
        }
        throw new OperationTypeNotFoundException(command.OperationTypeId);
    }

    public async Task<ActivateOperationTypeDto> ActivateOperationTypeAsync(ActivateOperationTypeCommand command, CancellationToken cancellationToken = default)
    {
        var queryCommand = new GetOperationTypeByIdQuery { OperationTypeId = command.OperationTypeId };
        var operationType = await _operationTypeQueryHandler.GetOperationTypeByIdAsync(queryCommand, cancellationToken);
        if (operationType != null)
        {
            await _operationTypeCommandHandler.ActivateOperationTypeAsync(operationType);
            return OperationTypeCommandHandler.ConvertOperationTypeEntitiesToActivateDto(operationType);
        }
        throw new OperationTypeNotFoundException(command.OperationTypeId);
    }

    public async Task<DeactivateOperationTypeDto> DeactivateOperationTypeAsync(DeactivateOperationTypeCommand command, CancellationToken cancellationToken = default)
    {
        var queryCommand = new GetOperationTypeByIdQuery { OperationTypeId = command.OperationTypeId };
        var operationType = await _operationTypeQueryHandler.GetOperationTypeByIdAsync(queryCommand, cancellationToken);
        if (operationType != null)
        {
            await _operationTypeCommandHandler.DeactivateOperationTypeAsync(operationType);
            return OperationTypeCommandHandler.ConvertOperationTypeEntitiesToDeactivateDto(operationType);
        }
        throw new OperationTypeNotFoundException(command.OperationTypeId);
    }

    public async Task<DeleteOperationTypeDto> DeleteOperationTypeAsync(DeleteOperationTypeCommand command, CancellationToken cancellationToken = default)
    {
        var queryCommand = new GetOperationTypeByIdQuery { OperationTypeId = command.OperationTypeId };
        var operationType = await _operationTypeQueryHandler.GetOperationTypeByIdAsync(queryCommand, cancellationToken);
        if (operationType != null)
        {
            await _operationTypeCommandHandler.DeleteOperationTypeAsync(operationType);
            return OperationTypeCommandHandler.ConvertOperationTypeEntitiesToDeleteDto(operationType);
        }
        throw new OperationTypeNotFoundException(command.OperationTypeId);
    }

    public async Task<RestoreOperationTypeDto> RestoreOperationTypeAsync(RestoreOperationTypeCommand command, CancellationToken cancellationToken = default)
    {
        var queryCommand = new GetOperationTypeByIdQuery { OperationTypeId = command.OperationTypeId };
        var operationType = await _operationTypeQueryHandler.GetOperationTypeByIdAsync(queryCommand, cancellationToken);
        if (operationType != null)
        {
            await _operationTypeCommandHandler.RestoreOperationTypeAsync(operationType);
            return OperationTypeCommandHandler.ConvertOperationTypeEntitiesToRestoreDto(operationType);
        }
        throw new OperationTypeNotFoundException(command.OperationTypeId);
    }

    //==================================================================================================================================================
    //USE CASE QUERY

    public async Task<OperationTypeDto> GetOperationTypeByIdAsync(GetOperationTypeByIdQuery query, CancellationToken cancellationToken = default)
    {
        var operationType = await _operationTypeQueryHandler.GetOperationTypeByIdAsync(query, cancellationToken);
        if (operationType != null) return OperationTypeQueryHandler.ConvertOperationTypeEntitiesToDto(operationType);
        throw new OperationTypeNotFoundException(query.OperationTypeId);
    }

    public async Task<OperationTypeDto> GetOperationTypeByCodeAsync(GetOperationTypeByCodeQuery query, CancellationToken cancellationToken = default)
    {
        var operationType = await _operationTypeQueryHandler.GetOperationTypeByCodeAsync(query, cancellationToken);
        if (operationType != null) return OperationTypeQueryHandler.ConvertOperationTypeEntitiesToDto(operationType);
        throw new OperationTypeNotFoundByCodeException(query.Code);
    }

    public async Task<List<OperationTypeDto>> GetAllOperationTypesAsync(CancellationToken cancellationToken = default)
    {
        var operationTypes = await _operationTypeQueryHandler.GetAllAsync(cancellationToken);
        if (operationTypes == null || operationTypes.Count == 0) return [];
        return OperationTypeQueryHandler.ConvertOperationTypeEntitiesToDto(operationTypes);
    }

    public async Task<List<OperationTypeDto>> GetActiveOperationTypesAsync(CancellationToken cancellationToken = default)
    {
        var operationTypes = await _operationTypeQueryHandler.GetActiveOperationTypesAsync(cancellationToken);
        if (operationTypes == null || operationTypes.Count == 0) return [];
        return OperationTypeQueryHandler.ConvertOperationTypeEntitiesToDto(operationTypes);
    }

    public async Task<List<OperationTypeDto>> GetNonActiveOperationTypesAsync(CancellationToken cancellationToken = default)
    {
        var operationTypes = await _operationTypeQueryHandler.GetNonActiveOperationTypesAsync(cancellationToken);
        if (operationTypes == null || operationTypes.Count == 0) return [];
        return OperationTypeQueryHandler.ConvertOperationTypeEntitiesToDto(operationTypes);
    }

    public async Task<List<OperationTypeDto>> GetDeletedOperationTypesAsync(CancellationToken cancellationToken = default)
    {
        var operationTypes = await _operationTypeQueryHandler.GetDeletedOperationTypesAsync(cancellationToken);
        if (operationTypes == null || operationTypes.Count == 0) return [];
        return OperationTypeQueryHandler.ConvertOperationTypeEntitiesToDto(operationTypes);
    }
}
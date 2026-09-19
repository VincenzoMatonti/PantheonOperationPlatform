using Hermes.Application.OperationTypes.DTOs.OperationTypeDtos;
using Hermes.Domain.Operations.Entities;
using Hermes.Domain.Operations.Repositories.OperationTypeRepositories;
using Hermes.Domain.Operations.ValueObjects;

namespace Hermes.Application.OperationTypes.Commands;

public class OperationTypeCommandHandler(IOperationTypeCommandRepository operationTypeCommandRepository)
{
    private readonly IOperationTypeCommandRepository _operationTypeCommandRepository = operationTypeCommandRepository;

    public async Task<OperationType> CreateOperationTypeAsync(CreateOperationTypeCommand command, CancellationToken cancellationToken = default)
    {
        var code = OperationTypeCode.Create(command.Code);
        var operationType = OperationType.Create(code, command.Name);
        await _operationTypeCommandRepository.AddAsync(operationType, cancellationToken);
        return operationType;
    }

    public async Task RenameOperationTypeAsync(OperationType operationType, string newName)
    {
        operationType.Rename(newName);
        _operationTypeCommandRepository.Update(operationType);
        await Task.CompletedTask;
    }

    public async Task RenameCodeOperationTypeAsync(OperationType operationType, string newCode)
    {
        var code = OperationTypeCode.Create(newCode);
        operationType.RenameCode(code);
        _operationTypeCommandRepository.Update(operationType);
        await Task.CompletedTask;
    }

    public async Task ActivateOperationTypeAsync(OperationType operationType)
    {
        operationType.Activate();
        _operationTypeCommandRepository.Update(operationType);
        await Task.CompletedTask;
    }

    public async Task DeactivateOperationTypeAsync(OperationType operationType)
    {
        operationType.Deactivate();
        _operationTypeCommandRepository.Update(operationType);
        await Task.CompletedTask;
    }

    public async Task DeleteOperationTypeAsync(OperationType operationType)
    {
        operationType.MarkAsDeleted();
        _operationTypeCommandRepository.Update(operationType);
        await Task.CompletedTask;
    }

    public async Task RestoreOperationTypeAsync(OperationType operationType)
    {
        operationType.Restore();
        _operationTypeCommandRepository.Update(operationType);
        await Task.CompletedTask;
    }

    public static CreateOperationTypeDto ConvertOperationTypeEntitiesToCreateDto(OperationType operationType)
    {
        return new CreateOperationTypeDto
        {
            Id = operationType.Id,
            Code = operationType.Code.Value,
            Name = operationType.Name,
            IsActive = operationType.IsActive,
            CreatedAt = operationType.CreatedAt
        };
    }

    public static List<CreateOperationTypeDto> ConvertOperationTypeEntitiesToCreateDto(List<OperationType> operationTypes)
    {
        return [.. operationTypes.Select(ConvertOperationTypeEntitiesToCreateDto)];
    }

    public static RenameOperationTypeDto ConvertOperationTypeEntitiesToRenameDto(OperationType operationType)
    {
        return new RenameOperationTypeDto
        {
            Id = operationType.Id,
            Code = operationType.Code.Value,
            Name = operationType.Name,
            UpdatedAt = operationType.UpdatedAt
        };
    }

    public static List<RenameOperationTypeDto> ConvertOperationTypeEntitiesToRenameDto(List<OperationType> operationTypes)
    {
        return [.. operationTypes.Select(ConvertOperationTypeEntitiesToRenameDto)];
    }

    public static RenameOperationTypeCodeDto ConvertOperationTypeEntitiesToRenameCodeDto(OperationType operationType)
    {
        return new RenameOperationTypeCodeDto
        {
            Id = operationType.Id,
            Code = operationType.Code.Value,
            Name = operationType.Name,
            UpdatedAt = operationType.UpdatedAt
        };
    }

    public static List<RenameOperationTypeCodeDto> ConvertOperationTypeEntitiesToRenameCodeDto(List<OperationType> operationTypes)
    {
        return [.. operationTypes.Select(ConvertOperationTypeEntitiesToRenameCodeDto)];
    }

    public static ActivateOperationTypeDto ConvertOperationTypeEntitiesToActivateDto(OperationType operationType)
    {
        return new ActivateOperationTypeDto
        {
            Id = operationType.Id,
            Code = operationType.Code.Value,
            Name = operationType.Name,
            IsActive = operationType.IsActive,
            UpdatedAt = operationType.UpdatedAt
        };
    }

    public static List<ActivateOperationTypeDto> ConvertOperationTypeEntitiesToActivateDto(List<OperationType> operationTypes)
    {
        return [.. operationTypes.Select(ConvertOperationTypeEntitiesToActivateDto)];
    }

    public static DeactivateOperationTypeDto ConvertOperationTypeEntitiesToDeactivateDto(OperationType operationType)
    {
        return new DeactivateOperationTypeDto
        {
            Id = operationType.Id,
            Code = operationType.Code.Value,
            Name = operationType.Name,
            IsActive = operationType.IsActive,
            UpdatedAt = operationType.UpdatedAt
        };
    }

    public static List<DeactivateOperationTypeDto> ConvertOperationTypeEntitiesToDeactivateDto(
        List<OperationType> operationTypes)
    {
        return [.. operationTypes.Select(ConvertOperationTypeEntitiesToDeactivateDto)];
    }

    public static DeleteOperationTypeDto ConvertOperationTypeEntitiesToDeleteDto(OperationType operationType)
    {
        return new DeleteOperationTypeDto
        {
            Id = operationType.Id,
            Code = operationType.Code.Value,
            Name = operationType.Name,
            IsDeleted = operationType.IsDeleted,
            UpdatedAt = operationType.UpdatedAt
        };
    }

    public static List<DeleteOperationTypeDto> ConvertOperationTypeEntitiesToDeleteDto(List<OperationType> operationTypes)
    {
        return [.. operationTypes.Select(ConvertOperationTypeEntitiesToDeleteDto)];
    }

    public static RestoreOperationTypeDto ConvertOperationTypeEntitiesToRestoreDto(OperationType operationType)
    {
        return new RestoreOperationTypeDto
        {
            Id = operationType.Id,
            Code = operationType.Code.Value,
            Name = operationType.Name,
            IsDeleted = operationType.IsDeleted,
            UpdatedAt = operationType.UpdatedAt
        };
    }

    public static List<RestoreOperationTypeDto> ConvertOperationTypeEntitiesToRestoreDto(
        List<OperationType> operationTypes)
    {
        return [.. operationTypes.Select(ConvertOperationTypeEntitiesToRestoreDto)];
    }
}
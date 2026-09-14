using Hermes.Application.Operations.DTOs.OperationDtos;
using Hermes.Application.Operations.Exceptions;
using Hermes.Domain.Operations.Entities;
using Hermes.Domain.Operations.Repositories.OperationRepositories;
using Hermes.Domain.Operations.ValueObjects;

namespace Hermes.Application.Operations.Commands;

public class OperationCommandHandler(
    IOperationCommandRepository operationCommandRepository,
    IOperationQueryRepository operationQueryRepository)
{
    private readonly IOperationCommandRepository _operationCommandRepository = operationCommandRepository;
    private readonly IOperationQueryRepository _operationQueryRepository = operationQueryRepository;

    public async Task<Operation> CreateOperationAsync(CreateOperationCommand command, CancellationToken cancellationToken = default)
    {
        var correlationId = CorrelationId.From(command.CorrelationId);
        var externalId = ExternalOperationId.Create(command.ExternalId);
        var existingByCorrelationId = await _operationQueryRepository.GetByCorrelationIdAsync(correlationId, cancellationToken);
        if (existingByCorrelationId is not null) throw new OperationAlreadyExistsByCorrelationIdException(command.CorrelationId);
        var existingByExternalId = await _operationQueryRepository.GetByExternalIdAsync(externalId, cancellationToken);
        if (existingByExternalId is not null) throw new OperationAlreadyExistsByExternalIdException(command.ExternalId);
        var operation = Operation.Create(command.ExecutionId, correlationId, externalId);
        await _operationCommandRepository.AddAsync(operation, cancellationToken);
        return operation;
    }

    public async Task SendOperationAsync(Operation operation)
    {
        operation.Send();
        _operationCommandRepository.Update(operation);
    }

    public async Task ValidateOperationAsync(Operation operation)
    {
        operation.Validate();
        _operationCommandRepository.Update(operation);
    }

    public async Task AcceptOperationAsync(Operation operation)
    {
        operation.Accept();
        _operationCommandRepository.Update(operation);
    }

    public async Task RejectOperationAsync(Operation operation)
    {
        operation.Reject();
        _operationCommandRepository.Update(operation);
    }

    public async Task DeleteOperationAsync(Operation operation)
    {
        operation.MarkAsDeleted();
        _operationCommandRepository.Update(operation);
    }

    public async Task RestoreOperationAsync(Operation operation)
    {
        operation.Restore();
        _operationCommandRepository.Update(operation);
    }

    public static CreateOperationDto ConvertOperationEntitiesToCreateDto(Operation operation)
    {
        return new CreateOperationDto
        {
            Id = operation.Id,
            ExecutionId = operation.ExecutionId,
            CorrelationId = operation.CorrelationId.Value,
            ExternalId = operation.ExternalId.Value,
            Status = operation.Status,
            CreatedAt = operation.CreatedAt
        };
    }

    public static List<CreateOperationDto> ConvertOperationEntitiesToCreateDto(List<Operation> operations)
    {
        return [.. operations.Select(ConvertOperationEntitiesToCreateDto)];
    }

    public static SendOperationDto ConvertOperationEntitiesToSendDto(Operation operation)
    {
        return new SendOperationDto
        {
            Id = operation.Id,
            ExecutionId = operation.ExecutionId,
            CorrelationId = operation.CorrelationId.Value,
            ExternalId = operation.ExternalId.Value,
            Status = operation.Status,
            UpdatedAt = operation.UpdatedAt
        };
    }

    public static List<SendOperationDto> ConvertOperationEntitiesToSendDto(
        List<Operation> operations)
    {
        return [.. operations.Select(ConvertOperationEntitiesToSendDto)];
    }

    public static DeleteOperationDto ConvertOperationEntitiesToDeleteDto(
        Operation operation)
    {
        return new DeleteOperationDto
        {
            Id = operation.Id,
            ExecutionId = operation.ExecutionId,
            CorrelationId = operation.CorrelationId.Value,
            ExternalId = operation.ExternalId.Value,
            Status = operation.Status,
            IsDeleted = operation.IsDeleted,
            UpdatedAt = operation.UpdatedAt
        };
    }

    public static List<DeleteOperationDto> ConvertOperationEntitiesToDeleteDto(
        List<Operation> operations)
    {
        return [.. operations.Select(ConvertOperationEntitiesToDeleteDto)];
    }

    public static RestoreOperationDto ConvertOperationEntitiesToRestoreDto(
        Operation operation)
    {
        return new RestoreOperationDto
        {
            Id = operation.Id,
            ExecutionId = operation.ExecutionId,
            CorrelationId = operation.CorrelationId.Value,
            ExternalId = operation.ExternalId.Value,
            Status = operation.Status,
            IsDeleted = operation.IsDeleted,
            UpdatedAt = operation.UpdatedAt
        };
    }

    public static List<RestoreOperationDto> ConvertOperationEntitiesToRestoreDto(
        List<Operation> operations)
    {
        return [.. operations.Select(ConvertOperationEntitiesToRestoreDto)];
    }

    public static ValidateOperationDto ConvertOperationEntitiesToValidateDto(Operation operation)
    {
        return new ValidateOperationDto
        {
            Id = operation.Id,
            ExecutionId = operation.ExecutionId,
            CorrelationId = operation.CorrelationId.Value,
            ExternalId = operation.ExternalId.Value,
            Status = operation.Status,
            UpdatedAt = operation.UpdatedAt
        };
    }

    public static List<ValidateOperationDto> ConvertOperationEntitiesToValidateDto(List<Operation> operations)
    {
        return [.. operations.Select(ConvertOperationEntitiesToValidateDto)];
    }

    public static AcceptOperationDto ConvertOperationEntitiesToAcceptDto(Operation operation)
    {
        return new AcceptOperationDto
        {
            Id = operation.Id,
            ExecutionId = operation.ExecutionId,
            CorrelationId = operation.CorrelationId.Value,
            ExternalId = operation.ExternalId.Value,
            Status = operation.Status,
            UpdatedAt = operation.UpdatedAt
        };
    }

    public static List<AcceptOperationDto> ConvertOperationEntitiesToAcceptDto(List<Operation> operations)
    {
        return [.. operations.Select(ConvertOperationEntitiesToAcceptDto)];
    }

    public static RejectOperationDto ConvertOperationEntitiesToRejectDto(Operation operation)
    {
        return new RejectOperationDto
        {
            Id = operation.Id,
            ExecutionId = operation.ExecutionId,
            CorrelationId = operation.CorrelationId.Value,
            ExternalId = operation.ExternalId.Value,
            Status = operation.Status,
            UpdatedAt = operation.UpdatedAt
        };
    }

    public static List<RejectOperationDto> ConvertOperationEntitiesToRejectDto(List<Operation> operations)
    {
        return [.. operations.Select(ConvertOperationEntitiesToRejectDto)];
    }
}
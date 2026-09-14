using Hermes.Application.Operations.DTOs.OperationDtos;
using Hermes.Domain.Operations.Entities;
using Hermes.Domain.Operations.Repositories.OperationRepositories;
using Hermes.Domain.Operations.ValueObjects;

namespace Hermes.Application.Operations.Queries;

public class OperationQueryHandler(IOperationQueryRepository operationQueryRepository)
{
    private readonly IOperationQueryRepository _operationQueryRepository = operationQueryRepository;

    public static OperationDto ConvertOperationEntitiesToDto(
        Operation operation)
    {
        return new OperationDto
        {
            Id = operation.Id,
            ExecutionId = operation.ExecutionId,
            CorrelationId = operation.CorrelationId.Value,
            ExternalId = operation.ExternalId.Value,
            Status = operation.Status
        };
    }

    public static List<OperationDto> ConvertOperationEntitiesToDto(List<Operation> operations)
    {
        return [.. operations.Select(ConvertOperationEntitiesToDto)];
    }

    public async Task<List<Operation>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _operationQueryRepository.GetAll(cancellationToken);
    }

    public async Task<List<Operation>> GetDeletedOperationsAsync(CancellationToken cancellationToken = default)
    {
        return await _operationQueryRepository.GetAllDeleted(cancellationToken);
    }

    public async Task<Operation?> GetOperationByIdAsync(GetOperationByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _operationQueryRepository.GetByIdAsync(query.OperationId, cancellationToken);
    }

    public async Task<Operation?> GetOperationByExternalIdAsync(GetOperationByExternalIdQuery query, CancellationToken cancellationToken = default)
    {
        var externalId = ExternalOperationId.Create(query.ExternalId);
        return await _operationQueryRepository.GetByExternalIdAsync(externalId, cancellationToken);
    }

    public async Task<Operation?> GetOperationByCorrelationIdAsync(GetOperationByCorrelationIdQuery query, CancellationToken cancellationToken = default)
    {
        var correlationId = CorrelationId.From(query.CorrelationId);
        return await _operationQueryRepository.GetByCorrelationIdAsync(correlationId, cancellationToken);
    }

    public async Task<List<Operation>> GetOperationsByStatusAsync(GetOperationsByStatusQuery query, CancellationToken cancellationToken = default)
    {
        return await _operationQueryRepository.GetByStatusAsync(query.Status, cancellationToken);
    }

    public async Task<List<Operation>> GetOperationsByExecutionIdAsync(GetOperationsByExecutionIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _operationQueryRepository.GetByExecutionIdAsync(query.ExecutionId, cancellationToken);
    }
}
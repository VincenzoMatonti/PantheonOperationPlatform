using Hermes.Application.OperationTypes.DTOs.OperationTypeDtos;
using Hermes.Domain.Operations.Entities;
using Hermes.Domain.Operations.Repositories.OperationTypeRepositories;

namespace Hermes.Application.OperationTypes.Queries;

public class OperationTypeQueryHandler(IOperationTypeQueryRepository operationTypeQueryRepository)
{
    private readonly IOperationTypeQueryRepository _operationTypeQueryRepository = operationTypeQueryRepository;

    public static OperationTypeDto ConvertOperationTypeEntitiesToDto(OperationType operationType)
    {
        return new OperationTypeDto
        {
            Id = operationType.Id,
            Code = operationType.Code.Value,
            Name = operationType.Name
        };
    }

    public static List<OperationTypeDto> ConvertOperationTypeEntitiesToDto(List<OperationType> operationTypes)
    {
        return [.. operationTypes.Select(ConvertOperationTypeEntitiesToDto)];
    }

    public async Task<OperationType?> GetOperationTypeByIdAsync(GetOperationTypeByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _operationTypeQueryRepository.GetByIdAsync(query.OperationTypeId, cancellationToken);
    }

    public async Task<OperationType?> GetOperationTypeByCodeAsync(GetOperationTypeByCodeQuery query, CancellationToken cancellationToken = default)
    {
        return await _operationTypeQueryRepository.GetByCodeAsync(query.Code, cancellationToken);
    }

    public async Task<List<OperationType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _operationTypeQueryRepository.GetAllAsync(cancellationToken);
    }

    public async Task<List<OperationType>> GetActiveOperationTypesAsync(CancellationToken cancellationToken = default)
    {
        return await _operationTypeQueryRepository.GetAllActive(cancellationToken);
    }

    public async Task<List<OperationType>> GetNonActiveOperationTypesAsync(CancellationToken cancellationToken = default)
    {
        return await _operationTypeQueryRepository.GetAllNonActive(cancellationToken);
    }

    public async Task<List<OperationType>> GetDeletedOperationTypesAsync(CancellationToken cancellationToken = default)
    {
        return await _operationTypeQueryRepository.GetAllDeleted(cancellationToken);
    }
}
using Hermes.Api.Operations.Responses;
using Hermes.Application.OperationTypes.DTOs.OperationTypeDtos;

namespace Hermes.Api.Operations.Mappings.Commands;

public class OperationTypeCommandResponseMapper
{
    public CreateOperationTypeResponse ToResponse(CreateOperationTypeDto operationType)
        => new()
        {
            Id = operationType.Id,
            Code = operationType.Code,
            Name = operationType.Name,
            IsActive = operationType.IsActive,
            CreatedAt = operationType.CreatedAt
        };

    public RenameOperationTypeResponse ToResponse(RenameOperationTypeDto operationType)
        => new()
        {
            Id = operationType.Id,
            Code = operationType.Code,
            Name = operationType.Name,
            UpdatedAt = operationType.UpdatedAt
        };

    public RenameOperationTypeCodeResponse ToResponse(RenameOperationTypeCodeDto operationType)
        => new()
        {
            Id = operationType.Id,
            Code = operationType.Code,
            Name = operationType.Name,
            UpdatedAt = operationType.UpdatedAt
        };

    public ActivateOperationTypeResponse ToResponse(ActivateOperationTypeDto operationType)
        => new()
        {
            Id = operationType.Id,
            Code = operationType.Code,
            Name = operationType.Name,
            IsActive = operationType.IsActive,
            UpdatedAt = operationType.UpdatedAt
        };

    public DeactivateOperationTypeResponse ToResponse(DeactivateOperationTypeDto operationType)
        => new()
        {
            Id = operationType.Id,
            Code = operationType.Code,
            Name = operationType.Name,
            IsActive = operationType.IsActive,
            UpdatedAt = operationType.UpdatedAt
        };

    public DeleteOperationTypeResponse ToResponse(DeleteOperationTypeDto operationType)
        => new()
        {
            Id = operationType.Id,
            Code = operationType.Code,
            Name = operationType.Name,
            IsDeleted = operationType.IsDeleted,
            UpdatedAt = operationType.UpdatedAt
        };

    public RestoreOperationTypeResponse ToResponse(RestoreOperationTypeDto operationType)
        => new()
        {
            Id = operationType.Id,
            Code = operationType.Code,
            Name = operationType.Name,
            IsDeleted = operationType.IsDeleted,
            UpdatedAt = operationType.UpdatedAt
        };
}


using Hermes.Api.Common;
using Hermes.Api.Operations.Mappings.Commands;
using Hermes.Api.Operations.Requests;
using Hermes.Api.Operations.Responses;
using Hermes.Application.OperationTypes.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.Api.Operations.Controllers;

[ApiController]
[Route("api/operation-type/command")]
public class OperationTypeCommandApiController(
    OperationTypeUseCaseHandler operationTypeUseCase,
    OperationTypeCommandResponseMapper responseMapper,
    OperationTypeCommandRequestMapper requestMapper) : ControllerBase
{
    private readonly OperationTypeUseCaseHandler _operationTypeUseCase = operationTypeUseCase;
    private readonly OperationTypeCommandResponseMapper _responseMapper = responseMapper;
    private readonly OperationTypeCommandRequestMapper _requestMapper = requestMapper;

    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<CreateOperationTypeResponse>>> Create(CreateOperationTypeRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _operationTypeUseCase.CreateOperationTypeAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<CreateOperationTypeResponse>.Ok(response));
    }

    [HttpPut("rename")]
    public async Task<ActionResult<ApiResponse<RenameOperationTypeResponse>>> Rename(RenameOperationTypeRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _operationTypeUseCase.RenameOperationTypeAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<RenameOperationTypeResponse>.Ok(response));
    }

    [HttpPut("rename-code")]
    public async Task<ActionResult<ApiResponse<RenameOperationTypeCodeResponse>>> RenameCode(RenameOperationTypeCodeRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _operationTypeUseCase.RenameOperationTypeCodeAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<RenameOperationTypeCodeResponse>.Ok(response));
    }

    [HttpPut("activate")]
    public async Task<ActionResult<ApiResponse<ActivateOperationTypeResponse>>> Activate(ActivateOperationTypeRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _operationTypeUseCase.ActivateOperationTypeAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<ActivateOperationTypeResponse>.Ok(response));
    }

    [HttpPut("deactivate")]
    public async Task<ActionResult<ApiResponse<DeactivateOperationTypeResponse>>> Deactivate(DeactivateOperationTypeRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _operationTypeUseCase.DeactivateOperationTypeAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<DeactivateOperationTypeResponse>.Ok(response));
    }

    [HttpDelete("delete")]
    public async Task<ActionResult<ApiResponse<DeleteOperationTypeResponse>>> Delete(DeleteOperationTypeRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _operationTypeUseCase.DeleteOperationTypeAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<DeleteOperationTypeResponse>.Ok(response));
    }

    [HttpPut("restore")]
    public async Task<ActionResult<ApiResponse<RestoreOperationTypeResponse>>> Restore(RestoreOperationTypeRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _operationTypeUseCase.RestoreOperationTypeAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<RestoreOperationTypeResponse>.Ok(response));
    }
}

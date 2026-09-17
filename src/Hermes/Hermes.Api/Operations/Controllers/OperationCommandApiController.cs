using Microsoft.AspNetCore.Mvc;
using Hermes.Api.Common;
using Hermes.Api.Operations.Requests;
using Hermes.Api.Operations.Responses;
using Hermes.Api.Operations.Mappings.Commands;
using Hermes.Application.Operations.UseCases;

namespace Hermes.Api.Operations.Controllers;

[ApiController]
[Route("api/operation/command")]
public class OperationCommandApiController(
    OperationUseCaseHandler operationUseCase,
    OperationCommandResponseMapper responseMapper,
    OperationCommandRequestMapper requestMapper) : ControllerBase
{
    private readonly OperationUseCaseHandler _operationUseCase = operationUseCase;
    private readonly OperationCommandResponseMapper _responseMapper = responseMapper;
    private readonly OperationCommandRequestMapper _requestMapper = requestMapper;

    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<CreateOperationResponse>>> CreateOperation(CreateOperationRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _operationUseCase.CreateOperationAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<CreateOperationResponse>.Ok(response));
    }

    [HttpPut("send")]
    public async Task<ActionResult<ApiResponse<SendOperationResponse>>> SendOperation(SendOperationRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _operationUseCase.SendOperationAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<SendOperationResponse>.Ok(response));
    }

    [HttpPut("validate")]
    public async Task<ActionResult<ApiResponse<ValidateOperationResponse>>> ValidateOperation(ValidateOperationRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _operationUseCase.ValidateOperationAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<ValidateOperationResponse>.Ok(response));
    }

    [HttpPut("accept")]
    public async Task<ActionResult<ApiResponse<AcceptOperationResponse>>> AcceptOperation(AcceptOperationRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _operationUseCase.AcceptOperationAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<AcceptOperationResponse>.Ok(response));
    }

    [HttpPut("reject")]
    public async Task<ActionResult<ApiResponse<RejectOperationResponse>>> RejectOperation(RejectOperationRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _operationUseCase.RejectOperationAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<RejectOperationResponse>.Ok(response));
    }

    [HttpDelete("delete")]
    public async Task<ActionResult<ApiResponse<DeleteOperationResponse>>> DeleteOperation(DeleteOperationRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _operationUseCase.DeleteOperationAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<DeleteOperationResponse>.Ok(response));
    }

    [HttpPut("restore")]
    public async Task<ActionResult<ApiResponse<RestoreOperationResponse>>> RestoreOperation(RestoreOperationRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _operationUseCase.RestoreOperationAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<RestoreOperationResponse>.Ok(response));
    }
}
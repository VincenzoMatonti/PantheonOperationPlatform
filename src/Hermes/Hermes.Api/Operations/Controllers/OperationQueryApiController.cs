using Microsoft.AspNetCore.Mvc;
using Hermes.Api.Common;
using Hermes.Api.Operations.Requests;
using Hermes.Api.Operations.Responses;
using Hermes.Api.Operations.Mappings.Queries;
using Hermes.Application.Operations.UseCases;

namespace Hermes.Api.Operations.Controllers;

[ApiController]
[Route("api/operation/query")]
public class OperationQueryApiController(
    OperationUseCaseHandler operationUseCase,
    OperationQueryResponseMapper responseMapper,
    OperationQueryRequestMapper requestMapper) : ControllerBase
{
    private readonly OperationUseCaseHandler _operationUseCase = operationUseCase;
    private readonly OperationQueryResponseMapper _responseMapper = responseMapper;
    private readonly OperationQueryRequestMapper _requestMapper = requestMapper;

    [HttpGet("all")]
    public async Task<ActionResult<ApiResponse<List<OperationResponse>>>> GetAllOperations(CancellationToken cancellationToken)
    {
        var result = await _operationUseCase.GetAllOperationsAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<OperationResponse>>.Ok(response));
    }

    [HttpGet("deleted")]
    public async Task<ActionResult<ApiResponse<List<OperationResponse>>>> GetDeletedOperations(CancellationToken cancellationToken)
    {
        var result = await _operationUseCase.GetDeletedOperationsAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<OperationResponse>>.Ok(response));
    }

    [HttpGet("by-id")]
    public async Task<ActionResult<ApiResponse<OperationResponse>>> GetOperationById(GetOperationByIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _operationUseCase.GetOperationByIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<OperationResponse>.Ok(response));
    }

    [HttpGet("by-external-id")]
    public async Task<ActionResult<ApiResponse<OperationResponse>>> GetOperationByExternalId(GetOperationByExternalIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _operationUseCase.GetOperationByExternalIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<OperationResponse>.Ok(response));
    }

    [HttpGet("by-correlation-id")]
    public async Task<ActionResult<ApiResponse<OperationResponse>>> GetOperationByCorrelationId(GetOperationByCorrelationIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _operationUseCase.GetOperationByCorrelationIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<OperationResponse>.Ok(response));
    }

    [HttpGet("by-status")]
    public async Task<ActionResult<ApiResponse<List<OperationResponse>>>> GetOperationsByStatus(GetOperationsByStatusRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _operationUseCase.GetOperationsByStatusAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<OperationResponse>>.Ok(response));
    }

    [HttpGet("by-execution-id")]
    public async Task<ActionResult<ApiResponse<List<OperationResponse>>>> GetOperationsByExecutionId(GetOperationsByExecutionIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _operationUseCase.GetOperationsByExecutionIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<OperationResponse>>.Ok(response));
    }
}
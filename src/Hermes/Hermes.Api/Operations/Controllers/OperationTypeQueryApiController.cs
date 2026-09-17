using Hermes.Api.Common;
using Hermes.Api.Operations.Mappings.Queries;
using Hermes.Api.Operations.Requests;
using Hermes.Api.Operations.Responses;
using Hermes.Application.OperationTypes.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.Api.Operations.Controllers;

[ApiController]
[Route("api/operation-type/query")]
public class OperationTypeQueryApiController(
    OperationTypeUseCaseHandler operationTypeUseCase,
    OperationTypeQueryResponseMapper responseMapper,
    OperationTypeQueryRequestMapper requestMapper) : ControllerBase
{
    private readonly OperationTypeUseCaseHandler _operationTypeUseCase = operationTypeUseCase;
    private readonly OperationTypeQueryResponseMapper _responseMapper = responseMapper;
    private readonly OperationTypeQueryRequestMapper _requestMapper = requestMapper;

    [HttpGet("all")]
    public async Task<ActionResult<ApiResponse<List<OperationTypeResponse>>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _operationTypeUseCase.GetAllOperationTypesAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<OperationTypeResponse>>.Ok(response));
    }

    [HttpGet("active")]
    public async Task<ActionResult<ApiResponse<List<OperationTypeResponse>>>> GetActive(CancellationToken cancellationToken)
    {
        var result = await _operationTypeUseCase.GetActiveOperationTypesAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<OperationTypeResponse>>.Ok(response));
    }

    [HttpGet("non-active")]
    public async Task<ActionResult<ApiResponse<List<OperationTypeResponse>>>> GetNonActive(CancellationToken cancellationToken)
    {
        var result = await _operationTypeUseCase.GetNonActiveOperationTypesAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<OperationTypeResponse>>.Ok(response));
    }

    [HttpGet("deleted")]
    public async Task<ActionResult<ApiResponse<List<OperationTypeResponse>>>> GetDeleted(CancellationToken cancellationToken)
    {
        var result = await _operationTypeUseCase.GetDeletedOperationTypesAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<OperationTypeResponse>>.Ok(response));
    }

    [HttpGet("by-id")]
    public async Task<ActionResult<ApiResponse<OperationTypeResponse>>> GetById(GetOperationTypeByIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _operationTypeUseCase.GetOperationTypeByIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<OperationTypeResponse>.Ok(response));
    }

    [HttpGet("by-code")]
    public async Task<ActionResult<ApiResponse<OperationTypeResponse>>> GetByCode(GetOperationTypeByCodeRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _operationTypeUseCase.GetOperationTypeByCodeAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<OperationTypeResponse>.Ok(response));
    }
}


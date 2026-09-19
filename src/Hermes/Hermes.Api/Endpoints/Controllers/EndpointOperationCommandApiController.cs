using Microsoft.AspNetCore.Mvc;
using Hermes.Api.Common;
using Hermes.Api.Endpoints.Requests;
using Hermes.Api.Endpoints.Responses;
using Hermes.Application.Endpoints.UseCases;
using Hermes.Api.Endpoints.Mappings.Commands;

namespace Hermes.Api.Endpoints.Controllers;

[ApiController]
[Route("api/endpointOperation/command")]
public class EndpointOperationCommandApiController(
    EndpointOperationUseCaseHandler endpointOperationUseCase,
    EndpointOperationResponseCommandMapping responseMapper,
    EndpointOperationRequestCommandMapping requestMapper) : ControllerBase
{
    private readonly EndpointOperationUseCaseHandler _endpointOperationUseCase = endpointOperationUseCase;
    private readonly EndpointOperationResponseCommandMapping _responseMapper = responseMapper;
    private readonly EndpointOperationRequestCommandMapping _requestMapper = requestMapper;

    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<CreateEndpointOperationResponse>>> CreateEndpointOperation(CreateEndpointOperationRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _endpointOperationUseCase.CreateEndpointOperationAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<CreateEndpointOperationResponse>.Ok(response));
    }

    [HttpPut("enable")]
    public async Task<ActionResult<ApiResponse<EnableEndpointOperationResponse>>> EnableEndpointOperation(EnableEndpointOperationRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _endpointOperationUseCase.EnableEndpointOperationAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<EnableEndpointOperationResponse>.Ok(response));
    }

    [HttpPut("disable")]
    public async Task<ActionResult<ApiResponse<DisableEndpointOperationResponse>>> DisableEndpointOperation(DisableEndpointOperationRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _endpointOperationUseCase.DisableEndpointOperationAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<DisableEndpointOperationResponse>.Ok(response));
    }

    [HttpDelete("delete")]
    public async Task<ActionResult<ApiResponse<DeleteEndpointOperationResponse>>> DeleteEndpointOperation(DeleteEndpointOperationRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _endpointOperationUseCase.DeleteEndpointOperationAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<DeleteEndpointOperationResponse>.Ok(response));
    }

    [HttpPut("restore")]
    public async Task<ActionResult<ApiResponse<RestoreEndpointOperationResponse>>> RestoreEndpointOperation(RestoreEndpointOperationRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _endpointOperationUseCase.RestoreEndpointOperationAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<RestoreEndpointOperationResponse>.Ok(response));
    }
}


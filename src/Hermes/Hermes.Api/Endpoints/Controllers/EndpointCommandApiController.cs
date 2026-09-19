using Microsoft.AspNetCore.Mvc;
using Hermes.Api.Common;
using Hermes.Api.Endpoints.Requests;
using Hermes.Api.Endpoints.Responses;
using Hermes.Application.Endpoints.UseCases;
using Hermes.Api.Endpoints.Mappings.Commands;

namespace Hermes.Api.Endpoints.Controllers;

[ApiController]
[Route("api/endpoint/command")]
public class EndpointCommandApiController(
    EndpointUseCaseHandler endpointUseCase,
    EndpointResponseCommandMapping responseMapper,
    EndpointRequestCommandMapping requestMapper) : ControllerBase
{
    private readonly EndpointUseCaseHandler _endpointUseCase = endpointUseCase;
    private readonly EndpointResponseCommandMapping _responseMapper = responseMapper;
    private readonly EndpointRequestCommandMapping _requestMapper = requestMapper;

    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<CreateEndpointResponse>>> CreateEndpoint(CreateEndpointRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _endpointUseCase.CreateEndpointAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<CreateEndpointResponse>.Ok(response));
    }

    [HttpPut("changeType")]
    public async Task<ActionResult<ApiResponse<ChangeEndpointTypeResponse>>> ChangeEndpointType(ChangeEndpointTypeRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _endpointUseCase.ChangeEndpointTypeAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<ChangeEndpointTypeResponse>.Ok(response));
    }

    [HttpPut("renameCode")]
    public async Task<ActionResult<ApiResponse<RenameCodeEndpointResponse>>> RenameCodeEndpoint(RenameCodeEndpointRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _endpointUseCase.RenameCodeEndpointAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<RenameCodeEndpointResponse>.Ok(response));
    }

    [HttpPut("activate")]
    public async Task<ActionResult<ApiResponse<ActivateEndpointResponse>>> ActivateEndpoint(ActivateEndpointRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _endpointUseCase.ActivateEndpointAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<ActivateEndpointResponse>.Ok(response));
    }

    [HttpPut("deactivate")]
    public async Task<ActionResult<ApiResponse<DeactivateEndpointResponse>>> DeactivateEndpoint(DeactivateEndpointRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _endpointUseCase.DeactivateEndpointAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<DeactivateEndpointResponse>.Ok(response));
    }

    [HttpDelete("delete")]
    public async Task<ActionResult<ApiResponse<DeleteEndpointResponse>>> DeleteEndpoint(DeleteEndpointRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _endpointUseCase.DeletedEndpointAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<DeleteEndpointResponse>.Ok(response));
    }

    [HttpPut("restore")]
    public async Task<ActionResult<ApiResponse<RestoreEndpointResponse>>> RestoreEndpoint(RestoreEndpointRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _endpointUseCase.RestoreEndpointAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<RestoreEndpointResponse>.Ok(response));
    }
}


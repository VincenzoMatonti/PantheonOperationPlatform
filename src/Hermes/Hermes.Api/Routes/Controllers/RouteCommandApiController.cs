using Microsoft.AspNetCore.Mvc;
using Hermes.Api.Common;
using Hermes.Api.Routes.Requests;
using Hermes.Api.Routes.Responses;
using Hermes.Api.Routes.Mappings.Commands;
using Hermes.Application.Routes.UseCases;

namespace Hermes.Api.Routes.Controllers;

[ApiController]
[Route("api/route/command")]
public class RouteCommandApiController(
    RouteUseCaseHandler routeUseCase,
    RouteCommandResponseMapper responseMapper,
    RouteCommandRequestMapper requestMapper) : ControllerBase
{
    private readonly RouteUseCaseHandler _routeUseCase = routeUseCase;
    private readonly RouteCommandResponseMapper _responseMapper = responseMapper;
    private readonly RouteCommandRequestMapper _requestMapper = requestMapper;

    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<CreateRouteResponse>>> CreateRoute(CreateRouteRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _routeUseCase.CreateRouteAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<CreateRouteResponse>.Ok(response));
    }

    [HttpPut("activate")]
    public async Task<ActionResult<ApiResponse<ActivateRouteResponse>>> ActivateRoute(ActivateRouteRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _routeUseCase.ActivateRouteAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<ActivateRouteResponse>.Ok(response));
    }

    [HttpPut("deactivate")]
    public async Task<ActionResult<ApiResponse<DeactivateRouteResponse>>> DeactivateRoute(DeactivateRouteRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _routeUseCase.DeactivateRouteAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<DeactivateRouteResponse>.Ok(response));
    }

    [HttpDelete("delete")]
    public async Task<ActionResult<ApiResponse<DeleteRouteResponse>>> DeleteRoute(DeleteRouteRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _routeUseCase.DeleteRouteAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<DeleteRouteResponse>.Ok(response));
    }

    [HttpPut("restore")]
    public async Task<ActionResult<ApiResponse<RestoreRouteResponse>>> RestoreRoute(RestoreRouteRequest request, CancellationToken cancellationToken)
    {
        var command = _requestMapper.ToCommand(request);
        var result = await _routeUseCase.RestoreRouteAsync(command, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<RestoreRouteResponse>.Ok(response));
    }

}

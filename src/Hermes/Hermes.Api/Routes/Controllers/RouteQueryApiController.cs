using Microsoft.AspNetCore.Mvc;
using Hermes.Api.Common;
using Hermes.Api.Routes.Requests;
using Hermes.Api.Routes.Responses;
using Hermes.Api.Routes.Mappings.Queries;
using Hermes.Application.Routes.UseCases;

namespace Hermes.Api.Routes.Controllers;

[ApiController]
[Route("api/route/query")]
public class RouteQueryApiController(
    RouteUseCaseHandler routeUseCase,
    RouteQueryResponseMapper responseMapper,
    RouteQueryRequestMapper requestMapper) : ControllerBase
{
    private readonly RouteUseCaseHandler _routeUseCase = routeUseCase;
    private readonly RouteQueryResponseMapper _responseMapper = responseMapper;
    private readonly RouteQueryRequestMapper _requestMapper = requestMapper;

    [HttpGet("getById")]
    public async Task<ActionResult<ApiResponse<RouteResponse>>> GetRouteById(GetRouteByIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _routeUseCase.GetRouteByIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<RouteResponse>.Ok(response));
    }

    [HttpGet("get")]
    public async Task<ActionResult<ApiResponse<RouteResponse>>> GetRoute(GetRouteRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _routeUseCase.GetRouteAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<RouteResponse>.Ok(response));
    }

    [HttpGet("getByClientId")]
    public async Task<ActionResult<ApiResponse<List<RouteResponse>>>> GetRoutesByClientId(GetRoutesByClientIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _routeUseCase.GetRoutesByClientIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<RouteResponse>>.Ok(response));
    }

    [HttpGet("getByOperationTypeId")]
    public async Task<ActionResult<ApiResponse<List<RouteResponse>>>> GetRoutesByOperationTypeId(GetRoutesByOperationTypeIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _routeUseCase.GetRoutesByOperationTypeIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<RouteResponse>>.Ok(response));
    }

    [HttpGet("getByEndpointId")]
    public async Task<ActionResult<ApiResponse<List<RouteResponse>>>> GetRoutesByEndpointId(GetRoutesByEndpointIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _routeUseCase.GetRoutesByEndpointIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<RouteResponse>>.Ok(response));
    }

    [HttpGet("getByClientIdAndOperationTypeId")]
    public async Task<ActionResult<ApiResponse<List<RouteResponse>>>> GetRoutesByClientIdAndOperationTypeId(GetRoutesByClientIdAndOperationTypeIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _routeUseCase.GetRoutesByClientIdAndOperationTypeIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<RouteResponse>>.Ok(response));
    }

    [HttpGet("getByClientIdAndEndpointId")]
    public async Task<ActionResult<ApiResponse<List<RouteResponse>>>> GetRoutesByClientIdAndEndpointId(GetRoutesByClientIdAndEndpointIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _routeUseCase.GetRoutesByClientIdAndEndpointIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<RouteResponse>>.Ok(response));
    }

    [HttpGet("getByOperationTypeIdAndEndpointId")]
    public async Task<ActionResult<ApiResponse<List<RouteResponse>>>> GetRoutesByOperationTypeIdAndEndpointId(GetRoutesByOperationTypeIdAndEndpointIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _routeUseCase.GetRoutesByOperationTypeIdAndEndpointIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<RouteResponse>>.Ok(response));
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<ApiResponse<List<RouteResponse>>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _routeUseCase.GetAllAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<RouteResponse>>.Ok(response));
    }

    [HttpGet("getActive")]
    public async Task<ActionResult<ApiResponse<List<RouteResponse>>>> GetActiveRoutes(CancellationToken cancellationToken)
    {
        var result = await _routeUseCase.GetActiveRoutesAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<RouteResponse>>.Ok(response));
    }

    [HttpGet("getNonActive")]
    public async Task<ActionResult<ApiResponse<List<RouteResponse>>>> GetNonActiveRoutes(CancellationToken cancellationToken)
    {
        var result = await _routeUseCase.GetNonActiveRoutesAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);

        return Ok(ApiResponse<List<RouteResponse>>.Ok(response));
    }

    [HttpGet("getDeleted")]
    public async Task<ActionResult<ApiResponse<List<RouteResponse>>>> GetDeletedRoutes(CancellationToken cancellationToken)
    {
        var result = await _routeUseCase.GetDeletedRoutesAsync(cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<RouteResponse>>.Ok(response));
    }

    [HttpGet("getActiveByClientId")]
    public async Task<ActionResult<ApiResponse<List<RouteResponse>>>> GetActiveRoutesByClientId(GetActiveRoutesByClientIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _routeUseCase.GetActiveRoutesByClientIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<RouteResponse>>.Ok(response));
    }

    [HttpGet("getNonActiveByClientId")]
    public async Task<ActionResult<ApiResponse<List<RouteResponse>>>> GetNonActiveRoutesByClientId(GetNonActiveRoutesByClientIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _routeUseCase.GetNonActiveRoutesByClientIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<RouteResponse>>.Ok(response));
    }

    [HttpGet("getDeletedByClientId")]
    public async Task<ActionResult<ApiResponse<List<RouteResponse>>>> GetDeletedRoutesByClientId(GetDeletedRoutesByClientIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _routeUseCase.GetDeletedRoutesByClientIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<RouteResponse>>.Ok(response));
    }

    [HttpGet("getActiveByOperationTypeId")]
    public async Task<ActionResult<ApiResponse<List<RouteResponse>>>> GetActiveRoutesByOperationTypeId(GetActiveRoutesByOperationTypeIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _routeUseCase.GetActiveRoutesByOperationTypeIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<RouteResponse>>.Ok(response));
    }

    [HttpGet("getNonActiveByOperationTypeId")]
    public async Task<ActionResult<ApiResponse<List<RouteResponse>>>> GetNonActiveRoutesByOperationTypeId(GetNonActiveRoutesByOperationTypeIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _routeUseCase.GetNonActiveRoutesByOperationTypeIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<RouteResponse>>.Ok(response));
    }

    [HttpGet("getDeletedByOperationTypeId")]
    public async Task<ActionResult<ApiResponse<List<RouteResponse>>>> GetDeletedRoutesByOperationTypeId(GetDeletedRoutesByOperationTypeIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _routeUseCase.GetDeletedRoutesByOperationTypeIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<RouteResponse>>.Ok(response));
    }

    [HttpGet("getActiveByEndpointId")]
    public async Task<ActionResult<ApiResponse<List<RouteResponse>>>> GetActiveRoutesByEndpointId(GetActiveRoutesByEndpointIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _routeUseCase.GetActiveRoutesByEndpointIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<RouteResponse>>.Ok(response));
    }

    [HttpGet("getNonActiveByEndpointId")]
    public async Task<ActionResult<ApiResponse<List<RouteResponse>>>> GetNonActiveRoutesByEndpointId(GetNonActiveRoutesByEndpointIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _routeUseCase.GetNonActiveRoutesByEndpointIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<RouteResponse>>.Ok(response));
    }

    [HttpGet("getDeletedByEndpointId")]
    public async Task<ActionResult<ApiResponse<List<RouteResponse>>>> GetDeletedRoutesByEndpointId(GetDeletedRoutesByEndpointIdRequest request, CancellationToken cancellationToken)
    {
        var query = _requestMapper.ToQuery(request);
        var result = await _routeUseCase.GetDeletedRoutesByEndpointIdAsync(query, cancellationToken);
        var response = _responseMapper.ToResponse(result);
        return Ok(ApiResponse<List<RouteResponse>>.Ok(response));
    }

}

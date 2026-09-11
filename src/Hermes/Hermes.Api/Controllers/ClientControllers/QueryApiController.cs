using Microsoft.AspNetCore.Mvc;
using Hermes.Application.Clients.DTOs;
using Hermes.Application.Clients.Queries;

namespace Hermes.Api.Controllers.ClientControllers;

[ApiController]
[Route("api/clients/queries")]
public class ClientsController : ControllerBase
{
    private readonly ClientQueryHandler _clientQueryHandler;

    public ClientsController(ClientQueryHandler clientQueryHandler)
    {
        _clientQueryHandler = clientQueryHandler;
    }



    [HttpGet("/all")]
    public async Task<ActionResult<List<ClientDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _clientQueryHandler.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("id/{id:guid}")]
    public async Task<ActionResult<ClientDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetClientByIdQuery
        {
            ClientId = id
        };

        var result = await _clientQueryHandler.GetClientByIdAsync(query.ClientId, cancellationToken);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("code/{code}")]
    public async Task<ActionResult<ClientDto>> GetByCode(string code, CancellationToken cancellationToken)
    {
        var query = new GetClientByCodeQuery
        {
            Code = code
        };

        var result = await _clientQueryHandler.GetClientByCodeAsync(query, cancellationToken);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
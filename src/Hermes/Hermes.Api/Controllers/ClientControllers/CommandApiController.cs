using Microsoft.AspNetCore.Mvc;
using Hermes.Application.Clients.DTOs;
using Hermes.Application.Clients.Commands;

namespace Hermes.Api.Controllers.ClientControllers;

[ApiController]
[Route("api/clients/commands")]
public class CommandApiController : ControllerBase
{
    private readonly ClientCommandHandler _clientCommandHandler;

    public CommandApiController(ClientCommandHandler clientCommandHandler)
    {
        _clientCommandHandler = clientCommandHandler;
    }

    [HttpPost("/create")]
    public async Task<ActionResult<ClientDto>> Create(CreateClientCommand command, CancellationToken cancellationToken)
    {
        var result = await _clientCommandHandler.CreateClientAsync(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("activate/{id:guid}")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken cancellationToken)
    {
        var command = new ActivateClientCommand
        {
            ClientId = id
        };

        await _clientCommandHandler.ActivateClientAsync(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("deactivate/{id:guid}")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeactivateClientCommand
        {
            ClientId = id
        };

        await _clientCommandHandler.DeactivateClientAsync(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("rename/{id:guid}")]
    public async Task<IActionResult> Rename(Guid id, RenameClientCommand command, CancellationToken cancellationToken)
    {
        command.ClientId = id;
        await _clientCommandHandler.RenameClientAsync(command, cancellationToken);
        return NoContent();
    }
}
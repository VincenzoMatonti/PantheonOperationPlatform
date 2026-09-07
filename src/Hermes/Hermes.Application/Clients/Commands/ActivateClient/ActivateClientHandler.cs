using Hermes.Domain.Clients.Repositories;

namespace Hermes.Application.Clients.Commands.ActivateClient;

public class ActivateClientHandler
{
    private readonly IClientRepository _clientRepository;

    public ActivateClientHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task HandleAsync(ActivateClientCommand command, CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByIdAsync(command.ClientId, cancellationToken);

        if (client is null)
        {
            throw new InvalidOperationException($"Client with ID '{command.ClientId}' was not found.");
        }

        client.Activate();

        _clientRepository.Update(client);
    }
}
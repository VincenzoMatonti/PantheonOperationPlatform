using Hermes.Domain.Clients.Repositories;

namespace Hermes.Application.Clients.Commands.DeactivateClient;

public class DeactivateClientHandler
{
    private readonly IClientRepository _clientRepository;

    public DeactivateClientHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task HandleAsync(DeactivateClientCommand command, CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByIdAsync(command.ClientId, cancellationToken);

        if (client is null)
        {
            throw new InvalidOperationException($"Client with ID '{command.ClientId}' was not found.");
        }

        client.Deactivate();

        _clientRepository.Update(client);
    }
}
using Hermes.Domain.Clients.Repositories;

namespace Hermes.Application.Clients.Commands.RenameClient;

public class RenameClientHandler
{
    private readonly IClientRepository _clientRepository;

    public RenameClientHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task HandleAsync(RenameClientCommand command, CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByIdAsync(command.ClientId, cancellationToken);

        if (client is null)
        {
            throw new InvalidOperationException($"Client with ID '{command.ClientId}' was not found.");
        }

        client.Rename(command.Name);

        _clientRepository.Update(client);
    }
}
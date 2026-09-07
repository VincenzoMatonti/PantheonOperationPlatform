using Hermes.Application.Clients.DTOs;
using Hermes.Domain.Clients.Repositories;
using Hermes.Domain.Clients.ValueObjects;

namespace Hermes.Application.Clients.Queries.GetClientByCode;

public class GetClientByCodeHandler
{
    private readonly IClientRepository _clientRepository;

    public GetClientByCodeHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<ClientDto?> HandleAsync(GetClientByCodeQuery query, CancellationToken cancellationToken = default)
    {
        var code = ClientCode.Create(query.Code);

        var client = await _clientRepository.GetByCodeAsync(code, cancellationToken);

        if (client is null)
        {
            return null;
        }

        return new ClientDto
        {
            Id = client.Id,
            Code = client.Code.Value,
            Name = client.Name,
            IsActive = client.IsActive
        };
    }
}
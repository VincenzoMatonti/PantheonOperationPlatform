using Hermes.Domain.Clients.Entities;
using Hermes.Domain.Clients.Repositories;
using Hermes.Application.Clients.DTOs.ClientOperationDtos;

namespace Hermes.Application.Clients.Commands
{
    public class ClientOperationCommandHandler(IClientOperationCommandRepository commandRepository)
    {
        public async Task<ClientOperation> CreateAsync(CreateClientOperationCommand command, CancellationToken cancellationToken = default)
        {
            var clientOperation = ClientOperation.Create(command.ClientId, command.OperationTypeId);
            await commandRepository.AddAsync(clientOperation, cancellationToken);
            return clientOperation;
        }

        public async Task Enable(ClientOperation clientOperation)
        {
            clientOperation.Enable();
            commandRepository.Update(clientOperation);
        }

        public async Task Disable(ClientOperation clientOperation)
        {
            clientOperation.Disable();
            commandRepository.Update(clientOperation);
        }

        public async Task Delete(ClientOperation clientOperation)
        {
            clientOperation.MarkAsDeleted();
            commandRepository.Update(clientOperation);
        }

        public async Task Restore(ClientOperation clientOperation)
        {
            clientOperation.Restore();
            commandRepository.Update(clientOperation);
        }

        public static CreateClientOperationDto ConvertClientOperationEntitesToCreateDto(ClientOperation clientOperation)
        {
            return new CreateClientOperationDto
            {
                Id = clientOperation.Id,
                ClientId = clientOperation.ClientId,
                OperationTypeId = clientOperation.OperationTypeId,
                IsEnabled = clientOperation.IsEnabled,
                IsDeleted = clientOperation.IsDeleted,
                CreatedAt = clientOperation.CreatedAt
            };
        }

        public static EnableClientOperationDto ConvertClientOperationEntitesToEnableDto(ClientOperation clientOperation)
        {
            return new EnableClientOperationDto
            {
                Id = clientOperation.Id,
                ClientId = clientOperation.ClientId,
                OperationTypeId = clientOperation.OperationTypeId,
                IsEnabled = clientOperation.IsEnabled,
                UpdatedAt = clientOperation.UpdatedAt
            };
        }

        public static DisableClientOperationDto ConvertClientOperationEntitesToDisableDto(ClientOperation clientOperation)
        {
            return new DisableClientOperationDto
            {
                Id = clientOperation.Id,
                ClientId = clientOperation.ClientId,
                OperationTypeId = clientOperation.OperationTypeId,
                IsEnabled = clientOperation.IsEnabled,
                UpdatedAt = clientOperation.UpdatedAt
            };
        }

        public static DeleteClientOperationDto ConvertClientOperationEntitesToDeleteDto(ClientOperation clientOperation)
        {
            return new DeleteClientOperationDto
            {
                Id = clientOperation.Id,
                ClientId = clientOperation.ClientId,
                OperationTypeId = clientOperation.OperationTypeId,
                IsDeleted = clientOperation.IsDeleted,
                UpdatedAt = clientOperation.UpdatedAt
            };
        }

        public static RestoreClientOperationDto ConvertClientOperationEntitesToRestoreDto(ClientOperation clientOperation)
        {
            return new RestoreClientOperationDto
            {
                Id = clientOperation.Id,
                ClientId = clientOperation.ClientId,
                OperationTypeId = clientOperation.OperationTypeId,
                IsDeleted = clientOperation.IsDeleted,
                UpdatedAt = clientOperation.UpdatedAt
            };
        }
    }
}
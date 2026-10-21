using Athena.Api.OrchestrationRegistry.Requests;
using Athena.Application.OrchestrationRegistry.Commands;

namespace Athena.Api.OrchestrationRegistry.Mappings;

public static class OrchestrationRegistryCommandRequestMapper
{
    public static CreateOrchestrationCommand ToCommand(CreateOrchestrationRequest request)
    {
        return new CreateOrchestrationCommand { OrchestrationId = request.OrchestrationId, Name = request.Name };
    }

    public static RegisterOrchestrationCommand ToRegisterCommand(Guid orchestrationId)
    {
        return new RegisterOrchestrationCommand { OrchestrationId = orchestrationId };
    }

    public static UnregisterOrchestrationCommand ToUnregisterCommand(Guid orchestrationId)
    {
        return new UnregisterOrchestrationCommand { OrchestrationId = orchestrationId };
    }

    public static EnableOrchestrationCommand ToEnableCommand(Guid orchestrationId)
    {
        return new EnableOrchestrationCommand { OrchestrationId = orchestrationId };
    }

    public static DisableOrchestrationCommand ToDisableCommand(Guid orchestrationId)
    {
        return new DisableOrchestrationCommand { OrchestrationId = orchestrationId };
    }

    public static RenameOrchestrationCommand ToRenameCommand(Guid orchestrationId, RenameOrchestrationRequest request)
    {
        return new RenameOrchestrationCommand { OrchestrationId = orchestrationId, Name = request.Name };
    }

    public static MarkOrchestrationAvailableCommand ToMarkAvailableCommand(Guid orchestrationId)
    {
        return new MarkOrchestrationAvailableCommand { OrchestrationId = orchestrationId };
    }

    public static MarkOrchestrationUnavailableCommand ToMarkUnavailableCommand(Guid orchestrationId)
    {
        return new MarkOrchestrationUnavailableCommand { OrchestrationId = orchestrationId };
    }

    public static MarkOrchestrationAvailabilityUnknownCommand ToMarkAvailabilityUnknownCommand(Guid orchestrationId)
    {
        return new MarkOrchestrationAvailabilityUnknownCommand { OrchestrationId = orchestrationId };
    }

    public static DeleteOrchestrationCommand ToDeleteCommand(Guid orchestrationId)
    {
        return new DeleteOrchestrationCommand { OrchestrationId = orchestrationId };
    }
}
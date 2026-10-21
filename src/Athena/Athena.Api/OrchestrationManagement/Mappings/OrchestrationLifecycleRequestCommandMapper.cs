using Athena.Api.OrchestrationManagement.Requests;
using Athena.Application.OrchestrationManagement.Commands;
using Athena.Application.OrchestrationManagement.Enums;

namespace Athena.Api.OrchestrationManagement.Mappings;

public static class OrchestrationLifecycleRequestCommandMapper
{
    public static SetOrchestrationDesiredStateCommand ToSetDesiredStateCommand(Guid orchestrationId, SetOrchestrationDesiredStateRequest request)
    {
        return new SetOrchestrationDesiredStateCommand
        {
            OrchestrationId = orchestrationId,
            DesiredState = Enum.TryParse<OrchestrationDesiredState>(request.DesiredState, true, out var state) ? state : default
        };
    }

    public static MarkOrchestrationStartingCommand ToMarkStartingCommand(Guid orchestrationId)
    {
        return new MarkOrchestrationStartingCommand { OrchestrationId = orchestrationId };
    }

    public static MarkOrchestrationRunningCommand ToMarkRunningCommand(Guid orchestrationId)
    {
        return new MarkOrchestrationRunningCommand { OrchestrationId = orchestrationId };
    }

    public static MarkOrchestrationStoppingCommand ToMarkStoppingCommand(Guid orchestrationId)
    {
        return new MarkOrchestrationStoppingCommand { OrchestrationId = orchestrationId };
    }

    public static MarkOrchestrationStoppedCommand ToMarkStoppedCommand(Guid orchestrationId)
    {
        return new MarkOrchestrationStoppedCommand { OrchestrationId = orchestrationId };
    }

    public static MarkOrchestrationFailedCommand ToMarkFailedCommand(Guid orchestrationId)
    {
        return new MarkOrchestrationFailedCommand { OrchestrationId = orchestrationId };
    }

    public static MarkOrchestrationOperationalStatusUnknownCommand ToMarkOperationalStatusUnknownCommand(Guid orchestrationId)
    {
        return new MarkOrchestrationOperationalStatusUnknownCommand { OrchestrationId = orchestrationId };
    }

    public static DeleteOrchestrationLifecycleCommand ToDeleteLifecycleCommand(Guid orchestrationId)
    {
        return new DeleteOrchestrationLifecycleCommand { OrchestrationId = orchestrationId };
    }
}

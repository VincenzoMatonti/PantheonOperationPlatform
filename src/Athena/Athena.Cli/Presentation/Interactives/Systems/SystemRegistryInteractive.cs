using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;

namespace Athena.Cli.Presentation.Interactives.Systems;

public class SystemRegistryInteractive(SystemRegistryService systemRegistryService)
{
    public async Task ListAsync(CancellationToken cancellationToken = default)
    {
        var systems = await systemRegistryService.GetAllAsync(cancellationToken);
        SystemRegistryOutput.WriteSystems(systems);
    }

    public async Task GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var system = await systemRegistryService.GetByIdAsync(id, cancellationToken);
        SystemRegistryOutput.WriteSystem(system);
    }

    public async Task GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var system = await systemRegistryService.GetByNameAsync(name, cancellationToken);
        SystemRegistryOutput.WriteSystem(system);
    }

    public async Task RegisterAsync(string name, CancellationToken cancellationToken = default)
    {
        var system = await systemRegistryService.RegisterAsync(new RegisterSystemRequest(name), cancellationToken);
        SystemRegistryOutput.WriteMessage($"System '{system.Name}' registered successfully.");
        SystemRegistryOutput.WriteSystem(system);
    }

    public async Task CreateAsync(string name, CancellationToken cancellationToken = default)
    {
        var system = await systemRegistryService.CreateAsync(new RegisterSystemRequest(name), cancellationToken);
        SystemRegistryOutput.WriteMessage($"System '{system.Name}' created successfully.");
        SystemRegistryOutput.WriteSystem(system);
    }

    public async Task RenameAsync(Guid id, string newName, CancellationToken cancellationToken = default)
    {
        var system = await systemRegistryService.RenameAsync(id, new RenameSystemRequest(newName), cancellationToken);
        SystemRegistryOutput.WriteMessage("System renamed successfully.");
        SystemRegistryOutput.WriteSystem(system);
    }

    public async Task EnableAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var system = await systemRegistryService.EnableAsync(id, cancellationToken);
        SystemRegistryOutput.WriteSystem(system);
    }

    public async Task DisableAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var system = await systemRegistryService.DisableAsync(id, cancellationToken);
        SystemRegistryOutput.WriteSystem(system);
    }

    public async Task UnregisterAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var system = await systemRegistryService.UnregisterAsync(id, cancellationToken);
        SystemRegistryOutput.WriteSystem(system);
    }

    public async Task MarkAvailableAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var system = await systemRegistryService.MarkAvailableAsync(id, cancellationToken);
        SystemRegistryOutput.WriteSystem(system);
    }

    public async Task MarkUnavailableAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var system = await systemRegistryService.MarkUnavailableAsync(id, cancellationToken);
        SystemRegistryOutput.WriteSystem(system);
    }

    public async Task MarkAvailabilityUnknownAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var system = await systemRegistryService.MarkAvailabilityUnknownAsync(id, cancellationToken);
        SystemRegistryOutput.WriteSystem(system);
    }

    public async Task AddEndpointAsync(Guid id, string name, string baseAddress, CancellationToken cancellationToken = default)
    {
        var system = await systemRegistryService.AddEndpointAsync(id, new AddSystemEndpointRequest(name, baseAddress), cancellationToken);
        SystemRegistryOutput.WriteSystem(system);
    }

    public async Task RemoveEndpointAsync(Guid id, string endpointName, CancellationToken cancellationToken = default)
    {
        var system = await systemRegistryService.RemoveEndpointAsync(id, new RemoveSystemEndpointRequest(endpointName), cancellationToken);
        SystemRegistryOutput.WriteSystem(system);
    }

    public async Task AddCapabilityAsync(Guid id, string name, string description, CancellationToken cancellationToken = default)
    {
        var system = await systemRegistryService.AddCapabilityAsync(id, new AddSystemCapabilityRequest(name, description), cancellationToken);
        SystemRegistryOutput.WriteSystem(system);
    }

    public async Task RemoveCapabilityAsync(Guid id, string capabilityName, CancellationToken cancellationToken = default)
    {
        var system = await systemRegistryService.RemoveCapabilityAsync(id, new RemoveSystemCapabilityRequest(capabilityName), cancellationToken);
        SystemRegistryOutput.WriteSystem(system);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var system = await systemRegistryService.DeleteAsync(id, cancellationToken);
        SystemRegistryOutput.WriteMessage($"System '{system.Name}' deleted successfully.");
    }
}

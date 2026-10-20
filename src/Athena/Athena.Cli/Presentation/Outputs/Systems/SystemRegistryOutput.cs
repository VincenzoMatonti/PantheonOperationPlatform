using Athena.Cli.Application.Contracts.Systems;

namespace Athena.Cli.Presentation.Outputs.Systems;

public class SystemRegistryOutput
{
    public static void WriteSystems(IEnumerable<SystemResponse> systems)
    {
        var items = systems.ToList();
        if (items.Count == 0)
        {
            Console.WriteLine("No systems found.");
            return;
        }

        Console.WriteLine($"{"ID",-36} {"NAME",-30} {"REGISTRATION",-15} {"AVAILABILITY",-15}");
        Console.WriteLine(new string('-', 102));
        foreach (var system in items)
        {
            Console.WriteLine($"{system.Id,-36} " + $"{system.Name,-30} " + $"{system.RegistrationStatus,-15} " + $"{system.AvailabilityStatus,-15}");
        }
    }

    public static void WriteSystem(SystemResponse system)
    {
        Console.WriteLine($"ID:                  {system.Id}");
        Console.WriteLine($"Name:                {system.Name}");
        Console.WriteLine($"Registration status: {system.RegistrationStatus}");
        Console.WriteLine($"Availability status: {system.AvailabilityStatus}");
        Console.WriteLine($"Created at (UTC):    {system.CreatedAtUtc:O}");
        Console.WriteLine($"Updated at (UTC):    {system.UpdatedAtUtc?.ToString("O") ?? "-"}");
        Console.WriteLine($"Deleted:             {system.IsDeleted}");
        Console.WriteLine();
        Console.WriteLine("Endpoints:");
        if (system.Endpoints.Count == 0)
        {
            Console.WriteLine("  None");
        }
        else
        {
            foreach (var endpoint in system.Endpoints)
            {
                Console.WriteLine($"  - {endpoint.Name}: {endpoint.BaseAddress}");
            }
        }

        Console.WriteLine();
        Console.WriteLine("Capabilities:");
        if (system.Capabilities.Count == 0)
        {
            Console.WriteLine("  None");
        }
        else
        {
            foreach (var capability in system.Capabilities)
            {
                Console.WriteLine($"  - {capability.Name}: {capability.Description}");
            }
        }
    }

    public static void WriteMessage(string message)
    {
        Console.WriteLine(message);
    }

    public static void WriteError(string message)
    {
        Console.Error.WriteLine($"Error: {message}");
    }
}

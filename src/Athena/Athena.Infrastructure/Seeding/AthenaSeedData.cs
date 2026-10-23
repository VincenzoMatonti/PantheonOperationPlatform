namespace Athena.Infrastructure.Seeding;

internal static class AthenaSeedData
{
    internal record SystemDefinition(string Name, Guid Id);

    internal record OrchestrationDefinition(string Name, Guid Id, string[] Systems);

    internal static readonly SystemDefinition[] Systems =
    [
        new("Hermes", Guid.Parse("10000000-0000-0000-0000-000000000001")),
        new("Hephaestus", Guid.Parse("10000000-0000-0000-0000-000000000002")),
        new("Argus", Guid.Parse("10000000-0000-0000-0000-000000000003"))
    ];

    private static readonly string[] AllSystems = ["Hermes", "Hephaestus", "Argus"];
    private static readonly string[] IntegrationsAndBackground = ["Hermes", "Hephaestus"];
    private static readonly string[] IntegrationsAndMonitoring = ["Hermes", "Argus"];
    private static readonly string[] BackgroundAndMonitoring = ["Hephaestus", "Argus"];

    internal static readonly OrchestrationDefinition[] Orchestrations =
    [
        // Start
        new("AvvioCompleto",Guid.Parse("20000000-0000-0000-0000-000000000001"),AllSystems),
        new("AvvioIntegrazioni",Guid.Parse("20000000-0000-0000-0000-000000000002"),["Hermes"]),
        new("AvvioBackgroundProcessing",Guid.Parse("20000000-0000-0000-0000-000000000003"),["Hephaestus"]),
        new("AvvioMonitoraggio",Guid.Parse("20000000-0000-0000-0000-000000000004"),["Argus"]),
        new("AvvioServizi",Guid.Parse("20000000-0000-0000-0000-000000000005"),IntegrationsAndBackground),
        new("AvvioIntegrazioniMonitoraggio",Guid.Parse("20000000-0000-0000-0000-000000000006"),IntegrationsAndMonitoring),
        new("AvvioBackgroundMonitoraggio",Guid.Parse("20000000-0000-0000-0000-000000000007"),BackgroundAndMonitoring),

        // Stop
        new("ArrestoCompleto",Guid.Parse("20000000-0000-0000-0000-000000000008"),AllSystems),
        new("ArrestoIntegrazioni",Guid.Parse("20000000-0000-0000-0000-000000000009"),["Hermes"]),
        new("ArrestoBackgroundProcessing",Guid.Parse("20000000-0000-0000-0000-000000000010"),["Hephaestus"]),
        new("ArrestoMonitoraggio",Guid.Parse("20000000-0000-0000-0000-000000000011"),["Argus"]),
        new("ArrestoServizi",Guid.Parse("20000000-0000-0000-0000-000000000012"),IntegrationsAndBackground),
        new("ArrestoIntegrazioniMonitoraggio",Guid.Parse("20000000-0000-0000-0000-000000000013"),IntegrationsAndMonitoring),
        new("ArrestoBackgroundMonitoraggio",Guid.Parse("20000000-0000-0000-0000-000000000014"),BackgroundAndMonitoring),

        // Restart
        new("RiavvioCompleto",Guid.Parse("20000000-0000-0000-0000-000000000015"),AllSystems),
        new("RiavvioIntegrazioni",Guid.Parse("20000000-0000-0000-0000-000000000016"),["Hermes"]),
        new("RiavvioBackgroundProcessing",Guid.Parse("20000000-0000-0000-0000-000000000017"),["Hephaestus"]),
        new("RiavvioMonitoraggio",Guid.Parse("20000000-0000-0000-0000-000000000018"),["Argus"]),
        new("RiavvioServizi",Guid.Parse("20000000-0000-0000-0000-000000000019"),IntegrationsAndBackground),
        new("RiavvioIntegrazioniMonitoraggio",Guid.Parse("20000000-0000-0000-0000-000000000020"),IntegrationsAndMonitoring),
        new("RiavvioBackgroundMonitoraggio",Guid.Parse("20000000-0000-0000-0000-000000000021"),BackgroundAndMonitoring)
    ];
}
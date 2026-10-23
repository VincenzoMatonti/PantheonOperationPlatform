using Athena.Api.Common.extensions;
using Athena.Application.Common.Extensions;
using Athena.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;

namespace Athena.Runtime;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAthenaApplication();
        builder.Services.AddAthenaInfrastructure(builder.Configuration);
        builder.Services.AddAthenaApi();

        var app = builder.Build();
        await app.Services.InitializeAthenaDatabaseAsync();
        app.UseAthenaApi();
        await app.RunAsync();
    }
}
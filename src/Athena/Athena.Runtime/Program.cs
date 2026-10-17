using Athena.Api.Common.extensions;
using Athena.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;

namespace Athena.Runtime;

internal static class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAthenaInfrastructure(builder.Configuration);
        builder.Services.AddAthenaApi();
        
        var app = builder.Build();
        app.UseAthenaApi();
        app.Run();
    }
}
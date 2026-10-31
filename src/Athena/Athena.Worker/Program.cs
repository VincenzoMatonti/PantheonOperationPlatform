using Athena.Application.Common.Extensions;
using Athena.Infrastructure.Extensions;
using Athena.Worker.Hosting.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddAthenaApplication();
builder.Services.AddAthenaInfrastructure(builder.Configuration);
builder.Services.AddWorkerServices();

var host = builder.Build();

await host.RunAsync();

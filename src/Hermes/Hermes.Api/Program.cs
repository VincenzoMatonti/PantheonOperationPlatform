using Hermes.Application.Clients.Commands.CreateClient;
using Hermes.Application.Clients.Commands.ActivateClient;
using Hermes.Application.Clients.Commands.DeactivateClient;
using Hermes.Application.Clients.Commands.RenameClient;
using Hermes.Application.Clients.Queries.GetClientById;
using Hermes.Application.Clients.Queries.GetClientByCode;
using Hermes.Application.Clients.Queries.GetClients;
using Hermes.Application.Clients.Queries.GetActiveClients;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<CreateClientHandler>();
builder.Services.AddScoped<ActivateClientHandler>();
builder.Services.AddScoped<DeactivateClientHandler>();
builder.Services.AddScoped<RenameClientHandler>();
builder.Services.AddScoped<GetClientByIdHandler>();
builder.Services.AddScoped<GetClientByCodeHandler>();
builder.Services.AddScoped<GetClientsHandler>();
builder.Services.AddScoped<GetActiveClientsHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
       options.SwaggerEndpoint("/openapi/v1.json", "Hermes API v1");
    });
}

// app.UseHttpsRedirection();
app.Run();

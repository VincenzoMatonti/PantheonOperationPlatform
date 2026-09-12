using FluentValidation;
using Hermes.Api.Filters;
using Hermes.Api.Clients.Validations;
using Hermes.Api.Clients.Mappings.Commands;
using Hermes.Api.Clients.Mappings.Queries;
using Hermes.Application.Clients.Commands;
using Hermes.Application.Clients.Queries;
using Hermes.Application.Clients.UseCases;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationFilter>();
});

builder.Services.AddValidatorsFromAssemblyContaining<CreateClientRequestValidator>();

builder.Services.AddScoped<ClientCommandRequestMapper>();
builder.Services.AddScoped<ClientCommandResponseMapper>();
builder.Services.AddScoped<ClientQueryRequestMapper>();
builder.Services.AddScoped<ClientQueryResponseMapper>();
builder.Services.AddScoped<ClientCommandHandler>();
builder.Services.AddScoped<ClientQueryHandler>();
builder.Services.AddScoped<ClientUseCaseHandler>();



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

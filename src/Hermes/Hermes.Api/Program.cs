using FluentValidation;
using Hermes.Api.Filters;
using Hermes.Api.Exceptions.Mappings;
using Hermes.Api.Exceptions.Builders;
using Hermes.Api.Clients.Mappings.Exceptions;
using Hermes.Api.Clients.Validations;
using Hermes.Api.Clients.Mappings.Commands;
using Hermes.Api.Clients.Mappings.Queries;
using Hermes.Api.Exceptions.Handlers;
using Hermes.Application.Clients.Commands;
using Hermes.Application.Clients.UseCases;
using Hermes.Application.Clients.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddOpenApi();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationFilter>();
});

//VALIDATION
builder.Services.AddValidatorsFromAssemblyContaining<CreateClientRequestValidator>();
builder.Services.AddScoped<IExceptionMapper, ValidationExceptionMapper>();
builder.Services.AddScoped<IExceptionMapper, InternalExceptionMapper>();
builder.Services.AddScoped<IExceptionMapper, ClientExceptionMapper>();
builder.Services.AddScoped<IExceptionMapper, ClientOperationExceptionMapper>();
builder.Services.AddScoped<ExceptionMapperResolver>();
builder.Services.AddScoped<ExceptionResponseBuilder>();

// CLIENT
builder.Services.AddScoped<ClientCommandRequestMapper>();
builder.Services.AddScoped<ClientCommandResponseMapper>();
builder.Services.AddScoped<ClientQueryRequestMapper>();
builder.Services.AddScoped<ClientQueryResponseMapper>();
builder.Services.AddScoped<ClientCommandHandler>();
builder.Services.AddScoped<ClientQueryHandler>();
builder.Services.AddScoped<ClientUseCaseHandler>();




// CLIENT OPERATION
builder.Services.AddScoped<ClientOperationCommandRequestMapper>();
builder.Services.AddScoped<ClientOperationCommandResponseMapper>();
builder.Services.AddScoped<ClientOperationQueryRequestMapper>();
builder.Services.AddScoped<ClientOperationQueryResponseMapper>();
builder.Services.AddScoped<ClientOperationCommandHandler>();
builder.Services.AddScoped<ClientOperationQueryHandler>();
builder.Services.AddScoped<ClientOperationUseCaseHandler>();


var app = builder.Build();
app.UseExceptionHandler();

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

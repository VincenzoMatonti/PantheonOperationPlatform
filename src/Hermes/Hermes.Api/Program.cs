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
using Hermes.Api.Operations.Mappings.Exceptions;
using Hermes.Api.Operations.Mappings.Commands;
using Hermes.Api.Operations.Mappings.Queries;
using Hermes.Application.Operations.Commands;
using Hermes.Application.Operations.Queries;
using Hermes.Application.Operations.UseCases;
using Hermes.Application.OperationTypes.Commands;
using Hermes.Application.OperationTypes.Queries;
using Hermes.Application.OperationTypes.UseCases;

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
builder.Services.AddScoped<IExceptionMapper, OperationExceptionMapper>();
builder.Services.AddScoped<IExceptionMapper, OperationTypeExceptionMapper>();
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

// OPERATION
builder.Services.AddScoped<OperationCommandRequestMapper>();
builder.Services.AddScoped<OperationCommandResponseMapper>();
builder.Services.AddScoped<OperationQueryRequestMapper>();
builder.Services.AddScoped<OperationQueryResponseMapper>();
builder.Services.AddScoped<OperationCommandHandler>();
builder.Services.AddScoped<OperationQueryHandler>();
builder.Services.AddScoped<OperationUseCaseHandler>();

// OPERATION TYPE
builder.Services.AddScoped<OperationTypeCommandRequestMapper>();
builder.Services.AddScoped<OperationTypeCommandResponseMapper>();
builder.Services.AddScoped<OperationTypeQueryRequestMapper>();
builder.Services.AddScoped<OperationTypeQueryResponseMapper>();
builder.Services.AddScoped<OperationTypeCommandHandler>();
builder.Services.AddScoped<OperationTypeQueryHandler>();
builder.Services.AddScoped<OperationTypeUseCaseHandler>();

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

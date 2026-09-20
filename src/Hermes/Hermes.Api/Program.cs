using FluentValidation;
using Hermes.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
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
using Hermes.Api.Endpoints.Mappings.Exceptions;
using Hermes.Api.Endpoints.Mappings.Commands;
using Hermes.Application.Endpoints.Commands;
using Hermes.Application.Endpoints.Queries;
using Hermes.Application.Endpoints.UseCases;
using Hermes.Api.Endpoints.Mappings.Queries;
using Hermes.Api.Routes.Mappings.Exceptions;
using Hermes.Api.Routes.Mappings.Commands;
using Hermes.Api.Routes.Mappings.Queries;
using Hermes.Application.Routes.Handlers;
using Hermes.Application.Routes.UseCases;
using Hermes.Domain.Clients.Repositories.ClientRepositories;
using Hermes.Infrastructure.Repositories.Clients.Queries;
using Hermes.Infrastructure.Repositories.Clients.Commands;
using Hermes.Domain.Clients.Repositories.ClientOperationRepositories;
using Hermes.Domain.Endpoints.Repositories.EndpointRepositories;
using Hermes.Domain.Endpoints.Repositories.EndpointOperationRepositories;
using Hermes.Domain.Routes.Repositories;
using Hermes.Infrastructure.Repositories.Routes;
using Hermes.Infrastructure.Repositories.Endpoints.Queries;
using Hermes.Infrastructure.Repositories.Endpoints.Commands;
using Hermes.Domain.Operations.Repositories.OperationRepositories;
using Hermes.Domain.Operations.Repositories.OperationTypeRepositories;
using Hermes.Infrastructure.Repositories.Operations.Commands;
using Hermes.Infrastructure.Repositories.Operations.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HermesDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Hermes"))
);

// PERSISTENCE - CLIENT
builder.Services.AddScoped<    IClientCommandRepository,    ClientCommandRepository>();
builder.Services.AddScoped<    IClientQueryRepository,    ClientQueryRepository>();

// PERSISTENCE - CLIENT OPERATION
builder.Services.AddScoped<    IClientOperationCommandRepository,    ClientOperationCommandRepository>();
builder.Services.AddScoped<    IClientOperationQueryRepository,    ClientOperationQueryRepository>();

// PERSISTENCE - OPERATION
builder.Services.AddScoped<IOperationCommandRepository, OperationCommandRepository>();
builder.Services.AddScoped<IOperationQueryRepository, OperationQueryRepository>();

// PERSISTENCE - OPERATION TYPE
builder.Services.AddScoped<IOperationTypeCommandRepository, OperationTypeCommandRepository>();
builder.Services.AddScoped<IOperationTypeQueryRepository, OperationTypeQueryRepository>();

// PERSISTENCE - ENDPOINT
builder.Services.AddScoped<    IEndpointCommandRepository,    EndpointCommandRepository>();
builder.Services.AddScoped<    IEndpointRepository,    EndpointQueryRepository>();

// PERSISTENCE - ENDPOINT OPERATION
builder.Services.AddScoped<    IEndpointOperationCommandRepository,    EndpointOperationCommandRepository>();
builder.Services.AddScoped<    IEndpointOperationQueryRepository,    EndpointOperationQueryRepository>();

// PERSISTENCE - ROUTE
builder.Services.AddScoped<    IRouteRepository,    RouteRepository>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationFilter>();
});

//VALIDATION
builder.Services.AddValidatorsFromAssemblyContaining<CreateClientRequestValidator>();
builder.Services.AddSingleton<IExceptionMapper, ValidationExceptionMapper>();
builder.Services.AddSingleton<IExceptionMapper, InternalExceptionMapper>();
builder.Services.AddSingleton<IExceptionMapper, ClientExceptionMapper>();
builder.Services.AddSingleton<IExceptionMapper, ClientOperationExceptionMapper>();
builder.Services.AddSingleton<IExceptionMapper, OperationExceptionMapper>();
builder.Services.AddSingleton<IExceptionMapper, OperationTypeExceptionMapper>();
builder.Services.AddSingleton<IExceptionMapper, EndpointExceptionMapper>();
builder.Services.AddSingleton<IExceptionMapper, EndpointOperationExceptionMapper>();
builder.Services.AddSingleton<IExceptionMapper, RouteExceptionMapper>();
builder.Services.AddSingleton<ExceptionMapperResolver>();
builder.Services.AddSingleton<ExceptionResponseBuilder>();

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

// ENDPOINT 
builder.Services.AddScoped<EndpointQueryRequestMapper>();
builder.Services.AddScoped<EndpointQueryResponseMapper>();
builder.Services.AddScoped<EndpointRequestCommandMapping>();
builder.Services.AddScoped<EndpointResponseCommandMapping>();
builder.Services.AddScoped<EndpointCommandHandler>();
builder.Services.AddScoped<EndpointQueryHandler>();
builder.Services.AddScoped<EndpointUseCaseHandler>();

// ENDPOINT OPERATION 
builder.Services.AddScoped<EndpointOperationRequestCommandMapping>();
builder.Services.AddScoped<EndpointOperationResponseCommandMapping>();
builder.Services.AddScoped<EndpointOperationQueryResponseMapping>();
builder.Services.AddScoped<EndpointOperationQueryRequestMapping>();
builder.Services.AddScoped<EndpointOperationCommandHandler>();
builder.Services.AddScoped<EndpointOperationQueryHandler>();
builder.Services.AddScoped<EndpointOperationUseCaseHandler>();

// ROUTE 
builder.Services.AddScoped<RouteCommandRequestMapper>();
builder.Services.AddScoped<RouteCommandResponseMapper>();
builder.Services.AddScoped<RouteQueryRequestMapper>();
builder.Services.AddScoped<RouteQueryResponseMapper>();
builder.Services.AddScoped<RouteCommandHandler>();
builder.Services.AddScoped<RouteQueryHandler>();
builder.Services.AddScoped<RouteUseCaseHandler>();

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

using Hermes.Application.Clients.Commands;
using Hermes.Application.Clients.Queries;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<ClientCommandHandler>();
builder.Services.AddScoped<ClientQueryHandler>();

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

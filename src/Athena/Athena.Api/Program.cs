using Athena.Api.Common.extensions;

var builder = WebApplication.CreateBuilder(args);

// OpenAPI document
builder.Services.AddOpenApi();

// Swagger UI support
builder.Services.AddSwaggerGen();

// Athena API services
builder.Services.AddAthenaApiCommonServices();
builder.Services.AddAthenaApiMiddleware();

var app = builder.Build();

// Global exception handling
app.UseAthenaApiMiddleware();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    // Built-in ASP.NET Core OpenAPI document
    app.MapOpenApi();

    // Swagger document and interactive UI
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Athena API v1");
        options.RoutePrefix = "swagger";
    });
}

app.MapControllers();

app.Run();

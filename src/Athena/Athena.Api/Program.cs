using Athena.Api.Common.extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddAthenaApiCommonServices();
builder.Services.AddAthenaApiMiddleware();

var app = builder.Build();
app.UseAthenaApiMiddleware();
if (app.Environment.IsDevelopment()) app.MapOpenApi();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

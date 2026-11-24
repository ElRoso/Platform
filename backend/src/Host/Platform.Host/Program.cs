using ElRoso.DependencyInjection;
using Platform.Identity.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddElRosoConfiguration(args);
builder.AddElRosoSerilog();

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Falta ConnectionStrings:Default. Exportar ConnectionStrings__Default.");

builder.Services.AddIdentityPersistence(connectionString);

builder.Services.AddElRosoForwardedHeaders(builder.Configuration);
builder.Services.AddExceptionHandlerMiddleware();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseElRosoForwardedHeaders();
app.UseExceptionHandlerMiddleware();

app.MapElRosoInfo(app.Environment);
app.MapHealthChecksCustom("/health");

app.Run();

using ElRoso.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.AddElRosoConfiguration(args);
builder.AddElRosoSerilog();

builder.Services.AddElRosoForwardedHeaders(builder.Configuration);
builder.Services.AddExceptionHandlerMiddleware();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseElRosoForwardedHeaders();
app.UseExceptionHandlerMiddleware();

app.MapElRosoInfo(app.Environment);
app.MapHealthChecksCustom("/health");

app.Run();

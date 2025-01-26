using Pingu.API.Endpoints;
using Pingu.Infrastructure;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();
app.AddUrlShortenerEndpoint();
app.UseHttpsRedirection();
app.UseExceptionHandler(new ExceptionHandlerOptions { ExceptionHandlingPath = "/error", AllowStatusCode404Response = true, });
app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();
app.MapScalarApiReference();
app.UseCors();
app.Run();
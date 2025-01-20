using Pingu.API.Endpoints;
using Pingu.Infrastructure;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();
app.AddUrlShortenerEndpoint();
app.UseHttpsRedirection();
app.UseExceptionHandler("/error");
app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();
app.MapScalarApiReference();
app.UseSerilogRequestLogging();
app.UseCors();
app.Run();
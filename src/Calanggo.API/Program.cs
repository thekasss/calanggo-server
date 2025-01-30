using Calanggo.API.Endpoints;
using Calanggo.Infrastructure;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddInfrastructure(builder.Configuration);

WebApplication app = builder.Build();

app.MapOpenApi();
app.AddUrlShortenerEndpoint();
app.UseHttpsRedirection();
app.UseExceptionHandler("/calango/api/errors");
app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();
app.MapScalarApiReference();
app.UseCors();
app.Run();
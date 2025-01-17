using Pingu.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseExceptionHandler("/error");
app.Run();
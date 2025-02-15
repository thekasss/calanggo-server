using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Calanggo.Application.Interfaces;
using Calanggo.Domain.Interfaces.Repositories;
using Calanggo.Infrastructure.Data.Context;
using Calanggo.Infrastructure.Data.Repositories;
using Calanggo.Infrastructure.Services;

using Serilog;
using Serilog.Events;

namespace Calanggo.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextConfig(configuration);
        services.AddRepositories();
        services.AddSerilogLogger();
        services.AddApplicationServices();
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddCors();
    }

    #region [Private Methods]

    private static void AddCors(this IServiceCollection services)
    {
        services.AddCors(options => options
            .AddDefaultPolicy(builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
    }

    private static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUrlShortenerService, UrlShortenerService>();
        services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
    }

    private static void AddDbContextConfig(this IServiceCollection services, IConfiguration configuration)
    {
#if DEBUG
        services.AddDbContext<CalanggoDbContext>(options => options.UseLazyLoadingProxies().UseInMemoryDatabase("CalanggoInMemoryDb"));
#else
        var connection = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<CalanggoDbContext>(options => options.UseLazyLoadingProxies().UseNpgsql(connection));
#endif
    }

    private static void AddSerilogLogger(this IServiceCollection services)
    {
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File("logs/app-.log",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate:
                    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger());
        });
        services.AddSerilog();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IShortenedUrlRepository, ShortenedUrlRepository>();
    }

    #endregion
}
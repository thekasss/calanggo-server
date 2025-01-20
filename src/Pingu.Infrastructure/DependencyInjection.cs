using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Pingu.Application.Interfaces;
using Pingu.Domain.Interfaces.Repositories;
using Pingu.Infrastructure.Data.Context;
using Pingu.Infrastructure.Data.Repositories;
using Pingu.Infrastructure.Services;

using Serilog;
using Serilog.Events;

namespace Pingu.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextConfig(configuration);
        services.AddRepositories();
        services.AddSerilogLogger();
        services.AddApplicationServices();
        services.AddCors();

        return services;
    }

    #region [Private Methods]

    private static IServiceCollection AddCors(this IServiceCollection services)
    {
        services.AddCors(options => options
            .AddDefaultPolicy(builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));

        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUrlShortenerService, UrlShortenerService>();
        services.AddScoped<IMemoryCacheService, MemoryCacheService>();
        return services;
    }

    private static IServiceCollection AddDbContextConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var conection = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<PinguDbContext>(options => options.UseNpgsql(conection));
        return services;
    }

    private static IServiceCollection AddSerilogLogger(this IServiceCollection services)
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
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger());
        });
        services.AddSerilog();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IShortenedUrlRepository, ShortenedUrlRepository>();
        return services;
    }

    #endregion
}
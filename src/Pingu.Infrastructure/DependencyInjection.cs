using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Pingu.Core.Domain.Entities;
using Pingu.Core.Domain.Interfaces.Repositories;
using Pingu.Infrastructure.Data.Context;
using Pingu.Infrastructure.Data.Repositories;

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

        return services;
    }

    #region [Private Methods]
    private static IServiceCollection AddDbContextConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PinguDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

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
        services.AddScoped<IRepository<ShortenedUrl>, Repository<ShortenedUrl>>();
        services.AddScoped<IRepository<UrlStatistics>, Repository<UrlStatistics>>();

        return services;
    }
    #endregion
}
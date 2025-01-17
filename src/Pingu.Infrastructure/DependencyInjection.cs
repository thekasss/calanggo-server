using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Pingu.Core.Domain.Entities;
using Pingu.Core.Interfaces.Repositories;
using Pingu.Infrastructure.Data.Context;
using Pingu.Infrastructure.Data.Repositories;

namespace Pingu.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PinguDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            #if DEBUG
            options.EnableDetailedErrors();
            if (options.IsConfigured)
            {
                Console.WriteLine("PinguDbContext is configured.");
            }
            else
            {
                Console.WriteLine("PinguDbContext is not configured.");
            }
            #endif
        });

        services.AddScoped<IRepository<ShortenedUrl>, Repository<ShortenedUrl>>();
        services.AddScoped<IRepository<UrlStatistics>, Repository<UrlStatistics>>();

        return services;
    }
}
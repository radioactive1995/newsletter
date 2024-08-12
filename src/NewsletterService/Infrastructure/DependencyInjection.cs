using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Infrastructure.Persistance;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IArticleRepository, ArticleRepository>();
        services.AddTransient<ISubscriberRepository, SubscriberRepository>();

        services.AddSingleton<ICacheService, CacheService>();

        services.AddDbContextFactory<NewsletterContext>((provider, options) =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            
            var connectionString = Environment.GetEnvironmentVariable("SQLCONNSTR_DefaultConnection")
                               ?? configuration.GetConnectionString("DefaultConnection");
            
            options.UseSqlServer(connectionString);
        });

        services.AddDistributedMemoryCache();

        return services;
    }
}

using Application.Interfaces;
using Infrastructure.Jobs;
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
        services.AddTransient<IEmailService, EmailService>();

        services.AddSingleton<IMemoryService, MemoryService>();
        services.AddSingleton<IEventBus, InMemoryEventBus>();

        services.AddHostedService<EventConsumerJob>();

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

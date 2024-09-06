using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Jobs;
public class EventConsumerJob(
    IEventBus inMemoryEventBus,
    IServiceProvider serviceProvider,
    ILogger<EventConsumerJob> logger) : BackgroundService
{
    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(2));


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            await foreach (var @event in inMemoryEventBus.Reader.ReadAllAsync(stoppingToken))
            {
                await using var scoped = serviceProvider.CreateAsyncScope();

                var publisher = scoped.ServiceProvider.GetRequiredService<IPublisher>();

                try
                {
                    await publisher.Publish(@event, stoppingToken);
                }

                catch (Exception ex)
                {
                    logger.LogError("An error occured during processing of an event job with type {Type} and message: {Message}", ex.GetType().Name, ex.Message);
                }
            }
        }
    }
}

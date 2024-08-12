using Application.Interfaces;
using MediatR;
using System.Threading.Channels;

namespace Infrastructure.Services;

public class InMemoryEventBus : IEventBus
{
    private readonly Channel<INotification> _channel;

    public InMemoryEventBus()
    {
        _channel = Channel.CreateUnbounded<INotification>();
    }

    public async Task PublishAsync<T>(
        T @event,
        CancellationToken cancellationToken = default)
        where T : class, INotification
    {
        await _channel.Writer.WriteAsync(@event, cancellationToken);
    }

    public ChannelReader<INotification> Reader => _channel.Reader;
}

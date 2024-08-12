using MediatR;
using System.Threading.Channels;

namespace Application.Interfaces;

public interface IEventBus
{
    Task PublishAsync<T>(
        T @event,
        CancellationToken cancellationToken = default)
        where T : class, INotification;

    ChannelReader<INotification> Reader { get; }
}

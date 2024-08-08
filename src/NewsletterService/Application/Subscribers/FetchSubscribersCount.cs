using MediatR;
using Application.Interfaces.Repositories;
using Application.Interfaces.Requests;

namespace Application.Subscribers;

public static class FetchSubscribersCount
{
    public record Query() : ICachedQuery<Response>
    {
        public string Key => $"{nameof(FetchSubscribersCount)}";
    }

    public record Response(int Count);

    public class QueryHandler(
        ISubscriberRepository subscriberRepository) : IRequestHandler<Query, Response>
    {
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var count = await subscriberRepository.GetSubscribersCount();

            return new Response(count);
        }
    }
}

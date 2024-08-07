using Domain.Subscribers;
using Application.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Subscribers;

public static class FetchSubscribersCount
{
    public record Query() : IRequest<Response>;

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

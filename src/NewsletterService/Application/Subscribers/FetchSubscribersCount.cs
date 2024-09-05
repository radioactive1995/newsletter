using MediatR;
using Application.Interfaces.Repositories;
using Application.Interfaces.Requests;
using ErrorOr;
using Domain.Common;

namespace Application.Subscribers;

public static class FetchSubscribersCount
{
    public record Query() : ICachedQuery<Result<Response>>
    {
        public string Key => $"{nameof(FetchSubscribersCount)}";
    }

    public record Response(int Count);

    public class QueryHandler(
        ISubscriberRepository subscriberRepository) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var count = await subscriberRepository.GetSubscribersCount();

            return new Response(count);
        }
    }
}

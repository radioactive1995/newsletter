using Application.Interfaces.Requests;
using Application.Interfaces.Services;
using Domain.Common;
using MediatR;

namespace Application.Behaviors;
public class CacheQueryPipelineBehavior<TRequest, TResponse>(
    ICacheService cacheService) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var result = await cacheService.GetOrAddCache(request.Key, handler: next, expiry: TimeSpan.FromDays(30));
        return result!;
    }
}

using Application.Interfaces.Repositories;
using Application.Interfaces.Requests;
using Application.Interfaces.Services;
using Domain.Articles;
using Domain.Subscribers;
using ErrorOr;
using MediatR;

namespace Application.Subscribers;

public static class SubscribeToNewsletter
{
    public record Query(string Email) : IInvalidateCacheCommand<ErrorOr<Response>>
    {
        public string[] InvalidateKeys => [new FetchSubscribersCount.Query().Key];
    }

    public record Response();

    public class QueryHandler(
        ICurrentUserService userService,
        ICacheService memoryService,
        ISubscriberRepository subscriberRepository) : IRequestHandler<Query, ErrorOr<Response>>
    {
        public async Task<ErrorOr<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var userIpAddress = userService.GetIpAddress() ?? string.Empty;
            var cooldownIsActive = await memoryService.DoesKeyExist(userIpAddress);

            if (cooldownIsActive) return Error.Validation("SubscribeToNewsletter.userIpAddress", "Cannot process request, cooldown is active");

            var subscriberExists = await subscriberRepository.DoesSubscriberExistWithEmail(request.Email);

            if (subscriberExists) return Error.Conflict("SubscribeToNewsletter.subscriberExists", "Subscriber already exists");

            var subcriber = Subscriber.CreateEntity(email: request.Email);

            await subscriberRepository.AddSubscriber(subcriber);

            await memoryService.AddCache(key: userIpAddress, value: "q", expiry: TimeSpan.FromHours(5));

            return new Response();
        }
    }
}

using Application.Interfaces;
using Domain.Articles;
using Domain.Subscribers;
using ErrorOr;
using MediatR;

namespace Application.Subscribers;

public static class SubscribeToNewsletter
{
    public record Query(string Email) : IRequest<ErrorOr<Response>>;

    public record Response();

    public class QueryHandler(
        ICurrentUserService userService,
        IMemoryService memoryService,
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

            await memoryService.StoreKeyValuePair(key: userIpAddress, value: "q", expiry: TimeSpan.FromMinutes(5));

            return new Response();
        }
    }
}

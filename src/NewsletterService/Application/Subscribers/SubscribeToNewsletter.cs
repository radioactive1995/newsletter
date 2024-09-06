using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Requests;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Subscribers;
using ErrorOr;
using MediatR;
using static Application.Articles.AddComment.EventHandler;

namespace Application.Subscribers;

public static class SubscribeToNewsletter
{
    public record Command(string Email) : IInvalidateCacheCommand<Result<Unit>>
    {
        public string[] InvalidateKeys => [new FetchSubscribersCount.Query().Key];
    }

    public record Event(string UserIpAddress, string Email) : INotification;

    public class CommandHandler(
        ICurrentUserService userService,
        ICacheService cacheService,
        ISubscriberRepository subscriberRepository,
        IEventBus eventBus) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command command, CancellationToken cancellationToken)
        {
            var userIpAddress = userService.GetIpAddress() ?? string.Empty;
            var cooldownIsActive = await cacheService.DoesKeyExist($"{nameof(SubscribeToNewsletter)}:{nameof(EventHandler)}:{nameof(ActivateCooldown)}:{userIpAddress}");
            
            if (cooldownIsActive) return Error.Validation("SubscribeToNewsletter.userIpAddress", "Cannot process request, cooldown is active");

            //var subscriberExists = await subscriberRepository.DoesSubscriberExistWithEmail(command.Email);
            //
            //if (subscriberExists) return Error.Conflict("SubscribeToNewsletter.subscriberExists", "Subscriber already exists");
            //
            //var subcriber = Subscriber.CreateEntity(email: command.Email);
            //
            //await subscriberRepository.AddSubscriber(subcriber);

            await eventBus.PublishAsync(new Event(UserIpAddress: userIpAddress, Email: command.Email), cancellationToken);

            return Unit.Value;
        }
    }

    public static class EventHandler
    {
        public class ActivateCooldown(ICacheService cacheService) : INotificationHandler<Event>
        {
            public async Task Handle(Event @event, CancellationToken cancellationToken)
            {
                await cacheService.AddCache(key: $"{nameof(SubscribeToNewsletter)}:{nameof(EventHandler)}:{nameof(ActivateCooldown)}:{@event.UserIpAddress}", value: nameof(ActivateCooldown), expiry: TimeSpan.FromSeconds(10));
            }
        }

        public class SendEmailConfirmation(IEmailService emailService) : INotificationHandler<Event>
        {
            public async Task Handle(Event @event, CancellationToken cancellationToken)
            {
                await Task.CompletedTask;
                //await emailService.SendSubscriberConfirmationMail();
            }
        }

        public class CreateSubscriber(ISubscriberRepository subscriberRepository) : INotificationHandler<Event>
        {
            public async Task Handle(Event @event, CancellationToken cancellationToken)
            {
                var subscriberExists = await subscriberRepository.DoesSubscriberExistWithEmail(@event.Email);

                if (subscriberExists) return;

                var subcriber = Subscriber.CreateEntity(email: @event.Email);

                await subscriberRepository.AddSubscriber(subcriber);
            }
        }
    }
}

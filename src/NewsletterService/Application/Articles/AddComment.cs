using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Requests;
using Application.Interfaces.Services;
using Domain.Articles;
using Domain.Common;
using ErrorOr;
using MediatR;
using static Application.Articles.AddComment.EventHandler;

namespace Application.Articles;

public static class AddComment
{
    public record Command(int ArticleId, string Content, int MaxPage, int PageSize) : IInvalidateCacheCommand<Result<Unit>>
    {
        public string[] InvalidateKeys
        {
            get
            {
                var keys = new List<string>();

                for (var i = 0; i <= MaxPage + 1; i++)
                {
                    keys.Add($"{nameof(FetchComments)}:{ArticleId}:{i}:{PageSize}");
                }

                return keys.ToArray();
            }
        }
    }

    public record Event(int ArticleId, string Content, string Oid) : INotification;

    public class CommandHandler(
        IArticleRepository articleRepository,
        ICurrentUserService userService,
        ICacheService cacheService,
        IEventBus eventBus) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command command, CancellationToken cancellationToken)
        {
            var user = userService.GetUserInformation();

            if (user == null) return Error.Unauthorized("AddCommentReply.Unauthorized", "User is not authenticated, cannot perform comment reply.");
            if (string.IsNullOrWhiteSpace(user.Oid)) return Error.Unauthorized("AddCommentReply.Unauthorized.UserId", "User is not authenticated, Oid is empty, cannot perform add comment.");

            var cooldownIsActive = await cacheService.DoesKeyExist($"{nameof(AddComment)}:{nameof(EventHandler)}:{nameof(ActivateCooldown)}:{user.Oid}");
            if (cooldownIsActive) return Error.Validation("SubscribeToNewsletter.userIpAddress", "Cannot process request, cooldown is active");

            //var article = await articleRepository.FetchArticle(command.ArticleId);
            //
            //if (article == null) return Error.NotFound("AddCommentReply.ArticleNotFound", "Article not found.");
            //
            //article.AddComment(content: command.Content, user.UserId);

            await eventBus.PublishAsync(new Event(command.ArticleId, command.Content, user.Oid));

            return Unit.Value;
        }
    }

    public static class EventHandler
    {
        public class ActivateCooldown(ICacheService cacheService) : INotificationHandler<Event>
        {
            public async Task Handle(Event @event, CancellationToken cancellationToken)
            {
                await cacheService.AddCache(key: $"{nameof(AddComment)}:{nameof(EventHandler)}:{nameof(ActivateCooldown)}:{@event.Oid}", value: nameof(ActivateCooldown), expiry: TimeSpan.FromSeconds(30));
            }
        }

        public class CreateComment(
            IArticleRepository articleRepository,
            IUserRepository userRepository) : INotificationHandler<Event>
        {
            public async Task Handle(Event @event, CancellationToken cancellationToken)
            {
                var user = await userRepository.GetUserByOid(@event.Oid);

                if (user == null) return;

                var article = await articleRepository.FetchArticle(@event.ArticleId);

                if (article == null) return;

                article.AddComment(content: @event.Content, userId: user.Id);

                await articleRepository.UpdateArticle(article);
            }
        }
    }
}

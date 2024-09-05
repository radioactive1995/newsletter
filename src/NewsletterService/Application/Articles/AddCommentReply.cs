using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Application.Articles;

//public static class AddCommentReply
//{
//    public record Command(int ParentCommentId, int ArticleId, string Content) : IRequest<ErrorOr<Unit>>;
//
//    public class CommandHandler(
//        IArticleRepository articleRepository,
//        ICurrentUserService currentUserService) : IRequestHandler<Command, ErrorOr<Unit>>
//    {
//        public async Task<ErrorOr<Unit>> Handle(Command command, CancellationToken cancellationToken)
//        {
//            var user = currentUserService.GetUserInformation();
//
//            if (user == null) return Error.Unauthorized("AddCommentReply.Unauthorized", "User is not authenticated, cannot perform comment reply.");
//            if (user.UserId == null) return Error.Unauthorized("AddCommentReply.Unauthorized.UserId", "User is not authenticated, userId is empty, cannot perform comment reply.");
//
//            var article = await articleRepository.FetchArticle(command.ArticleId);
//
//            if (article == null) return Error.NotFound("AddCommentReply.ArticleNotFound", "Article not found.");
//
//            var persistReplyResult = article.AddCommentReply(parentCommentId: command.ParentCommentId, content: command.Content, authorId: user.UserId);
//
//            return persistReplyResult switch
//            {
//                { IsError: true } => persistReplyResult.Errors,
//                { IsError: false } => Unit.Value,
//            };
//        }
//    }
//}

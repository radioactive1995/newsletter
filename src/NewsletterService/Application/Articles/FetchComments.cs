using Application.Interfaces.Repositories;
using Application.Interfaces.Requests;
using Domain.Articles;
using Domain.Common;
using ErrorOr;
using MediatR;

namespace Application.Articles;
public static class FetchComments
{
    public record Query(int ArticleId, int CurrentPage, int PageSize) : ICachedQuery<Result<Response[]>>
    {
        public string Key => $"{nameof(FetchComments)}:{ArticleId}:{CurrentPage}:{PageSize}";
    }

    public record Response(int CommentId, string Content, int UserId, string EmailsString, DateTime CreatedDate, int CommentsCount)
    {
        public string[] Emails => EmailsString.Split(',', StringSplitOptions.None);
        public string DisplayName => Math.Abs(string.Join(", ", Emails).GetHashCode()).ToString();
    }

    public class QueryHandler(IArticleRepository articleRepository) : IRequestHandler<Query, Result<Response[]>>
    {
        public async Task<Result<Response[]>> Handle(Query request, CancellationToken cancellationToken)
        {
            var comments = await articleRepository.FetchCommentsBasedOnArticle(request.ArticleId, request.CurrentPage, request.PageSize);
            
            //if (comments.Length > 0)
            //{
            //    var commentsCount = await articleRepository.FetchCommentsCount(request.ArticleId);
            //    foreach (var comment in comments)
            //    {
            //        comment.CommentsCount = commentsCount;
            //    }
            //}

            return comments;
        }
    }
}

using Domain.Articles;
using MediatR;
using Application.Interfaces.Repositories;
using Application.Interfaces.Requests;
using ErrorOr;
using Domain.Common;

namespace Application.Articles;

public static class FetchArticles
{
    public record Query(int CurrentPage, int PageSize) : ICachedQuery<Result<Response[]>>
    {
        public string Key => $"{nameof(FetchArticles)}:{CurrentPage}:{PageSize}";
    }

    public record Response(
        int Id,
        string Title,
        string MarkdownContent,
        int ArticlesCount);

    public class QueryHandler(
        IArticleRepository articleRepository) : IRequestHandler<Query, Result<Response[]>>
    {
        public async Task<Result<Response[]>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entities = await articleRepository.FetchArticles(request.CurrentPage, request.PageSize);
            var articlesCount = await articleRepository.FetchArticlesCount();

            var response = entities.Select(e => Mapping.FromEntity(e, articlesCount)).ToArray();

            return response;
        }
    }


    private static class Mapping
    {
        public static Response FromEntity(Article entity, int articlesCount)
        {
            return new Response(
                entity.Id,
                entity.Title,
                entity.MarkdownContent,
                articlesCount);
        }
    }
}

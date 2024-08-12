using Domain.Articles;
using MediatR;
using Application.Interfaces.Repositories;
using Application.Interfaces.Requests;

namespace Application.Articles;

public static class FetchArticle
{
    public record Query(int Id) : ICachedQuery<Response?>
    {
        public string Key => $"{nameof(FetchArticle)}:{Id}";
    }

    public record Response(
        int Id,
        string Title,
        string MarkdownContent,
        string Author,
        DateOnly PublishedDate);

    public class QueryHandler(
        IArticleRepository articleRepository) : IRequestHandler<Query, Response?>
    {
        public async Task<Response?> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await articleRepository.FetchArticle(id: request.Id);

            if (entity == null) return null;

            var response = Mapping.FromEntity(entity);

            return response;
        }
    }


    private static class Mapping
    {
        public static Response FromEntity(Article entity)
        {
            return new Response(
                entity.Id,
                entity.Title,
                entity.MarkdownContent,
                entity.Author,
                DateOnly.FromDateTime(entity.CreatedDate));
        }
    }
}

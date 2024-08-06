using Domain.Articles;
using MediatR;
using Application.Interfaces;

namespace Application.Articles;

public static class FetchArticle
{
    public record Query(int Id) : IRequest<Response?>;

    public record Response(
        int Id,
        string Title,
        string MarkdownContent);

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
                entity.MarkdownContent);
        }
    }
}

using Domain.Articles;
using MediatR;
using Application.Interfaces.Repositories;
using Application.Interfaces.Requests;
using ErrorOr;
using Domain.Common;

namespace Application.Articles;

public static class FetchArticle
{
    public record Query(int Id) : ICachedQuery<Result<Response>>
    {
        public string Key => $"{nameof(FetchArticle)}:{Id}";
    }

    public record Response(
        int Id,
        string Title,
        string MarkdownContent,
        string Author,
        string PublishedDate);


    public class QueryHandler(
        IArticleRepository articleRepository) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await articleRepository.FetchArticle(id: request.Id);

            if (entity == null) return Error.NotFound("FetchArticle.NotFound", $"Did not find article with id {request.Id}");

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
                entity.CreatedDate.ToString("dd MMMM yyyy"));

            //return new Response
            //{
            //    Title = entity.Title,
            //    MarkdownContent = entity.MarkdownContent,
            //    Author = entity.Author,
            //    PublishedDate = entity.CreatedDate.ToString("dd MMMM yyyy")
            //};

        }
    }
}

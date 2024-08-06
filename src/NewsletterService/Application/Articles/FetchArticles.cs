﻿using Domain.Articles;
using MediatR;
using Application.Interfaces;

namespace Application.Articles;

public static class FetchArticles
{
    public record Query(int CurrentPage, int PageSize) : IRequest<Response[]>;

    public record Response(
        int Id,
        string Title,
        string ArticleImageUrl,
        string MarkdownContent,
        int ArticlesCount);

    public class QueryHandler(
        IArticleRepository articleRepository) : IRequestHandler<Query, Response[]>
    {
        public async Task<Response[]> Handle(Query request, CancellationToken cancellationToken)
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
                entity.ArticleImageUrl,
                entity.MarkdownContent,
                articlesCount);
        }
    }
}
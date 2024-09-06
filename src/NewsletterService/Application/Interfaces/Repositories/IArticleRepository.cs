using Application.Articles;
using Domain.Articles;
using Domain.Subscribers;

namespace Application.Interfaces.Repositories;

public interface IArticleRepository
{
    Task<int> FetchCommentsCount(int articleId);
    Task<int> FetchArticlesCount();
    Task<Article[]> FetchArticles(int currentPage, int pageSize);
    Task<Article?> FetchArticle(int id);

    Task<FetchComments.Response[]> FetchCommentsBasedOnArticle(int articleId, int currentPage, int pageSize);
    Task UpdateArticle(Article article);
}

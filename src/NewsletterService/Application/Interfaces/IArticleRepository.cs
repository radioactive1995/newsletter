using Domain.Articles;

namespace Application.Interfaces;

public interface IArticleRepository
{
    Task<int> FetchArticlesCount();
    Task<Article[]> FetchArticles(int currentPage, int pageSize);
    Task<Article?> FetchArticle(int id);
}

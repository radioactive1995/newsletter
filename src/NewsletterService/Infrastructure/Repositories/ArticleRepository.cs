using Application.Interfaces.Repositories;
using Domain.Articles;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ArticleRepository(
    IDbContextFactory<NewsletterContext> dbFactory) : IArticleRepository
{
    public async Task<Article?> FetchArticle(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Articles.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Article[]> FetchArticles(int currentPage, int pageSize)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        int skipCount = (currentPage - 1) * pageSize;
        return await db.Articles.Skip(skipCount).Take(pageSize).ToArrayAsync();
    }

    public async Task<int> FetchArticlesCount()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Articles.CountAsync();
    }
}

using Application.Articles;
using Application.Interfaces.Repositories;
using Domain.Articles;
using Domain.Users;
using Infrastructure.Persistance;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

//Continue with this comment: Wow, this is amazing article, thank you!
// 


public class ArticleRepository(
    IDbContextFactory<NewsletterContext> dbFactory) : IArticleRepository
{
    public async Task<Article?> FetchArticle(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Articles.Include(e => e.Comments).FirstOrDefaultAsync(x => x.Id == id);
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

    public async Task<int> FetchCommentsCount(int articleId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Articles.Where(e => e.Id == articleId).SelectMany(e => e.Comments).CountAsync();
    }

    public async Task<FetchComments.Response[]> FetchCommentsBasedOnArticle(int articleId, int currentPage, int pageSize)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        int offset = (currentPage - 1) * pageSize;

        var sql = @"
        SELECT 
            c.Id AS CommentId,
            c.Content,
            c.UserId,
            u.Emails AS EmailsString,
            c.CreatedDate,
            COUNT(*) OVER() AS CommentsCount
        FROM 
            Comments c
        INNER JOIN 
            Users u ON c.UserId = u.Id
        WHERE 
            c.ArticleId = @ArticleId
        ORDER BY 
            c.CreatedDate DESC
        OFFSET @Offset ROWS 
        FETCH NEXT @PageSize ROWS ONLY";
        
        var comments = await db.Database.SqlQueryRaw<FetchComments.Response>(sql,
            new SqlParameter("@ArticleId", articleId),
            new SqlParameter("@Offset", offset),
            new SqlParameter("@PageSize", pageSize))
            .AsNoTracking()
            .ToArrayAsync();

        return comments ?? Array.Empty<FetchComments.Response>();
    }

    public async Task UpdateArticle(Article article)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        db.Articles.Update(article);
        await db.SaveChangesAsync();
    }
}

using Application.Interfaces;
using Domain.Articles;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
//public class ArticleRepository : IArticleRepository
//{
//    private Article[] articles = [
//        new Article
//        {
//            Id = 1,
//            Title = "Why coding makes you smarter",
//            MarkdownContent = @"
//# Introduction to C#
//
//C# is a modern, object-oriented programming language developed by Microsoft. It is widely used for developing web applications, desktop applications, and more.
//
//## Features of C#
//
//- Strongly Typed
//- Object-Oriented
//- Interoperable
//
//### Sample Code
//
//```csharp
//using System;
//
//public class HelloWorld
//{
//    public static void Main()
//    {
//        Console.WriteLine(""Hello, World!"");
//    }
//}"
//        },
//
//
//
//        new Article
//        {
//            Id = 2,
//            Title = "Blazor is the C# based frontend framework",
//            MarkdownContent = "#Dummy title2!"
//        },
//        new Article
//        {
//            Id = 3,
//            Title = "Coding is fun!",
//            MarkdownContent = "#Dummy title3!"
//        },
//        new Article
//        {
//            Id = 4,
//            Title = "Why coding makes you smarter",
//            MarkdownContent = "#Dummy title!"
//        },
//        new Article
//        {
//            Id = 5,
//            Title = "Blazor is the C# based frontend framework",
//            MarkdownContent = "#Dummy title2!"
//        },
//        new Article
//        {
//            Id = 6,
//            Title = "Coding is fun!",
//            MarkdownContent = "#Dummy title3!"
//        },
//        new Article
//        {
//            Id = 7,
//            Title = "Why coding makes you smarter",
//            MarkdownContent = "#Dummy title!"
//        },
//        new Article
//        {
//            Id = 8,
//            Title = "Blazor is the C# based frontend framework",
//            MarkdownContent = "#Dummy title2!"
//        },
//        new Article
//        {
//            Id = 9,
//            Title = "Coding is fun!",
//            MarkdownContent = "#Dummy title3!"
//        },
//        new Article
//        {
//            Id = 10,
//            Title = "Why coding makes you smarter",
//            MarkdownContent = "#Dummy title!"
//        },
//        new Article
//        {
//            Id = 11,
//            Title = "Blazor is the C# based frontend framework",
//            MarkdownContent = "#Dummy title2!"
//        },
//        new Article
//        {
//            Id = 12,
//            Title = "Coding is fun!",
//            MarkdownContent = "#Dummy title3!"
//        },
//        new Article
//        {
//            Id = 13,
//            Title = "Coding is fun!",
//            MarkdownContent = "#Dummy title3!"
//        }
//    ];
//
//    public Task<int> FetchArticlesCount()
//    {
//        return Task.FromResult(articles.Length);
//    }
//
//    Task<Article[]> IArticleRepository.FetchArticles(int currentPage, int pageSize)
//    {
//        int skipCount = (currentPage - 1) * pageSize;
//        var resultArticles = articles.Skip(skipCount).Take(pageSize).ToArray();
//        return Task.FromResult(resultArticles);
//    }
//
//    public Task<Article?> FetchArticle(int id)
//    {
//        return Task.FromResult(articles.FirstOrDefault(article => article.Id == id));
//    }
//}

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

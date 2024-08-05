using Domain.Articles;

namespace Web.Core;

public interface IArticleRepository
{
    Task<int> FetchArticlesCount();
    Task<Article[]> FetchArticles(int currentPage, int pageSize);
    Task<Article?> FetchArticle(int id);
}

public class ArticleRepository : IArticleRepository
{
    private Article[] articles = [
        new Article
        {
            Id = 1,
            Title = "Why coding makes you smarter",
            ArticleImageUrl = "./c#_image.webp",
            MarkdownContent = @"
# Introduction to C#

C# is a modern, object-oriented programming language developed by Microsoft. It is widely used for developing web applications, desktop applications, and more.

## Features of C#

- Strongly Typed
- Object-Oriented
- Interoperable

### Sample Code

```csharp
using System;

public class HelloWorld
{
    public static void Main()
    {
        Console.WriteLine(""Hello, World!"");
    }
}"
        },



        new Article
        {
            Id = 2,
            Title = "Blazor is the C# based frontend framework",
            ArticleImageUrl = "https://cdn.worldvectorlogo.com/logos/blazor.svg",
            MarkdownContent = "#Dummy title2!"
        },
        new Article
        {
            Id = 3,
            Title = "Coding is fun!",
            ArticleImageUrl = "https://www.svgrepo.com/show/16272/programming-code.svg",
            MarkdownContent = "#Dummy title3!"
        },
        new Article
        {
            Id = 4,
            Title = "Why coding makes you smarter",
            ArticleImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Elon_Musk_Royal_Society_%28crop2%29.jpg/1200px-Elon_Musk_Royal_Society_%28crop2%29.jpg",
            MarkdownContent = "#Dummy title!"
        },
        new Article
        {
            Id = 5,
            Title = "Blazor is the C# based frontend framework",
            ArticleImageUrl = "https://img1.spoki.lv/upload/articles/40/409044/images/trolololo-1.png",
            MarkdownContent = "#Dummy title2!"
        },
        new Article
        {
            Id = 6,
            Title = "Coding is fun!",
            ArticleImageUrl = "https://www.developer.com/wp-content/uploads/2021/09/Java-tutorials.jpg",
            MarkdownContent = "#Dummy title3!"
        },
        new Article
        {
            Id = 7,
            Title = "Why coding makes you smarter",
            ArticleImageUrl = "https://storage.googleapis.com/pod_public/1300/168838.jpg",
            MarkdownContent = "#Dummy title!"
        },
        new Article
        {
            Id = 8,
            Title = "Blazor is the C# based frontend framework",
            ArticleImageUrl = "https://img.freepik.com/free-photo/view-computer-monitor-with-gradient-display_23-2150757379.jpg",
            MarkdownContent = "#Dummy title2!"
        },
        new Article
        {
            Id = 9,
            Title = "Coding is fun!",
            ArticleImageUrl = "https://cdn.mos.cms.futurecdn.net/z36tiyqbpJ7PYx8bafhks3.jpg",
            MarkdownContent = "#Dummy title3!"
        },
        new Article
        {
            Id = 10,
            Title = "Why coding makes you smarter",
            ArticleImageUrl = "https://cdn.mos.cms.futurecdn.net/23052f5829479b8c9f7185d0d5d74aa2.jpg",
            MarkdownContent = "#Dummy title!"
        },
        new Article
        {
            Id = 11,
            Title = "Blazor is the C# based frontend framework",
            ArticleImageUrl = "https://www.elon.no/media/catalog/product/cache/61900c822c171e170ec7e59ab89edbca/1/1/119758_1_cab56f0ae7906041cde6cb2717f588b8.jpeg",
            MarkdownContent = "#Dummy title2!"
        },
        new Article
        {
            Id = 12,
            Title = "Coding is fun!",
            ArticleImageUrl = "https://www.investopedia.com/thmb/5-hnhHpOzLM2GVXPlSstg8tJYLw=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/dotdash_Final_Neural_Network_Apr_2020-01-5f4088dfda4c49d99a4d927c9a3a5ba0.jpg",
            MarkdownContent = "#Dummy title3!"
        },
        new Article
        {
            Id = 13,
            Title = "Coding is fun!",
            ArticleImageUrl = "https://www.electronics-tutorials.ws/wp-content/uploads/2022/08/binary-numbers.jpg",
            MarkdownContent = "#Dummy title3!"
        }
    ];

    public Task<int> FetchArticlesCount()
    {
        return Task.FromResult(articles.Length);
    }

    Task<Article[]> IArticleRepository.FetchArticles(int currentPage, int pageSize)
    {
        int skipCount = (currentPage - 1) * pageSize;
        var resultArticles = articles.Skip(skipCount).Take(pageSize).ToArray();
        return Task.FromResult(resultArticles);
    }

    public Task<Article?> FetchArticle(int id)
    {
        return Task.FromResult(articles.FirstOrDefault(article => article.Id == id));
    }
}

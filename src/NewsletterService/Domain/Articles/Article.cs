using Domain.Common;

namespace Domain.Articles;

public class Article : Entity<int>
{
    public required string Title { get; set; }
    public required string MarkdownContent { get; set; }
    public required string Author { get; set; }
}

namespace Domain.Articles;

public class Article
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string ArticleImageUrl { get; set; }
    public required string MarkdownContent { get; set; }
}

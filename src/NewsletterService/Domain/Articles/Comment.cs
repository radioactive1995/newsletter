using Domain.Common;

namespace Domain.Articles;
public class Comment : Entity<int>
{
    public required string Content { get; set; }
    public required int ArticleId { get; set; }
    public Article? Article { get; set; }
    public required int UserId { get; set; }

    public static Comment CreateEntity(string content, int articleId, int userId)
    {
        return new Comment
        {
            Content = content,
            ArticleId = articleId,
            UserId = userId,
        };
    }
}

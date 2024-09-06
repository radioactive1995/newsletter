using Domain.Common;
using ErrorOr;

namespace Domain.Articles;

public class Article : Entity<int>
{
    public required string Title { get; set; }
    public required string MarkdownContent { get; set; }
    public required string Author { get; set; }
    public List<Comment> Comments { get; set; }

    public Article()
    {
        Comments ??= new();
    }

    public void AddComment(string content, int userId)
    {
        var comment = Comment.CreateEntity(content, Id, userId);

        Comments.Add(comment);
    }

    //public ErrorOr<ValueTuple> AddCommentReply(int parentCommentId, string content, string authorId)
    //{
    //    var comment = Comments.Find(c => c.Id == parentCommentId);
    //
    //    if (comment is null) return Error.NotFound("Article.AddCommentReply", "Did not find a parent comment based on the provided id for adding reply to.");
    //
    //    comment.AddReply(content, authorId);
    //
    //    return ValueTuple.Create();
    //}
}

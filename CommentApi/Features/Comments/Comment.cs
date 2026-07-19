namespace CommentApi.Features.Comments
{
    public class Comment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string PostId { get; set; }
        public required string AuthorName { get; set; }
        public string? AuthorEmail { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid? ParentCommentId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
